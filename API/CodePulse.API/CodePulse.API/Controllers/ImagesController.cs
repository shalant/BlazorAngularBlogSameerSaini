using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    public ImagesController(IImageRepository imageRepository)
    {
        ImageRepository = imageRepository;
    }

    public IImageRepository ImageRepository { get; }

    // GET: {apibaseUrl}/api/Images
    [HttpGet]
    public async Task<IActionResult> GetAllImages()
    {
        // call image repository to get all images
        var images = await ImageRepository.GetAll();

        // convert domain model to DTO
        var response = new List<BlogImageDto>();
        foreach (var image in images)
        {
            response.Add(new BlogImageDto
            {
                Id = image.Id,
                Title = image.Title,
                DateCreated = image.DateCreated,
                FileExtension = image.FileExtension,
                FileName = image.FileName,
                Url = image.Url
            });
        }

        return Ok(response);
    }



    // POST: {apibaseurl}/api/images
    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
        [FromForm] string filename, [FromForm] string title)
    {
        ValidateFileUpload(file);

        if(ModelState.IsValid)
        {
            // File upload
            var blogImage = new BlogImage
            {
                FileExtension = Path.GetExtension(file.FileName).ToLower(),
                FileName = filename,
                Title = title,
                DateCreated = DateTime.Now
            };

            blogImage = await ImageRepository.Upload(file, blogImage);

            // convert domain model to DTO
            var response = new BlogImageDto
            {
                Id = blogImage.Id,
                Title = blogImage.Title,
                DateCreated = blogImage.DateCreated,
                FileExtension = blogImage.FileExtension,
                FileName = blogImage.FileName,
                Url = blogImage.Url
            };

            return Ok(response);
        }

        return BadRequest(ModelState);
    }

    private void ValidateFileUpload(IFormFile file)
    {
        var allowedExtensions = new string[]
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        if(!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) )
        {
            ModelState.AddModelError("file", "Unsupported file format");
        }

        if(file.Length > 10485760 )
        {
            ModelState.AddModelError("file", "File size cannot exceed 10MB");
        }
    }
}
