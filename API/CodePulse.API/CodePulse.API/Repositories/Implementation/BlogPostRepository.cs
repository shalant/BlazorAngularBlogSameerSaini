using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;

namespace CodePulse.API.Repositories.Implementation;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly AppDbContext dbContext;
    public BlogPostRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<BlogPost> CreateAsync(BlogPost blogPost)
    {
        await dbContext.BlogPosts.AddAsync(blogPost);
        dbContext.SaveChangesAsync();
        return blogPost;
    }
}
