using Microsoft.EntityFrameworkCore;
using PetHouse.Models;

namespace PetHouse.Data.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly PetHouseDbContext _context;

        public CategoryRepository(PetHouseDbContext context) 
        {
            _context = context;
        }

        public async Task Add(Category category)
        {
             await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var cate = await _context.Categories.FirstOrDefaultAsync(ct=>ct.Id == Id);
            cate.Status = -1;
            _context.Update(cate);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public Task<Category> GetById(int id)
        {
            return _context.Categories.FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public Task Update(int Id, Category category)
        {
            throw new NotImplementedException();
        }
    }
}
