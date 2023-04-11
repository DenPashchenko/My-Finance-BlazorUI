using AutoMapper;
using MyFinance.Application.Categories.Commands.UpdateCategory;
using MyFinance.Application.Common.Mappings;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.WebBlazorUI.Models.CategoryDtos
{
	public class UpdateCategoryDto : IMapWith<UpdateCategoryCommand>
	{
		[Required]
		[StringLength(50, MinimumLength = 2, ErrorMessage = "Name length can't be more then 50 and less then 2.")]
		public string Name { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<UpdateCategoryDto, UpdateCategoryCommand>();
		}
	}
}
