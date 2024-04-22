using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface;

public interface ICategoryRepository
{
    // Should only work with Domain Models
    Task<Category> CreateAsync(Category category);
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetById(Guid id);
    Task<Category?> UpdateAsync(Category category);
    Task<Category?> DeleteAsync(Guid id);
}
