namespace Domain.Exceptions
{
    public class DeliveryMethodNotFoundException : NotFoundException
    {
        public DeliveryMethodNotFoundException(int id) : base($"delivery method of id {id} is not found")
        {
        }
    }
}
