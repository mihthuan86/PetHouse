using PetHouse.Models;

namespace PetHouse.Data.Repository
{
    public interface IImportsRepository
    {
        Task<IEnumerable<Import>> GetAll();
        Task<Import> GetById(int id);
        Task Add(Import Import);
        Task Update(int id, Import Import);
        Task Delete(int id);
    }
}