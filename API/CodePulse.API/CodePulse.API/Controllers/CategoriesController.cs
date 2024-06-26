using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository categoryRepository;

    //use repository, not the 
    public CategoriesController(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryRequestDto request)
    {
        // map DTO to domain model
        var category = new Category
        {
            Name = request.Name,
            UrlHandle = request.UrlHandle,
        };

        await categoryRepository.CreateAsync(category);

        // Domain model to DTO
        var response = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            UrlHandle = category.UrlHandle,
        };

        return Ok(response);
    }

    // GET: /api/categories
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await categoryRepository.GetAllCategoriesAsync();
        
        // map domain model to dto
        var response = new List<CategoryDto>();
        foreach (var category in categories)
        {
            response.Add(new CategoryDto
            {
                Id= category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            });
        }

        return Ok(response);
    }

    // GET: https://localhost:7226/api/categories/{id}
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
    {
        var existingCategory = await categoryRepository.GetById(id);
        if(existingCategory == null)
        {
            return NotFound();
        }

        var response = new CategoryDto 
        { 
            Id = existingCategory.Id,
            Name = existingCategory.Name,
            UrlHandle = existingCategory.UrlHandle,
        };

        return Ok(response);
    }


    // PUT: https://localhost:7226/api/categories/{id}
    [HttpPut]
    [Route("{id:guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
    {
        // Convert Dto to domain model
        var category = new Category
        {
            Id = id,
            Name = request.Name,
            UrlHandle = request.UrlHandle
        };

        category = await categoryRepository.UpdateAsync(category);

        if(category == null)
        {
            return NotFound();
        }

        // convert domain model to dto
        var response = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            UrlHandle = category.UrlHandle,
        };

        return Ok(response);
    }

    // DELETE https://localhost:7226/api/categories/{id}
    [HttpDelete]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    {
        var category = await categoryRepository.DeleteAsync(id);

        if(category == null)
        {
            return NotFound();
        }

        //convert Domain model to DTO
        var response = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            UrlHandle = category.UrlHandle,
        };
        
        return Ok(response);
    }
}
