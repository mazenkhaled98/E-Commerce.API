using Domain.Entites.OrderModule;
using System.Linq.Expressions;

namespace Services.Specifications
{
    internal class OrderWithIncludeSpecifications : BaseSpecifications<Order, Guid>
    {
        //Get Order By Id ==> cretria ==> id == o.Id ==> Includes (DeliveryMethod , OrderItems)
        public OrderWithIncludeSpecifications(Guid id) : base(o => o.Id == id)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
        }

        //Get All OrdersByEmail ==> cretria ==> email == o.email ==> Inclues (DeliveryMethod , orderItems)
        public OrderWithIncludeSpecifications(string userEmail) : base(o => o.UserEmail == userEmail)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
            AddOrderBy(o => o.OrderDate);
        }
    }
}
