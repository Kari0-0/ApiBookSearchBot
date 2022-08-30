namespace ApiBookSearchBot.Models
{
    //клас для повертання статусу виконання запиту
    public class StatusRequest
    {
        public StatusRequest(string status)
        {
            Status = status;
        }
        public string Status { get; set; }

        public static string OK { get; } = "OK"; 
        public static string FAIL { get; } = "FAIL";
    }
}
