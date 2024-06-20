using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation;

public class ImageRepository : IImageRepository
{
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly AppDbContext dbContext;

    public ImageRepository(IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor,
        AppDbContext dbContext)
    {
        this.webHostEnvironment = webHostEnvironment;
        this.httpContextAccessor = httpContextAccessor;
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<BlogImage>> GetAll()
    {
        return await dbContext.BlogImages.ToListAsync();
    }

    public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
    {
        // 1) upload the image to the API/images
        var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

        using var stream = new FileStream(localPath, FileMode.Create);
        await file.CopyToAsync(stream);

        // 2) update DB
        //https://codepulse.com/images/somefilename.jpg
        var httpRequest = httpContextAccessor.HttpContext.Request;
        var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

        blogImage.Url = urlPath;

        await dbContext.BlogImages.AddAsync(blogImage);
        await dbContext.SaveChangesAsync();

        return blogImage;
    }
}
