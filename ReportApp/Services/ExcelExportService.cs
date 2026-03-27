using ClosedXML.Excel;
using ReportApp.Dtos;

namespace ReportApp.Services
{
    public class ExcelExportService
    {
        public void Export(string filePath, List<ReportRow> rows)
        {
            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Разузловка");

            // Заголовки колонок
            worksheet.Cell(1, 1).Value = "№";
            worksheet.Cell(1, 2).Value = "Уровень";
            worksheet.Cell(1, 3).Value = "Наименование";
            worksheet.Cell(1, 4).Value = "Тип";
            worksheet.Cell(1, 5).Value = "Партия";
            worksheet.Cell(1, 6).Value = "На партию";
            worksheet.Cell(1, 7).Value = "Количество";
            worksheet.Cell(1, 8).Value = "Комментарий";

            int rowIndex = 2;

            foreach (var row in rows)
            {
                worksheet.Cell(rowIndex, 1).Value = row.Position;
                worksheet.Cell(rowIndex, 2).Value = row.Level;

                // делаем визуальный отступ по уровню вложенности
                string indent = new string(' ', row.Level * 4);
                worksheet.Cell(rowIndex, 3).Value = indent + row.Name;

                worksheet.Cell(rowIndex, 4).Value =
                    row.IsAssembly ? "ДСЕ" : "Деталь";

                worksheet.Cell(rowIndex, 5).Value = row.Batch;
                worksheet.Cell(rowIndex, 6).Value = row.ParentBatch ?? "-";
                worksheet.Cell(rowIndex, 7).Value = row.Quantity;
                worksheet.Cell(rowIndex, 8).Value = row.Comment ?? "";

                rowIndex++;
            }

            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(filePath);
        }
    }
}