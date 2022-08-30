using ApiBookSearchBot.Models;
using System.Linq;
using System.Text;

namespace ApiBookSearchBot.Reposytory
{
    public class Reposytory
    {
        public void AddUser(TelegramUser telegramUser)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Users.Where(x => x.TgId == telegramUser.TgId).Count() != 0)
                {
                    return;
                }
                
                db.Users.Add(telegramUser);     
                db.SaveChanges();               
            }
        }
        public void DeleteUser(TelegramUser telegramUser)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Users.Where(x => x.TgId == telegramUser.TgId).Count() == 0)
                {
                    return;
                }

                var user = db.Users.FirstOrDefault(x => x.TgId == telegramUser.TgId);

                db.Users.Remove(user);
                db.SaveChanges();   
            }
        }

        public void AddBook(UserBook userBook, string tgId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(x => x.TgId == tgId);
                
                if (user != default(TelegramUser))
                {
                    userBook.TelegramUser = user;

                    db.Books.Add(userBook);
                }

                db.SaveChanges();
            }

        }

        public void DeleteBook(UserBook userBoot, string tgId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(x => x.TgId == tgId);

                if(user == default(TelegramUser)) return;

                if (db.Books.Where(x => x.Isbn == userBoot.Isbn &&
                                        user.Id == x.TelegramUserId)
                            .Count() == 0)
                {
                    return;
                }
                var book = db.Books.FirstOrDefault(x => x.Isbn == userBoot.Isbn &&
                                                   x.TelegramUserId == user.Id);
                if (book != default(UserBook))
                {
                    db.Books.Remove(book);

                    db.SaveChanges();
                }
            }
        }

        public void DeleteAllBook(string tgid)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(x => x.TgId == tgid);
                if (user == default(TelegramUser))
                {
                    return;
                }

                var books = db.Books.Where(x => x.TelegramUserId == user.Id).ToList();

                db.Books.RemoveRange(books);
                db.SaveChanges();
            }
        }
        public string GetUsers()
        {
            StringBuilder bdr = new StringBuilder();

            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var user in db.Users)
                {
                    bdr.AppendLine($"Name: {user.Name} TgId: {user.TgId}");

                    foreach (var book in user.Books.Where(x=> x.TelegramUserId == user.Id))
                    {
                        bdr.AppendLine($"\t{book.Isbn}");
                    }
                };
            }
            return bdr.ToString();
        }

        public string GetBooks()
        {
            StringBuilder stringBuilder = new StringBuilder();
            using(ApplicationContext db = new ApplicationContext())
            {
                foreach (var book in db.Books)
                {
                    var user = db.Users.First(x=> x.Id == book.TelegramUserId);
                    stringBuilder.AppendLine($"Id: {book.Id} Isbn: {book.Isbn} TgId:{book.TelegramUserId} User: {user?.Name}");
                }
            }
            return stringBuilder.ToString();
        }

        public string GetBooks(string tgid)
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (ApplicationContext db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(x => x.TgId == tgid);

                if (user == default(TelegramUser)) return "You don`t have any books";

                var books = db.Books.Where(x => x.TelegramUserId == user.Id).ToList();
                
                foreach (var book in books)
                {
                    stringBuilder.AppendLine($"isbn: {book.Isbn}");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
