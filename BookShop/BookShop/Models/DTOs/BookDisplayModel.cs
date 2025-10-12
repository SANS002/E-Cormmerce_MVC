using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models.DTOs
{
    public class BookDisplayModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public string STerm { get; set; } = "";
        public int GenreId { get; set; } = 0;

        [NotMapped]
        public string GenreName { get; set; }

    }  
}
