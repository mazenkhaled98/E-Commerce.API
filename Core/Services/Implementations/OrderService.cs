using AutoMapper;
using Domain.Contracts;
using Domain.Entites.BasketModule;
using Domain.Entites.OrderModule;
using Domain.Entites.ProductModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Shared.Dtos.OrderModule;


namespace Services.Implementations
{
    internal class OrderService(IMapper _mapper,IBasketRepository _basketRepository,IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail)
        {
            //1] Map addressDto to address
            var address = _mapper.Map<Address>(orderRequest.ShippingAddress);
            //2] GetOrderItems ==> BasketId ==> Basket ==> BasketItems [Id]
            var basket = await _basketRepository.GetBasketAsync(orderRequest.BasketId)
                ?? throw new BasketNotFoundException(orderRequest.BasketId);
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.GetRepository<Product, int>()
                    .GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(product,item));
            }
            //3] GetDeliveryMethod ==> DeliverMethodId ==> DB
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(orderRequest.DeliveryMethodId)
                ?? throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);

            //4] Calculate SubTotal ==> OrderItems ==> OrderItem.Q * OrderItem.Price
            var subTotal = orderItems.Sum(o => o.Price * o.Quantity);

            //5] Create obj from order ==> params , Add DB , Save Changes
            var orderToCreate = new Order(userEmail, address, orderItems, deliveryMethod, subTotal);
            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(orderToCreate);
            await _unitOfWork.SaveChangesAsync();

            //6] Map <Order , OrderResult>
            return _mapper.Map<OrderResult>(orderToCreate);
        }

        private OrderItem CreateOrderItem(Product product, BasketItem item)
        {
            var productInOrderItem = new ProductInOrderItem(product.Id, product.Name, product.PictureUrl);
            return new OrderItem(productInOrderItem, product.Price, item.Quantity);
        }


        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userEmail)
        {
            throw new NotImplementedException();
        }
    }
}
