namespace Credit_Monitor.ViewModel
{
    public class ResolutionSubmissionViewModel
    {
        public string AppId { get; set; }
        public List<ResolutionItem> Resolutions { get; set; } = new();
    }

    public class ResolutionItem
    {
        // UI Helper Properties (Not posted)
        public int Index { get; set; }
        public string Label { get; set; }
        public string FinValue { get; set; }
        public string CatValue { get; set; }
        public string Discrepancy { get; set; }

        // Form Submission Properties (Posted to Controller)
        public Guid Id { get; set; }
        public string FieldName { get; set; }
        public string RecommendedAction { get; set; }
        public string Responsibility { get; set; }
        public string Remarks { get; set; }
    }
}