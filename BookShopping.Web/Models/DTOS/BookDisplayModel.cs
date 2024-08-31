namespace BookShopping.Web.Models.DTOS
{
    public class BookDisplayModel
    {
        public IEnumerable<Book> Books { get; set; } 

        public IEnumerable<Gendre> gendres { get; set; }
        public string Sterm { get; set; } = "";

        public int GendreID { get; set; } = 0;
    }
}
