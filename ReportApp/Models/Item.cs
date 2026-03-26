namespace ReportApp.Models
{
    public class Item
    {
        public int Id { get; set; }

        // Наименование элемента
        public string Name { get; set; } = string.Empty;

        // true = ДСЕ, false = обычная деталь
        public bool IsAssembly { get; set; }

        // Партия самого элемента
        public string Batch { get; set; } = string.Empty;

        // партия родительской ДСЕ
        public string? ParentBatch { get; set; }

        // Количество
        public int Quantity { get; set; }

        // Порядок вывода в отчете
        public int SortOrder { get; set; }

        // Комментарий
        public string? Comment { get; set; }
    }
}
