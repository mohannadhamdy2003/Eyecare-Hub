using EyeCareHub.API.Dtos.Appointment;
using EyeCareHub.API.Dtos.AuthDtos;
using EyeCareHub.API.Dtos.Product;
using EyeCareHub.API.Errors;
using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Data;
using EyeCareHub.DAL.Entities.Basket;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Entities.ProductInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EyeCareHub.API.Controllers
{
    public class BasketController : BaseApiController
    {
        #region Inject
        private readonly IBasketRepository basketRepo;
        private readonly UserManager<AppUser> userManger;
        private readonly string BasketKeyPrefix;

        public BasketController(IBasketRepository BasketRepo, UserManager<AppUser> userManger,
            IConfiguration configuration, IGenericRepository<StoreContext,Products> pr)
        {
            basketRepo = BasketRepo;
            this.userManger = userManger;
            BasketKeyPrefix = configuration.GetSection("BasketSettings:BasketKeyPrefix").Value;
        }

        #endregion



        #region GetBasket
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasket>> GetBasket()
        {
            var UserId =await GetCurrentUser();
            if (UserId == null)
                return BadRequest(new ApiResponse(401));
            var basket = await basketRepo.GetCustomerBasket(UserId);
            return Ok(basket ?? new CustomerBasket(UserId));
        }

        #endregion

        #region AddBasket 
        [HttpPost("Add-BasketItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasket>> AddBasket([FromQuery] BasketItem basket)
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return BadRequest(new ApiResponse(401, "User email not found."));



            var user = await userManger.FindByEmailAsync(userEmail);

            if (user == null) return BadRequest(new ApiResponse(401, "Can Not Found Acount"));

            var data = await basketRepo.AddItemsToBasketAsync(user.Id,basket);

            if (data == null) return BadRequest(new ApiResponse(500));

            return Ok(data);

        }

        #endregion

        #region UpdateBasket
        [HttpPut("Update-Quantity-Basket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket([FromQuery]UpdateBasketDto update)
        {
            var UserId = await GetCurrentUser();
            if (UserId == null)
                return BadRequest(new ApiResponse(401));

            var basket = await basketRepo.UpdateItemQuantityAsync(UserId,update.ProductId,update.Quantity);

            if (basket == null) return NotFound(new ApiResponse(404));


            return Ok(basket);  
        }

        #endregion

        #region DeleteBasket
        [HttpDelete("Delete-Basket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteBasket()
        {
            var UserId = await GetCurrentUser();
            return await basketRepo.DeleteCustomerBasket(UserId);
        }


        [HttpDelete("Delete-ItemBasket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasket>> DeleteItemBasket(int ProductId)
        {
            var UserId = await GetCurrentUser();
             return await basketRepo.RemoveItemFromBasketAsync(UserId, ProductId);
           
        }

        #endregion


        private async  Task<string> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return null;

            var user =await userManger.FindByEmailAsync(email);

            if (user == null )
                return null;
            

            return user.Id;
        }

    }
}

