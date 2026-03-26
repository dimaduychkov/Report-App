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

            Console.WriteLine("База заполнена тестовыми данными.");
            Console.ReadKey();
        }
    }
}