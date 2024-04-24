using CodePulse.API.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

public class ImagesController : Controller
{

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
                FileExtension = Path.GetExtension(filename).ToLower(),
                FileName = filename,
                Title = title,
                DateCreated = DateTime.Now
            };
        }
    }

    private void ValidateFileUpload(IFormFile file)
    {
        var allowedExtensions = new string[]
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        if(allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) )
        {
            ModelState.AddModelError("file", "Unsupported file format");
        }

        if(file.Length > 10485760 )
        {
            ModelState.AddModelError("file", "File size cannot exceed 10MB");
        }
    }
}
