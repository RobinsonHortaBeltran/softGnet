using apiNew.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace apiNew.Repositories.Interface
{
    public class DriverRepository : IDriverRepository
    {
        private readonly ApplicationDbContext _context;

        public DriverRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Drivers driver)
        {
            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
           var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Drivers>> GetAllAsync()
        {
             return await _context.Drivers.ToListAsync();
        }

        public async Task<Drivers> GetByIdAsync(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            return driver ?? throw new Exception("Driver not found.");
        }

        public async Task UpdateAsync(Drivers driver)
        {
            _context.Entry(driver).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}