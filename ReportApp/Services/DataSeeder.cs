using ReportApp.Data;
using ReportApp.Models;

namespace ReportApp.Services
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext db)
        {
            // если данные уже есть — ничего не делаем
            if (db.Items.Any())
                return;

            var items = new List<Item>
            {
                // Верхняя ДСЕ
                new Item
                {
                    Name = "Клапан",
                    IsAssembly = true,
                    Batch = "111-111",
                    ParentBatch = null,
                    Quantity = 1,
                    SortOrder = 1
                },

                // Под-ДСЕ: Круг
                new Item
                {
                    Name = "Круг",
                    IsAssembly = true,
                    Batch = "222-222",
                    ParentBatch = "111-111",
                    Quantity = 1,
                    SortOrder = 2
                },

                // Кольца внутри круга
                new Item { Name = "Кольцо1", Batch = "444-001", ParentBatch = "222-222", Quantity = 1, SortOrder = 3 },
                new Item { Name = "Кольцо2", Batch = "444-002", ParentBatch = "222-222", Quantity = 1, SortOrder = 4 },
                new Item { Name = "Кольцо3", Batch = "444-003", ParentBatch = "222-222", Quantity = 1, SortOrder = 5 },
                new Item { Name = "Кольцо4", Batch = "444-004", ParentBatch = "222-222", Quantity = 1, SortOrder = 6 },
                new Item { Name = "Кольцо5", Batch = "444-005", ParentBatch = "222-222", Quantity = 1, SortOrder = 7 },

                new Item { Name = "Гайка", Batch = "555-001", ParentBatch = "222-222", Quantity = 2, SortOrder = 8 },
                new Item { Name = "Шайба", Batch = "555-002", ParentBatch = "222-222", Quantity = 2, SortOrder = 9 },

                // Еще детали клапана
                new Item
                {
                    Name = "Затвор",
                    Batch = "666-001",
                    ParentBatch = "111-111",
                    Quantity = 1,
                    SortOrder = 10
                },

                new Item
                {
                    Name = "Втулка",
                    Batch = "666-002",
                    ParentBatch = "111-111",
                    Quantity = 1,
                    SortOrder = 11
                },

                // Под-ДСЕ: Корпус
                new Item
                {
                    Name = "Корпус",
                    IsAssembly = true,
                    Batch = "333-333",
                    ParentBatch = "111-111",
                    Quantity = 1,
                    SortOrder = 12
                },

                new Item { Name = "Болт", Batch = "777-001", ParentBatch = "333-333", Quantity = 4, SortOrder = 13 },
                new Item { Name = "Гайка", Batch = "777-002", ParentBatch = "333-333", Quantity = 4, SortOrder = 14 },
                new Item { Name = "Шайба", Batch = "777-003", ParentBatch = "333-333", Quantity = 4, SortOrder = 15 }
            };

            db.Items.AddRange(items);
            db.SaveChanges();
        }
    }
}