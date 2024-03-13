using LibraryBooksManagement.Data.Configuration;
using LibraryBooksManagement.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace LibraryBooksManagement.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext appDbContext;

        public BookService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<(bool Success, string Message)> DeleteBookAsync(int id)
        {
            if (id <= 0) return ErrorMessage();

            var bookToDelete = await appDbContext.Books.FirstOrDefaultAsync(_ => _.Id == id);
            if (bookToDelete is null) return ErrorMessage();

            appDbContext.Books.Remove(bookToDelete!); await appDbContext.SaveChangesAsync();
            return SuccessMessage();
        }

        public async Task<List<Book>> GetBookAsync() => await appDbContext.Books.ToListAsync();

        public async Task<(bool Success, string Message)> ManageBookAsync(Book book)
        {
            if (book is null) return ErrorMessage();

            if (book.Id == 0)
            {
                if (!await IsBookAlreadyAdded(book.Title!))
                {
                    appDbContext.Books.Add(book); await appDbContext.SaveChangesAsync();
                    return SuccessMessage();
                }
            }
            var bookToUpdate = await appDbContext.Books.FirstOrDefaultAsync(_ => _.Id == book.Id);
            if (bookToUpdate is null) return ErrorMessage();

            bookToUpdate.Title = book.Title;
            bookToUpdate.Description = book.Description;
            bookToUpdate.Image = book.Image;
            await appDbContext.SaveChangesAsync();
            return SuccessMessage();
        }

        private static (bool, string) SuccessMessage() => (true, "Process successfully completed.");
        private static (bool, string) ErrorMessage() => (false, "Error occured whiles processing.");

        private async Task<bool> IsBookAlreadyAdded(string bookName)
        {
            var book = await appDbContext.Books.Where(_ => _.Title!.ToLower().Equals(bookName)).FirstOrDefaultAsync();
            return book is not null;
        }
    }
}
