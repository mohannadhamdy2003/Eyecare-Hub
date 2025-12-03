using AutoMapper;
using EyeCareHub.API.Dtos.ContentEducations;
using EyeCareHub.API.Errors;
using EyeCareHub.API.Helper;
using EyeCareHub.BLL.Interface;
using EyeCareHub.BLL.Repositories;
using EyeCareHub.BLL.specifications;
using EyeCareHub.BLL.specifications.ArticlesSpecifications;
using EyeCareHub.DAL.Entities.Content_Education;
using EyeCareHub.DAL.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace EyeCareHub.API.Controllers
{

    public class ContentEducationController : BaseApiController
    {

        #region Inject
        private readonly IMapper _mapper;
        private readonly IContentEducationRepo _contentEducationRepo;
        private readonly UserManager<AppUser> _userManger;

        public ContentEducationController( IMapper mapper, IContentEducationRepo contentEducationRepo, UserManager<AppUser> userManger)
        {
            
            _mapper = mapper;
            _contentEducationRepo = contentEducationRepo;
            _userManger = userManger;
        }

        #endregion

        #region EducationalCategory

        
        [HttpGet("GetAll-EducationalCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<EducationalCategoryDto>>> GetAllEducationalCategory()
        {
            var Category = await _contentEducationRepo.GeAllEducationalCategory();
            if (Category == null) return BadRequest(new ApiResponse(404));
            var educationalCategoryDto = _mapper.Map<IReadOnlyList<EducationalCategory>, IReadOnlyList<EducationalCategoryDto>>(Category);
            return Ok(educationalCategoryDto);
        }

        [HttpGet("Get-EducationalCategory-ById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EducationalCategoryDto>> GetEducationalCategoryById(int id)
        {
            var category = await _contentEducationRepo.GetByIdEducationalCategory(id);
            if (category == null) return BadRequest(new ApiResponse(404));

            var categoryDto = _mapper.Map<EducationalCategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpPost("Add-EducationalCategory")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddEducationalCategory(EducationalCategoryAddDto categoryDto)
        {

            var categories = await _contentEducationRepo.GeAllEducationalCategory();

            
            var exists = categories.AsEnumerable().Any(c => string.Equals(c.Name, categoryDto.Name, StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                return BadRequest(new ApiResponse(400, "A category with this name already exists"));
            }

            var category = _mapper.Map<EducationalCategory>(categoryDto);
            var result= await _contentEducationRepo.AddEducationalCategory(category);
            
            if (result ==false) return BadRequest(new ApiResponse(500,("Failed to add category")));

            return Ok(new { message= "Category added successfully", EducationalCategoryId = category.Id });
        }

        [HttpPut("Update-EducationalCategory-ById")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateEducationalCategory(EducationalCategoryDto categoryDto)
        {
            var existingCategory = await _contentEducationRepo.GetByIdEducationalCategory(categoryDto.Id);
            if (existingCategory == null)
                return BadRequest(new ApiResponse(404, "Not existingCategory for Id"));

            
            _mapper.Map(categoryDto, existingCategory);

            
            var result =await _contentEducationRepo.UpdateEducationalCategory(_mapper.Map<EducationalCategory>(existingCategory));
            
            if (result == false)
                return BadRequest(new ApiResponse(404, "Failed to update category"));

            // Return a success response
            return Ok("Category updated successfully");
        }

        [HttpDelete("Delete-EducationalCategory-ById/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteEducationalCategory(int id)
        {
            var category = await _contentEducationRepo.GetByIdEducationalCategory(id);
            if (category == null) return NotFound();

            var result= await _contentEducationRepo.DeleteEducationalCategory(category.Id);
            
            if (result ==false ) return BadRequest("Failed to delete category");

            return Ok("Category deleted successfully");
        }

        
        [HttpGet("search-EducationalCategory-ByName")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<EducationalCategoryDto>>> SearchEducationalCategory(string Name)
        {
            var category = await _contentEducationRepo.SearchEducationalCategoryByName(Name);
            if (category == null) return BadRequest(new ApiResponse(404));

            return Ok(_mapper.Map<IReadOnlyList<EducationalCategoryDto>>(category));
        }

        #endregion

        #region ArticleAdmin
        [HttpPost("add-Article")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddArticle(ArticleAddDto articleDto)
        {

            var category = await _contentEducationRepo.GetByIdEducationalCategory(articleDto.CategoryId);
            if (category == null)
                return NotFound(new ApiResponse(404, "Category not found."));

            var article = _mapper.Map<Article>(articleDto);
            article.PublishedDate = DateTime.UtcNow;

            if (articleDto.Picture == null)
            {
                article.PictureUrl = await FileSetting.UploadFileAsync(articleDto.Picture, "images", "Article", HttpContext);
            }

            var result = await _contentEducationRepo.AddArticle(article);
            
            if (result ==false)
                return BadRequest(new ApiResponse(500, "Failed to add the article."));

            return Ok(new { message = "Article added successfully", articleId = article.Id });
        }


        [HttpPut("update-Article")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateArticle( ArticleDto articleDto)
        {
            var existingArticle = await _contentEducationRepo.GetByIdArticle(articleDto.Id);
            if (existingArticle == null)
                return NotFound(new ApiResponse(404, "Article not found."));
            var pathpic = existingArticle.PictureUrl;
            var category = await _contentEducationRepo.GetByIdEducationalCategory(articleDto.CategoryId);
            if (category == null)
                return NotFound(new ApiResponse(404, "Category not found."));

            _mapper.Map(articleDto, existingArticle);

            if (articleDto.Picture == null)
            {
                existingArticle.PictureUrl = await FileSetting.UploadFileAsync(articleDto.Picture, "files", "DoctorInfo", HttpContext);
            }

            var result = await _contentEducationRepo.UpdateArticle(existingArticle);
            
            if (result == false)
                return BadRequest(new ApiResponse(500, "Failed to update the article."));
            FileSetting.DeleteFile(pathpic);

            return Ok(new { message = "Article updated successfully", articleId = existingArticle.Id });
        }

        [HttpDelete("delete-Article/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _contentEducationRepo.GetByIdArticle(id);
            if (article == null)
                return NotFound(new ApiResponse(404, "Article not found."));

            var result = await _contentEducationRepo.DeleteArticle(id);

            if (result ==false)
                return BadRequest(new ApiResponse(500, "Failed to delete the article."));

            FileSetting.DeleteFile(article.PictureUrl);

            return Ok("Article deleted successfully");
        }
        [HttpGet("Get-Articles-Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]

        public async  Task<ActionResult<IReadOnlyList<ArticleResponseAdminDto>>> GetAllArticlesForAdmin()
        {
            var Articles = await _contentEducationRepo.GetAllArticle();
            if (Articles == null)
                return NotFound(new ApiResponse(404,"Can't Found Articles"));
            return Ok(Articles);
        }
        [HttpGet("GetById-Articles-Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ArticleResponseAdminDto>> GetByIdArticlesForAdmin(int Id)
        {
            var Articles = await _contentEducationRepo.GetByIdArticle(Id);
            if (Articles == null)
                return NotFound(new ApiResponse(404, "Can't Found Articles"));
            return Ok(Articles);
        }

        #endregion

        #region ArticleLoved
        [HttpPost("Add-love-Article")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult>  AddLoveArticle(int Id)
        {
            var article =await _contentEducationRepo.GetByIdArticle(Id);
            if (article == null)
                return NotFound(new ApiResponse(404, "Article not found."));


            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return BadRequest(new ApiResponse(400, "User email not found."));

            var IsLoved = await _contentEducationRepo.IsAlreadyLoved(userEmail,Id);

            if (IsLoved)
                return BadRequest(new ApiResponse(400, "Article already loved."));

            ArticleLove articleLove = new ArticleLove { UserEmail = userEmail, ArticleId = Id};

            var result = await _contentEducationRepo.AddLove(articleLove);
            if(!result)
                return BadRequest(new ApiResponse(500, "Failed to love the article."));

            return Ok(new { message = "Article loved successfully" });


        }

        [HttpDelete("unlove-Article")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> deleteLoveArticle(int ArticleId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var article = await _contentEducationRepo.GetByIdArticle(ArticleId);
            if (article == null)
                return NotFound(new ApiResponse(404, "Article not found."));

            var loveArticle = await _contentEducationRepo.GetArticleLoved(userEmail, ArticleId);
            if (loveArticle == null)
                return BadRequest(new ApiResponse(400, "Article Not loved."));

            var result = await _contentEducationRepo.DeleteLove(loveArticle);
            if (!result)
                return BadRequest(new ApiResponse(500, "Failed to Delete the Love."));

            return Ok("Delete loved successfully");
        }
        #endregion

        #region Saved
        [HttpPost("Saved-Article")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SavedArticle(int Id)
        {
            var article = await _contentEducationRepo.GetByIdArticle(Id);
            if (article == null)
                return NotFound(new ApiResponse(404, "Article not found."));


            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return BadRequest(new ApiResponse(400, "User email not found."));

            var IsSaved = await _contentEducationRepo.IsAlreadySaved(userEmail, Id);

            if (IsSaved)
                return BadRequest(new ApiResponse(400, "Article already loved."));

            SavedArticle savedArticle = new SavedArticle { UserEmail = userEmail, ArticleId = Id };

            var result = await _contentEducationRepo.AddSave(savedArticle);
            if (!result)
                return BadRequest(new ApiResponse(500, "Failed to Saved the article."));

            return Ok(new { message = "Article Saved successfully" });


        }

        [HttpDelete("unSave-Article")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> deleteSaveArticle(int ArticleId)
        {
            var article =await _contentEducationRepo.GetByIdArticle(ArticleId);

            if (article == null)
                return NotFound(new ApiResponse(404, "Article not found."));

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var SavedArticle = await _contentEducationRepo.GetArticleSaved(userEmail, ArticleId);
            if (SavedArticle == null)
                return BadRequest(new ApiResponse(400, "Article Not loved."));

            var result = await _contentEducationRepo.DeleteSave(SavedArticle);

            if (!result) return BadRequest(new ApiResponse(500, "Failed to Delete the Love."));

            return Ok("Delete loved successfully");

        }

        [HttpGet("Get-SavedArticle-ForUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pagination<ArticleResponseUserDto>>> GetSavedArticle([FromQuery] SavedArticleParams savedArticleParams)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var SavedArticle = await _contentEducationRepo.GetArticleSavedForUser(savedArticleParams, userEmail);

            if (SavedArticle == null)
                return NotFound(new ApiResponse(404, "Article not found."));

            var articleDto = _mapper.Map<IReadOnlyList<ArticleResponseUserDto>>(SavedArticle);

            foreach (var x in articleDto)
            {
                x.IsLoved = await _contentEducationRepo.IsAlreadyLoved(userEmail, x.Id);
                x.IsSaved = true;
            }

            return Ok(articleDto);
        }

        #endregion

        #region Comments

        [Authorize]
        [HttpPost("Add-Comment-Article")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> AddComentArticle(CommentsArticleDto commentDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return null;

            var user = await _userManger.FindByEmailAsync(email);

            if (user == null)
               return BadRequest(new ApiResponse(401));

            var comment = _mapper.Map<CommentsArticle>(commentDto);

            comment.UserId = user.Id;
            var result =await _contentEducationRepo.AddComment(comment);

            if (!result)
                return BadRequest(new ApiResponse(500, "Error Server To Add comment"));

            return Ok("Add Comment Is Successful");
        }

        [Authorize]
        [HttpPut("Update-Comment-Article")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
       
        public async Task<ActionResult> UpdateComentArticle(UpdateCommentsArticleDto commentDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return null;

            var user = await _userManger.FindByEmailAsync(email);

            if (user == null)
                return BadRequest(new ApiResponse(401));

            var comment =await _contentEducationRepo.GetCommentById(commentDto.Id);

            if (comment == null)
                return NotFound(new ApiResponse(404,"Can Not Found Comment"));

            if (comment.UserId != user.Id)
                return BadRequest(new ApiResponse(400));
            comment.comment = commentDto.comment;

            var result =await _contentEducationRepo.UpdateComment(comment);

            if (!result)
                return BadRequest(new ApiResponse(500, "Error Server To Update comment"));

            return Ok("Update Comment Is Successful");
        }

       
        [HttpDelete("Delete-Comment-Article")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> DeleteComentArticle(int commentId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return null;

            var user = await _userManger.FindByEmailAsync(email);

            if (user == null)
                return BadRequest(new ApiResponse(401));

            var comment = await _contentEducationRepo.GetCommentById(commentId);

            if (comment == null)
                return NotFound(new ApiResponse(404, "Can Not Found Comment"));

            if (comment.UserId != user.Id)
                return BadRequest(new ApiResponse(400));

            var result = await _contentEducationRepo.DeleteComment(comment);

            if (!result)
                return BadRequest(new ApiResponse(500, "Error Server To Delete comment"));

            return Ok("Delete Comment Is Successful");
        }


        [HttpGet("GetAll-Comment-Article")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<Pagination<CommentToReturnDto>>> GetComentArticle(CommentArticleParams commentParam)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return null;

            var user = await _userManger.FindByEmailAsync(email);

            if (user == null)
                return BadRequest(new ApiResponse(401));

            var comment = await _contentEducationRepo.GetCommentByIdArticleWithSpec(commentParam);

            if (comment == null)
                return NotFound(new ApiResponse(404, "Can Not Found Comment"));

            var commentDto = _mapper.Map<IReadOnlyList<CommentToReturnDto>>(comment);


            var count = await _contentEducationRepo.GetCountCommentWithSpec(commentParam);

            var data = new Pagination<CommentToReturnDto>(commentParam.PageIndex,commentParam.PageSize,count, commentDto);

            return Ok(data);
        }

        // add from Query To Add Action
        //get for article By ArticleId
        #endregion

        #region Article
        [HttpGet("Get-Article-ById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ArticleResponseUserDto>> GetArticleById(int Id)
        {
            var article = await _contentEducationRepo.GetArticleByIdWithSpec(Id);

            if (article == null)
                return NotFound(new ApiResponse(404, "Article not found."));

            var articleDto = _mapper.Map<ArticleResponseUserDto>(article);

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail != null)
            {


                articleDto.IsLoved = await _contentEducationRepo.IsAlreadyLoved(userEmail, articleDto.Id);
                articleDto.IsSaved = await _contentEducationRepo.IsAlreadySaved(userEmail, articleDto.Id);
                
            }
            else
            {
                articleDto.IsLoved = false;
                articleDto.IsSaved = false;
            }

            return Ok(articleDto);
        }

        [HttpGet("GetAll-Article")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pagination<ArticleResponseUserDto>>> GetAllArticle([FromQuery]ArticleSpecParams articleParams)
        {
            var  articles = await _contentEducationRepo.GetAllArticledWithSpec(articleParams);
            

            if (articles == null)
                return NotFound(new ApiResponse(404, "Article not found."));

            var articleDto = _mapper.Map<IReadOnlyList<ArticleResponseUserDto>>(articles);

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail != null)
            {
                foreach (var Art in articleDto)
                {
                    Art.IsLoved = await _contentEducationRepo.IsAlreadyLoved(userEmail, Art.Id);
                    Art.IsSaved = await _contentEducationRepo.IsAlreadySaved(userEmail, Art.Id);
                }
            }
            else
            {
                foreach(var art in articleDto)
                {
                    art.IsLoved = false;
                    art.IsSaved = false;
                }
            }
            var count = await _contentEducationRepo.GeyCountArticleWithSpec(articleParams);
            return Ok(new Pagination<ArticleResponseUserDto>(articleParams.PageIndex, articleParams.PageSize, count, articleDto));
        }

        
        #endregion

    }
}
