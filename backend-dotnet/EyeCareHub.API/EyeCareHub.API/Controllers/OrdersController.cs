using AutoMapper;
using EyeCareHub.API.Dtos.OrderDtos;
using EyeCareHub.API.Errors;
using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Entities.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EyeCareHub.API.Controllers
{
    public class OrdersController : BaseApiController
    {
        #region Inject
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        #endregion

        #region CreateOrder
        [HttpPost("Create-Order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            if (!await orderService.AnyGetDeliveryMethodsForIdAsync(orderDto.DeliveryMethodId))
                return BadRequest(new ApiResponse(404, "DeliveryMethods is not found"));

            var emailBuyer = User.FindFirstValue(ClaimTypes.Email);

            if (emailBuyer == null) return BadRequest(new ApiResponse(401));

            var exitorder = await orderService.GetActiveOrderAsync(emailBuyer);

            if (exitorder != null) return BadRequest(new ApiResponse(401, "U Have Order"));


            var address = mapper.Map<AddresDto, AddressOrder>(orderDto.ShipToAddress);

            var order = await orderService.CreateOrderAsync(emailBuyer, orderDto.BasketId, orderDto.DeliveryMethodId, address);
            if (order == null)
                return BadRequest(new ApiResponse(400, "Error Occured Creating Order"));
            var orderDt = mapper.Map<OrderToReturnDto>(order);
            return Ok(orderDt);
        }
        #endregion


        [HttpPost("Get-Order-ForUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetForUser()
        {
            var emailBuyer = User.FindFirstValue(ClaimTypes.Email);
            if (emailBuyer == null) return BadRequest(new ApiResponse(401));

            var order = await orderService.GetActiveOrderAsync(emailBuyer);

            if (order == null) return BadRequest(new ApiResponse(404, "You Not Have Order"));

            var data = mapper.Map<OrderToReturnDto>(order);

            return Ok(data);

        }

        
        
        
        
        
        [HttpPost("GetAll-Order-ForUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllForUser()
        {
            var emailBuyer = User.FindFirstValue(ClaimTypes.Email);
            if (emailBuyer == null) return BadRequest(new ApiResponse(401));

            var order = await orderService.GetOrderForUserAsync(emailBuyer);

            if (order == null) return BadRequest(new ApiResponse(404, "You Not Have Order"));

            var data = mapper.Map<IReadOnlyList<OrderToReturnDto>>(order);

            return Ok(data);

        }






        [HttpPost("Get-DeliveryMethod-ForUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var emailBuyer = User.FindFirstValue(ClaimTypes.Email);
            if (emailBuyer == null) return BadRequest(new ApiResponse(401));

            var order = await orderService.GetDeliveryMethodsAsync();

            if (order == null) return BadRequest(new ApiResponse(404, "You Not Have Order"));

            var data = mapper.Map<IReadOnlyList<DeliveryMethod>>(order);

            return Ok(data);

        }



    }
}
