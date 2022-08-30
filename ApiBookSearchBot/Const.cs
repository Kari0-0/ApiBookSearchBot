using ApiBookSearchBot.Models;
using ApiBookSearchBot.Reposytory;
using Newtonsoft.Json;
using System;

namespace ApiBookSearchBot
{
    public class Const
    {
        //Path
        public const string BOOK_NAME_SEARCH = "book_name_search";
        public const string BOOK_ISBN_SEARCH = "book_isbn_search";
        public const string BOOK_AUTHOR_SEARCH = "book_author_search";
        public const string BOOK_BOOKCOVER_SEARCH = "book_bookcover_search";
        public const string BOOK_RANDOMBOOK = "book_randombook";
        public const string ADD_USER = "addUser";
        public const string ADD_BOOK = "addBook";
        public const string DELETE_BOOK = "deleteBook";
        public const string DELETE_ALL_BOOKS = "deleteAllBooks";
        public const string GET_ALL_BOOKS = "getAllBooks";

        //Key
        public const string BOOK_NAME = "bookName";
        public const string ISBN = "isbn";
        public const string AUTHOR = "author";
        public const string TG_ID = "tgid";
        public const string NAME = "name";

    }
}
