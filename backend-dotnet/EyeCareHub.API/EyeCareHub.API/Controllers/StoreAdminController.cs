using AutoMapper;
using EyeCareHub.API.Dtos.ContentEducations;
using EyeCareHub.API.Dtos.OrderDtos;
using EyeCareHub.API.Dtos.Product;
using EyeCareHub.API.Errors;
using EyeCareHub.API.Helper;
using EyeCareHub.BLL.Interface;
using EyeCareHub.BLL.Repositories;
using EyeCareHub.BLL.specifications.Order_Specifications;
using EyeCareHub.DAL.Entities.Content_Education;
using EyeCareHub.DAL.Entities.OrderAggregate;

using EyeCareHub.DAL.Entities.ProductInfo;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace EyeCareHub.API.Controllers
{
    public class StoreAdminController : BaseApiController
    {

        #region Inject
        private readonly IStoreAdminRepo _store;
        private readonly IMapper _mapper;

        public StoreAdminController(IStoreAdminRepo store, IMapper mapper)
        {
            _store = store;
            _mapper = mapper;
        }
        #endregion


        #region MangeType&Brand&Category

        //Type
        [HttpPost("Add-Type")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddType(string name)
        {
            if (await _store.SearchTypeByName(name))
            {
                return BadRequest(new ApiResponse(400, "The name already exists. "));
            }

            ProductTypes data = new ProductTypes { Name = name };
            
            if (!await _store.AddType(data))
                return BadRequest(new ApiResponse(500));
            return Ok("Added successfully.");
        }
        [HttpDelete("Delete-Type")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteType(int typeId)
        {
            var data = await _store.GetTypeById(typeId);
            if (!await _store.DeleteType(data))
                return BadRequest(new ApiResponse(500));

            return Ok("Deleted successfully.");
        }

        //Category
        [HttpPost("Add-Category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddCategory(string name)
        {
            if (await _store.SearchCategoryByName(name))
            {
                return BadRequest(new ApiResponse(400, "The name already exists. "));
            }

            ProductCategory data = new ProductCategory { Name = name };

            if (!await _store.AddCategory(data))
                return BadRequest(new ApiResponse(500));
            return Ok("Added successfully.");
        }

        [HttpDelete("Delete-Category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCategory(int CategoryId)
        {
            var data = await _store.GetCategoryById(CategoryId);
            if (!await _store.DeleteCategory(data))
                return BadRequest(new ApiResponse(500));
            return Ok("Deleted successfully.");
        }


        //Brand
        [HttpPost("Add-Brand")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddBrand(string name)
        {
            if (await _store.SearchBrandByName(name))
            {
                return BadRequest(new ApiResponse(400, " The name already exists. "));
            }

            ProductBrands data = new ProductBrands { Name = name };

            if (!await _store.AddBrand(data))
                return BadRequest(new ApiResponse(500));
            return Ok("Added successfully.");
        }

        [HttpDelete("Delete-Brand")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBrand(int brandId)
        {
            var data = await _store.GetBrandById(brandId);
            if (!await _store.DeleteBrand(data))
                return BadRequest(new ApiResponse(500));
            return Ok("Deleted successfully.");
        }
        #endregion

        #region MangeProduct

        [HttpPost("Add-Product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddProduct([FromForm]ProductDto item)
        {
            if (!await _store.SearchBrandById(item.ProductBrandId))
                return BadRequest(new ApiResponse(400,"Can Not Found Brand"));

            if (!await _store.SearchTypeById(item.ProductTypeId))
                return BadRequest(new ApiResponse(400, "Can Not Found Type"));

            if (!await _store.SearchCategoryById(item.ProductCategoryId))
                return BadRequest(new ApiResponse(400, "Can Not Found Category"));
            //article.PictureUrl = await FileSetting.UploadFileAsync(articleDto.Picture, "images", "Article", HttpContext);

            var data = _mapper.Map<Products>(item);

            if (!await _store.AddProduct(data))
                return BadRequest(new ApiResponse(500));
            return Ok("Added successfully.");
        }

        [HttpDelete("Delete-Product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProduct(int ProductId)
        {
            var data = await _store.GetProductById(ProductId);

            if (data == null)
                return BadRequest(new ApiResponse(404, "Product Not Found"));

            //var data = _mapper.Map<ProductBrand>(item);
            if (!await _store.DeleteProduct(data))
                return BadRequest(new ApiResponse(500));

            return Ok("Deleted successfully.");
        }

        [HttpPut("Update-Product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateProduct([FromQuery] UpdateProductDto item)
        {
            if (!await _store.SearchBrandById(item.ProductBrandId))
                return BadRequest(new ApiResponse(400, "Can Not Found Brand"));

            if (!await _store.SearchTypeById(item.ProductTypeId))
                return BadRequest(new ApiResponse(400, "Can Not Found Type"));

            if (!await _store.SearchCategoryById(item.ProductCategoryId))
                return BadRequest(new ApiResponse(400, "Can Not Found Category"));

            var data = _mapper.Map<Products>(item);

            if (!await _store.UpdateProduct(data))
                return BadRequest(new ApiResponse(500));
            return Ok("Updated successfully.");
        }


        #endregion

        #region MangeOrder

        [HttpGet("GetAll-Order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Pagination<Order>>> GetAllOrder(OrderSpecParams orderParams)
        {
            var data =await _store.GetAllOrder(orderParams);
            if (data == null) return BadRequest(new ApiResponse(404));
            var order =_mapper.Map<IReadOnlyList<OrderToReturnDto>>(data);

            var count = await _store.GetCountOrder(orderParams);

            return Ok(new Pagination<OrderToReturnDto>(orderParams.PageIndex, orderParams.PageSize, count, order));
        }
        
        
        [HttpPut("Update-OrderStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateOrderStatus(UpdateOrderStatusDto newStatus)
        {
            var order = await _store.GetOrderById(newStatus.OrderId);

            if (order == null)
                return NotFound(new ApiResponse(404,"Order NotFound."));

            if (order.Status == newStatus.OrderStatus)
                return BadRequest(new ApiResponse(400, "Order already has this status."));

            if (order.Status.HasFlag(OrderStatus.Cancelled) || order.Status.HasFlag(OrderStatus.Delivered))
                return BadRequest(new ApiResponse(400,"Cannot change status of a completed or cancelled order" ));


            order.Status = newStatus.OrderStatus;
            var result = await _store.UpdateOrder(order);

            if (!result)
                return BadRequest( new ApiResponse (500 ,"Failed to update order status" ));

            return Ok( "Order status updated successfully");
        }


        #endregion

        #region MangeDeliveryMethods

        [HttpPost("Add-DeliveryMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddDeliveryMethods(DeliveryMethodToAddDto item)
        {
            var data = _mapper.Map<DeliveryMethod>(item);
            var result = await _store.AddDeliveryMethod(data);

            if (!result) return BadRequest(new ApiResponse(500));

            return Ok(data);
        }

        [HttpGet("GetAll-DeliveryMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethods()
        {
            var deliverM = await _store.GetAllDeliveryMethod();
            if (deliverM == null) return BadRequest(new ApiResponse(404));
            var data = _mapper.Map<IReadOnlyList<DeliveryMethod>>(deliverM);

            return Ok(data);
        }

        [HttpDelete("Delete-DeliveryMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteDeliveryMethods(int Id)
        {
            var deliverM = await _store.GetDeliveryMethodById(Id);
            if (deliverM == null) return BadRequest(new ApiResponse(404));

            var result = await _store.DeleteDeliveryMethod(deliverM);
            if (!result) return BadRequest(new ApiResponse(500));
            return Ok("Delete Success");
        }


        #endregion

    }
}
