namespace ApiBookSearchBot.Models
{
    //моделі для бази даних
    public class TelegramUser
    {
        public int Id { get; set; }
        public string TgId { get; set; }
        public string Name { get; set; }
        public List<UserBook> Books { get; set; } = new();
    }

    public class UserBook
    {
        public int Id { get; set; }
        public string Isbn { get; set; }

        public int TelegramUserId { get; set; }
        public TelegramUser? TelegramUser { get; set; }
    }

}
