using System.Collections.Generic;

namespace ApiBookSearchBot.Models
{
    public class BookModel
    {
        public int totalItems { get => books.Count; }
        public List<Book> books { get; set; } = new List<Book>();

        public static BookModel FullGoogleModelToBookModel(FullGoogleModels gModel)
        {
            BookModel bookModel = new BookModel();
            foreach (var item in gModel.items)
            {
                Book book = new Book();
                book.id = item.id;
                book.selfLink = item.selfLink;
                book.title = item.volumeInfo.title;
                book.description = item.volumeInfo.description;
                book.publisher = item.volumeInfo.publisher;
                book.publishedDate = item.volumeInfo.publishedDate;
                book.pageCount = item.volumeInfo.pageCount;
                book.language = item.volumeInfo.language;

                if (item.volumeInfo.categories != null)
                {
                    book.categories = new List<string>(item.volumeInfo.categories);
                }
                bookModel.books.Add(book);
            }

            return bookModel;
        }
    }

    public class Book
    {
        public string id { get; set; }= string.Empty;
        public string selfLink { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string publisher { get; set; } = string.Empty;
        public string publishedDate { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int? pageCount { get; set; }
        public List<string> categories { get; set; } = new List<string>();
        public string language { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
    }
}
