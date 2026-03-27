namespace ReportApp.Dtos
{
    public class ReportRow
    {
        public int Position { get; set; }
        public int Level { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsAssembly { get; set; }
        public string Batch { get; set; } = string.Empty;
        public string? ParentBatch { get; set; }
        public int Quantity { get; set; }
        public int SortOrder { get; set; }
        public string? Comment { get; set; }
    }
}