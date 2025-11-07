using AutoMapper;
using Domain.Contracts;
using Domain.Entites.BasketModule;
using Domain.Entites.OrderModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared.Dtos.BasketModule;
using Stripe;
using Order = Domain.Entites.OrderModule.Order;
using Product= Domain.Entites.ProductModule.Product;

namespace Services.Implementations
{
    public class PaymentService(IConfiguration _configuration
            , IBasketRepository _basketRepository,IUnitOfWork _unitOfWork, IMapper _mapper) : IPaymentService 
    {
        //public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId )
        //{
        //    //0] install package stripe.net


        //    //1] set up stripe api key[secret key]
        //    Stripe.StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];

        //    //2] get basket from repo
        //    var basket =await  _basketRepository.GetBasketAsync(basketId)?? throw new BasketNotFoundException(basketId);


        //    //3] validate item prtice [basket.item.price vs product.price from db]
        //    foreach (var item in basket.BasketItems)
        //    {
        //        var product = await _unitOfWork.GetRepository<Product, int>()
        //            .GetByIdAsync(item.Id)??throw new ProductNotFoundException(item.Id);
        //        item.Price = product.Price;

        //    }

        //    if (!basket.DeliveryMethodId.HasValue) throw new Exception("No delivery method selected");
        //    var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
        //        .GetByIdAsync(basket.DeliveryMethodId.Value) ??
        //        throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
        //    basket.ShippingPrice = deliveryMethod.Price;


        //    //5) Total ==> [SubTotal + ShippingPrice] ==> cent ==> * 100 ==> Long
        //    //     ==> (long) ((basket.items.q * basket.items.price) + shippingPrice [DeliveryMethod.Price]) * 100
        //    var amount = (long)(basket.BasketItems.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;


        //    //6) Create or update paymentIntent
        //    var stripeService = new PaymentIntentService();
        //    if (string.IsNullOrEmpty(basket.PaymentIntentId))
        //    {
        //        //create
        //        var options = new PaymentIntentCreateOptions()
        //        {
        //            Amount = amount, //total = subtotal + shippingPrice
        //            Currency = "USD", //dollar
        //            PaymentMethodTypes = ["card"]
        //        };
        //        var paymentIntent = await stripeService.CreateAsync(options);
        //        basket.PaymentIntentId = paymentIntent.Id;
        //        basket.ClientSecret = paymentIntent.ClientSecret;
        //    }
        //    else
        //    {
        //        var options = new PaymentIntentUpdateOptions()
        //        {
        //            Amount = amount
        //        };
        //        await stripeService.UpdateAsync(basket.PaymentIntentId, options);
        //    }
        //    //7) Save changes [Update] Basket
        //    await _basketRepository.CreateOrUpdateBasketAsync(basket);
        //    //8) Map to basketDto ==> return
        //    return _mapper.Map<BasketDto>(basket);


        ////}


        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];
            var basket = await GetBasketAsync(basketId);
            await ValidateBasketAsync(basket);
            var amount = CalculateTotalAsync(basket);
            await CreateOrUpdatePaymentIntentAsync(basket, amount);
            await _basketRepository.CreateOrUpdateBasketAsync(basket);
            return _mapper.Map<BasketDto>(basket);
        }
        private async Task CreateOrUpdatePaymentIntentAsync(CustomerBasket basket, long amount)
        {
            var stripeService = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                //create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = amount, //total = subtotal + shippingPrice
                    Currency = "USD", //dollar
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await stripeService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount
                };
                await stripeService.UpdateAsync(basket.PaymentIntentId, options);
            }
        }
        private long CalculateTotalAsync(CustomerBasket basket)
        {
            var amount = (long)(basket.Items.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;
            return amount;
        }

        private async Task ValidateBasketAsync(CustomerBasket basket)
        {
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }

            if (!basket.DeliveryMethodId.HasValue) throw new Exception("No delivery method selected");
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId.Value) ??
                throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;
        }

        private async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            return await _basketRepository.GetBasketAsync(basketId)
                ?? throw new BasketNotFoundException(basketId);
        }

        public async Task UpdatePaymentStatusAsync(string json, string signatureHeader)
        {
             string endpointSecret = _configuration.GetSection("StripeSettings")["EndPointSecret"];
          
                var stripeEvent = EventUtility.ParseEvent(json,throwOnApiVersionMismatch :false);

                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret ,throwOnApiVersionMismatch :false);
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            // Handle the event
            // If on SDK version < 46, use class Events instead of EventTypes
            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                //Change order payment status ==> paymentRecieved
                await UpdatePaymentStatusRecievedAsync(paymentIntent.Id);
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                //Change order payment status ==> paymentFailed
                await UpdatePaymentStatusFailedAsync(paymentIntent.Id);
            }
            // ... handle other event types
            else
                {
                    // Unexpected event type
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
 
            
        }

        private async Task UpdatePaymentStatusFailedAsync(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo
                .GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
            if (order is not null)
            {
                order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
                orderRepo.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private async Task UpdatePaymentStatusRecievedAsync(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo
                .GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
            if (order is not null)
            {
                order.PaymentStatus = OrderPaymentStatus.PaymentRecieved;
                orderRepo.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
