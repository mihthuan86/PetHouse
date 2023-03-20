using PetHouse.Models;

namespace PetHouse.Data.Repository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task Add(Category category);
        Task Update(int Id,Category category);
        Task Delete(int Id);
    }
}
