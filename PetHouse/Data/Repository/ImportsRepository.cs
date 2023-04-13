using Microsoft.EntityFrameworkCore;
using PetHouse.Models;


namespace PetHouse.Data.Repository
{
    public class ImportRepository :IImportsRepository
    {
        private readonly PetHouseDbContext _context;

        public ImportRepository(PetHouseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Import>> GetAll()
        {
            var imports = await _context.Imports
                .Include(i => i.Supplier)
                .Include(i => i.User)
                .Include(i => i.ImportDetails)
                    .ThenInclude(id => id.Product)
                .Select(i => i.ToViewModel())
                .ToListAsync();

            return (IEnumerable<Import>)imports;
        }

        public async Task<Import> GetById(int id)
        {
            var import = await _context.Imports
                .Include(i => i.Supplier)
                .Include(i => i.User)
                .Include(i => i.ImportDetails)
                    .ThenInclude(id => id.Product)
                .FirstOrDefaultAsync(i => i.Id == id);
            return import;
        }

        public async Task Add(Import Imports)
        {
            var import = new Import
            {
                SupplierId = Imports.SupplierId,
                UserId = Imports.UserId,
                ImportDate = Imports.ImportDate,
                PaymentStatus = Imports.PaymentStatus,
                Status = Imports.Status
            };

            _context.Imports.Add(import);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Import Imports)
        {
            var import = await _context.Imports.FindAsync(id);

            if (import == null)
            {
                throw new InvalidOperationException("Import not found.");
            }

            import.SupplierId = Imports.SupplierId;
            import.UserId = Imports.UserId;
            import.ImportDate = Imports.ImportDate;
            import.PaymentStatus = Imports.PaymentStatus;
            import.Status = Imports.Status;
            _context.Imports.Update(import);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var import = await _context.Imports.FindAsync(id);

            if (import == null)
            {
                throw new InvalidOperationException("Import not found.");
            }

            _context.Imports.Remove(import);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliers()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
