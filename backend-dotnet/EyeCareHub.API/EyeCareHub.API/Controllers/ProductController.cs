using AutoMapper;
using EyeCareHub.API.Dtos.Product;
using EyeCareHub.API.Errors;
using EyeCareHub.API.Helper;
using EyeCareHub.BLL.Interface;
using EyeCareHub.BLL.specifications.Product_Specifications;
using EyeCareHub.DAL.Data;
using EyeCareHub.DAL.Entities.ProductInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EyeCareHub.API.Controllers
{
    public class ProductController : BaseApiController
    {

        #region Inject

        private readonly IGenericRepository<StoreContext, Products> Repo;
        private readonly IGenericRepository<StoreContext, ProductBrands> Brands;
        private readonly IGenericRepository<StoreContext, ProductTypes> Types;
        private readonly IGenericRepository<StoreContext, ProductCategory> Categories;
        private readonly IMapper mapper;


        public ProductController(IGenericRepository<StoreContext, Products> Repo,
            IGenericRepository<StoreContext,ProductBrands> Brands,
            IGenericRepository<StoreContext, ProductTypes> Types,
            IGenericRepository<StoreContext, ProductCategory> Categories,
            IMapper mapper)
        {
            this.Repo = Repo;
            this.Brands = Brands;
            this.Types = Types;
            this.Categories = Categories;
            this.mapper = mapper;
        }


        #endregion

        #region GetProductById
        [CachedResponse(600)]
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<ProductToReturn>> GetProduct(int Id)
        {
            if (Id < 0)
            {
                return BadRequest(new ApiValidationErrorResponse());
            }

            var spec = new ProductWithTypeAndBrandSpec(Id);
            var products = await Repo.GetByIdWithspecAsync(spec); // rrrr
            if (products == null)
                return NotFound(new ApiResponse(404));

            return Ok(mapper.Map<Products, ProductToReturn>(products));
        }

        #endregion

        #region GetAllProducts
        [CachedResponse(600)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pagination<ProductToReturn>>> GetProduct([FromQuery] ProductSpecParams productparams)
        {
            var spec = new ProductWithTypeAndBrandSpec(productparams);
            var products = await Repo.GetAllWithspecAsync(spec);
            if (products == null)
                return NotFound(new ApiResponse(404));
            var data = mapper.Map<IReadOnlyList<Products>, IReadOnlyList<ProductToReturn>>(products);
            var countspec = new CountProductspec(productparams);
            var count = await Repo.GetCountAsync(countspec);

            return Ok(new Pagination<ProductToReturn>(productparams.PageIndex, productparams.PageSize, count, data));
        }

        #endregion

        #region GetTypes
        [CachedResponse(600)]
        [HttpGet("Get-Types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)] 
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetTypes()
        {
            var types = await Types.GetAllAsync();
            if (types == null)
                return NotFound(new ApiResponse(404));
            var data = mapper.Map<TypeDto>(types);
            return Ok(data);
        }
        #endregion

        #region GetBrands
        [CachedResponse(600)]
        [HttpGet("Get-Brands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetBrands()
        {
            var brands = await Brands.GetAllAsync();
            if (brands == null)
                return NotFound(new ApiResponse(404));
            var data = mapper.Map<BrandDto>(brands);
            return Ok(data);
        }
        #endregion

        #region GetCategory
        [CachedResponse(600)]
        [HttpGet("Get-Category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategory()
        {
            var categories = await Categories.GetAllAsync();
            if (categories == null)
                return NotFound(new ApiResponse(404));
            var data = mapper.Map<CategoryDto>(categories);
            return Ok(data);
        }
        #endregion

    }
}
