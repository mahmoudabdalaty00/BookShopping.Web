namespace BookShopping.Web.Repository
{
    public interface IGendreRepository
    {
        Task AddGendre (Gendre gendre);
        Task UpdateGendre (Gendre gendre);
        Task DeleteGendre (Gendre gendre  );  
        
        Task<Gendre> GetGendreById (int gendreId);

     Task<IEnumerable<Gendre>> GetAllGendres ();

    }
}
