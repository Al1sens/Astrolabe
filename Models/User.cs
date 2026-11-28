namespace Astrolabe.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        //Навигационное свойство — все звёзды этого пользователя
        public List<StarNode> Stars { get; set; } = new();
    }
}
