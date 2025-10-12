using BookShop.Data;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repository
{
    public class HomeRepository : IHomeRepository
    {
        public readonly ApplicationDbContext _db;
        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Genre>> Genres()
        {
            
            return await _db.Genres.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int categoryId = 0)
        {
            sTerm = sTerm.ToLower();
            var query = from book in _db.Books
                        join genre in _db.Genres
                        on book.GenreId equals genre.Id
                        where string.IsNullOrEmpty(sTerm) || book.BookName.ToLower().StartsWith(sTerm) || book.AuthorName.ToLower().StartsWith(sTerm)
                        where categoryId == 0 || book.GenreId == categoryId
                        select new Book
                        {
                            Id = book.Id,
                            Image = book.Image,
                            AuthorName = book.AuthorName,
                            BookName = book.BookName,
                            GenreId = book.GenreId,
                            Price = book.Price,
                            GenreName = genre.GenreName,
                            Quantity = book.Stock == null ? 0 : book.Stock.Quantity
                        };

            // Apply search term filter if provided
            if (!string.IsNullOrWhiteSpace(sTerm))
            {
                query = query.Where(book => book.BookName.Contains(sTerm) || book.AuthorName.Contains(sTerm));
            }

            // Apply category filter if provided
            if (categoryId > 0)
            {
                query = query.Where(book => book.GenreId == categoryId);
            }

            // Execute the query and return the result
            return await query.ToListAsync();
        }
    }
}
