namespace BookShopping.Web
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Book>> GetBook(string sterm = "", int gendreId = 0);

            Task<IEnumerable<Gendre>> GetGenre();

    }
}