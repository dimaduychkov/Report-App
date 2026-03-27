using ReportApp.Data;
using ReportApp.Dtos;
using ReportApp.Models;

namespace ReportApp.Services
{
    public class AssemblyReportService
    {
        private readonly AppDbContext _db;

        public AssemblyReportService(AppDbContext db)
        {
            _db = db;
        }

        public List<ReportRow> BuildReport(string rootBatch)
        {
            var result = new List<ReportRow>();

            var rootItem = _db.Items.FirstOrDefault(x => x.Batch == rootBatch);

            if (rootItem == null)
                return result;

            int position = 1;

            AddItemWithChildren(rootItem, 0, result, ref position);

            return result;
        }

        private void AddItemWithChildren(Item item, int level,
            List<ReportRow> result, ref int position)
        {
            result.Add(new ReportRow
            {
                Position = position++,
                Level = level,
                Name = item.Name,
                IsAssembly = item.IsAssembly,
                Batch = item.Batch,
                ParentBatch = item.ParentBatch,
                Quantity = item.Quantity,
                SortOrder = item.SortOrder,
                Comment = item.Comment
            });

            var children = _db.Items
                .Where(x => x.ParentBatch == item.Batch)
                .OrderBy(x => x.SortOrder)
                .ToList();

            foreach (var child in children)
            {
                AddItemWithChildren(child, level + 1, result, ref position);
            }
        }
    }
}