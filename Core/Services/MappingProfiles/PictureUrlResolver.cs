using AutoMapper;
using Domain.Entites.ProductModule;
using Microsoft.Extensions.Configuration;
using Shared.Dtos;

namespace Services.MappingProfiles
{
    internal class PictureUrlResolver(IConfiguration _configurations) : IValueResolver<Product, ProductResultDto, string>
    {
        public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
        {
            

            if (string.IsNullOrEmpty(source.PictureUrl))
            {
                return string.Empty;
            }
            return $"{_configurations.GetSection("URLS")["BaseUrl"]}{source.PictureUrl}";


        }
    }
}
