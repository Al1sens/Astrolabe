namespace Astrolabe.Models
{
    public class StarConnection
    {
        public int Id { get; set; }

        // Внешние ключи — ID первой и второй звезды
        public int FromStarId { get; set; }
        public int ToStarId { get; set; }

        // Навигационные свойства для EF Core
        public StarNode? FromStar { get; set; }
        public StarNode? ToStar { get; set; }

    }
}
