using apiNew.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   apiNew.Repositories.Interface
{
    public interface IDriverRepository
    {
        Task<IEnumerable<Drivers>> GetAllAsync();
        Task<Drivers> GetByIdAsync(int id);
        Task AddAsync(Drivers driver);
        Task UpdateAsync(Drivers driver);
        Task DeleteAsync(int id);
    }
}