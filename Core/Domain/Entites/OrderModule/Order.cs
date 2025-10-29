using ShippingAddress = Domain.Entites.OrderModule.Address;
namespace Domain.Entites.OrderModule;

public class Order : BaseEntity<Guid>
{
    public Order()
    {

    }
    public Order(string userEmail, ShippingAddress shippingAddress, ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod, decimal subTotal)
    {
        UserEmail = userEmail;
        ShippingAddress = shippingAddress;
        OrderItems = orderItems;
        DeliveryMethod = deliveryMethod;
        SubTotal = subTotal;
    }

    public string UserEmail { get; set; } = string.Empty;

    public ShippingAddress ShippingAddress { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;

    public DeliveryMethod DeliveryMethod { get; set; }

    public int? DeliveryMethodId { get; set; }

    public decimal SubTotal { get; set; } //SubTotal = OrderItem * quantity * price
                                          //Total = Subtotal + deliveryPrice

    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

    public string PaymentIntentId { get; set; } = string.Empty;
}