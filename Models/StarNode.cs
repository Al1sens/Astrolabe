namespace Astrolabe.Models
{
    public class StarNode
    {
        // Номер звезды в БД
        public int Id { get; set; }
        // Название темы
        public string Title { get; set; } = string.Empty;
        // Координаты звезды на экране
        public double PosX { get; set; }
        public double PosY { get; set; }

        // Поле для краткого конспекта или ссылки на материал
        public string? Notes { get; set; }


        // Этап звезды: 0 = Тёмная (не начато), 1 = В процессе (золотая), 2 = Изучено (бирюзовая)
        public int Stage { get; set; } = 0;


        // Сложность темы: 1 = Лёгкая, 2 = Средняя, 3 = Сложная (влияет на размер звезды)
        public int Difficulty { get; set; } = 2;


 
    }
}
