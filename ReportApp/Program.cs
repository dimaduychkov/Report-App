using Microsoft.EntityFrameworkCore;
using ReportApp.Data;
using ReportApp.Services;

namespace ReportApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new AppDbContext();

            db.Database.Migrate();
            DataSeeder.Seed(db);

            var reportService = new AssemblyReportService(db);
            var excelService = new ExcelExportService();

            while (true)
            {
                ShowMenu();
                Console.Write("Выбери пункт меню: ");
                string? input = Console.ReadLine();

                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        ShowAllAssemblies(db);
                        break;

                    case "2":
                        ShowAllItems(db);
                        break;

                    case "3":
                        ShowExplosionById(db, reportService);
                        break;

                    case "4":
                        ExportExplosionById(db, reportService, excelService);
                        break;

                    case "0":
                        Console.WriteLine("Выход из программы.");
                        return;

                    default:
                        Console.WriteLine("Неверный пункт меню.");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Нажми любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\t\tМЕНЮ");
            Console.WriteLine("1. Показать все ДСЕ");
            Console.WriteLine("2. Показать все элементы");
            Console.WriteLine("3. Построить разузловку по Id");
            Console.WriteLine("4. Выгрузить разузловку в Excel по Id");
            Console.WriteLine("0. Выход");
            Console.WriteLine();
        }

        static void ShowAllAssemblies(AppDbContext db)
        {
            var assemblies = db.Items
                .Where(x => x.IsAssembly)
                .OrderBy(x => x.SortOrder)
                .ToList();

            if (assemblies.Count == 0)
            {
                Console.WriteLine("ДСЕ не найдены.");
            }

            foreach (var assembly in assemblies)
            {
                Console.WriteLine(
                  $"Id: {assembly.Id} | Наименование: {assembly.Name} | Партия: {assembly.Batch} | На партию: {assembly.ParentBatch ?? "-"}");
            }
        }

        static void ShowAllItems(AppDbContext db)
        {
            var items = db.Items
                .OrderBy(x => x.Id)
                .ToList();

            if(items.Count == 0)
            {
                Console.WriteLine("Элементов нет.");
                return;
            }

            foreach (var item in items)
            {
                string type = item.IsAssembly ? "ДСЕ" : "Деталь";

                Console.WriteLine(
                    $"Id: {item.Id} | {item.Name} | {type} | Партия: {item.Batch} | На партию: {item.ParentBatch ?? "-"} | Кол-во: {item.Quantity} | Комментарий: {item.Comment ?? "-"}");
            }
        }

        static void ShowExplosionById(AppDbContext db, AssemblyReportService reportService)
        {
            Console.Write("Введите Id элемента: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Id должен быть числом.");
                return;
            }

            var item = db.Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                Console.WriteLine("Элемент с таким Id не найден.");
                return;
            }

            if (!item.IsAssembly)
            {
                Console.WriteLine("Нельзя сделать разузловку для не ДСЕ.");
                return;
            }

            var reportRows = reportService.BuildReport(item.Batch);

            if (reportRows.Count == 0)
            {
                Console.WriteLine("Не удалось построить разузловку.");
                return;
            }

            Console.WriteLine($"Разузловка для элемента: {item.Name} (Id={item.Id}, Batch={item.Batch})");
            Console.WriteLine();

            foreach (var row in reportRows)
            {
                string indent = new string(' ', row.Level * 2);
                string type = row.IsAssembly ? "ДСЕ" : "Деталь";

                Console.WriteLine(
                    $"{row.Position}. {indent}{row.Name} | {type} | Партия: {row.Batch} | На партию: {row.ParentBatch ?? "-"} | Кол-во: {row.Quantity}");
            }
        }

        static void ExportExplosionById(AppDbContext db,AssemblyReportService reportService,
            ExcelExportService excelService)
        {
            Console.Write("Введите Id элемента для выгрузки в Excel: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Id должен быть числом.");
                return;
            }

            var item = db.Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                Console.WriteLine("Элемент с таким Id не найден.");
                return;
            }

            if (!item.IsAssembly)
            {
                Console.WriteLine("Нельзя сделать разузловку для не ДСЕ.");
                return;
            }

            var reportRows = reportService.BuildReport(item.Batch);

            if (reportRows.Count == 0)
            {
                Console.WriteLine("Не удалось построить разузловку.");
                return;
            }

            string safeName = item.Name.Replace(" ", "_");
            string filePath = $"Report_{item.Id}_{safeName}.xlsx";

            excelService.Export(filePath, reportRows);

            Console.WriteLine($"Excel-файл успешно создан: {filePath}");
        }
    }
}