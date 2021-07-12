using Microsoft.AspNetCore.Mvc;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Constants;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Helpers;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using MusicKG.WebApi.Contract.BindingModels;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Category  controller.
    /// </summary>
    [Route("api/Workspace/{workspaceId}/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        /// <summary>
        /// Category controller constructor.
        /// </summary>
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Get a category by id.
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{categoryId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CategoryListItemViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator, UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.ReadOnly })]
        public async Task<CategoryListItemViewModel> Get(
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string categoryId)
        {
            var result = await categoryService.GetOneAsync(workspaceId, categoryId);
            return new CategoryListItemViewModel()
            {
                Id = result.Id.ToString(),
                Name = result.Name.ToString(),
                WorkspaceId = result.WorkspaceId.ToString(),
            };
        }

        /// <summary>
        /// Get category list.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="keyword"></param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>category object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CategoryListItemViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator, UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.ReadOnly })]
        public async Task<PaginationViewModel<CategoryListItemViewModel>> GetCategories(
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromQuery] string keyword,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (totalCount, categories) = await categoryService.GetCategoriesAsync(workspaceId, keyword, from, size);

            return new PaginationViewModel<CategoryListItemViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = categories.Count(),
                Items = categories.Select(u => new CategoryListItemViewModel()
                {
                    Id = u.Id,
                    WorkspaceId = u.WorkspaceId,
                    Name = u.Name
                })
            };
        }

        /// <summary>
        /// Get All Categories.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <returns>CategoryListItemViewModel List.</returns>
        [HttpGet]
        [Route("All")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CategoryListItemViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator, UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.ReadOnly })]
        public async Task<List<CategoryListItemViewModel>> GetAllCategories(
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId)
        {
            var allCategories =  await categoryService.GetAllAsync(workspaceId);
            return allCategories.Select(x => new CategoryListItemViewModel()
            {
                Id = x.Id.ToString(),
                Name = x.Name.ToString(),
                WorkspaceId = x.WorkspaceId.ToString(),
            }).ToList();
        }

        /// <summary>
        /// Create multiple categories.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="names">category names to create.</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator, UserRoleEnum.Manager, UserRoleEnum.Annotator })]
        public async Task<bool> CreateCategories(
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromBody][RequiredLocalized][MinLengthLocalized(1)] List<string> names)
        {
            var currentUser = HttpContextHelper.GetCurrentUser(HttpContext);
            var success = await categoryService.CreateManyAsync(workspaceId, currentUser.Item1, names);

            return success;
        }


        [HttpPost]
        [Route("One")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategoryViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator, UserRoleEnum.Manager, UserRoleEnum.Annotator })]
        public async Task<CategoryViewModel> CreateAsync(
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromBody] CategoryBindingModel binding)
        {
            var currentUser = HttpContextHelper.GetCurrentUser(HttpContext);
            var serviceModel = new CategoryServiceModel()
            {
                WorkspaceId = workspaceId,
                Id = binding.Id,
                Name = binding.Name,
                CreatedBy = currentUser.Item1
            };
            var category = await categoryService.CreateOneAsync(serviceModel);

            return ServiceModelToCategoryViewModel(category);
        }

        private CategoryViewModel ServiceModelToCategoryViewModel(CategoryServiceModel serviceModel)
        {
            var result = new CategoryViewModel()
            {
                Id = serviceModel.Id,
                WorkspaceId = serviceModel.WorkspaceId,
                Name = serviceModel.Name,
                CreatedAt = serviceModel.CreatedAt,
                CreatedBy = serviceModel.CreatedBy
            };

            return result;
        }

        /// <summary>
        /// Update Category.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="categoryId">Category ID.</param>
        /// <param name="name"> Category update name.</param>
        /// <returns>Category object.</returns>
        [HttpPut]
        [Route("{categoryId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategoryViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Administrator, UserRoleEnum.Manager, UserRoleEnum.Annotator })]
        public async Task<CategoryViewModel> UpdateCategory(
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string categoryId,
            [FromForm][RequiredLocalized] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                ErrorHelper.ThrowException(MusicKGMessages.HondaSyndromeNameEmtpyMessage, HttpStatusCode.BadRequest);

            var category = await categoryService.UpdateAsync(workspaceId, categoryId, name);

            return ServiceModelToCategoryViewModel(category);
        }
    }
}
