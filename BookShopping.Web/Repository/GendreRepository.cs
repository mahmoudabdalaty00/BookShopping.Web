
using Microsoft.EntityFrameworkCore;

namespace BookShopping.Web.Repository
{
    public class GendreRepository : IGendreRepository
    {
     private readonly ApplicationDbContext _context;

        public GendreRepository(ApplicationDbContext context)
        {
            _context = context;
        }


         /// to editing &adding & deleting gendres
       
        #region
        public async Task AddGendre(Gendre gendre)
        {
           _context.Gendres.Add(gendre);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGendre(Gendre gendre)
        {
            _context.Gendres.Update(gendre);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteGendre(Gendre gendre)
        {
            _context.Gendres.Remove(gendre);
            await _context.SaveChangesAsync();
        }

        #endregion
       
        
         
        /// to get data
           
        #region
        public async Task<IEnumerable<Gendre>> GetAllGendres()
        {
            return await _context.Gendres.ToListAsync();
        }
 

        public async Task<Gendre> GetGendreById(int gendreId)
        {
            return await _context.Gendres.FindAsync(gendreId);
        }
        #endregion

    }
}
