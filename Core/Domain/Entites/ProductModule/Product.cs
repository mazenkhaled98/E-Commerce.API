namespace Domain.Entites.ProductModule
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; }= null!;

        public decimal Price { get; set; }
        public string PictureUrl { get; set; } = null!;
        public ProductType ProductType { get; set; }
        public int TypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public int BrandId { get; set; }



    }
}
