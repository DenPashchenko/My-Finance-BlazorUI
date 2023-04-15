using AutoMapper;
using MyFinance.Application.Common.Mappings;
using MyFinance.Domain;

namespace MyFinance.Application.Categories.Queries.GetCategoryList
{
    public class CategoryListDTO : IMapWith<Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryListDTO>();
        }
    }
}
