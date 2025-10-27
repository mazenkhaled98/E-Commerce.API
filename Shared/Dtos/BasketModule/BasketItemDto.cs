using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.BasketModule
{
    public record BasketItemDto
    {
        public int Id { get; init; }
        public string ProductName { get; init; }= string.Empty;

        [Range(1, double.MaxValue)]
        public decimal Price { get; init; }
        public int Quantity { get; init; }
        [Range(1, 99)]
        public string PictureUrl { get; init; }= string.Empty;
    }
}