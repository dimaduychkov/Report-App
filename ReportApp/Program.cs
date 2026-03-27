using ReportApp.Data;
using ReportApp.Services;

namespace ReportApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new AppDbContext();

            DataSeeder.Seed(db);

            var reportService = new AssemblyReportService(db);
            var reportRows = reportService.BuildReport("111-111");

            Console.WriteLine("Разузловка для Клапана:");
            Console.WriteLine();

            foreach (var row in reportRows)
            {
                string indent = new string(' ', row.Level * 2);
                string type = row.IsAssembly ? "ДСЕ" : "Деталь";

                Console.WriteLine(
                    $"{row.Position}. {indent}{row.Name} | {type} | Партия: {row.Batch} | На партию: {row.ParentBatch ?? "-"} | Кол-во: {row.Quantity}");
            }

            Console.WriteLine();
            Console.WriteLine("Нажми любую клавишу...");
            Console.ReadKey();
        }
    }
}