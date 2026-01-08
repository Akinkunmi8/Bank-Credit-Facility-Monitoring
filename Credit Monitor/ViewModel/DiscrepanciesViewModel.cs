public class ValidationResult
{
    public string Message { get; set; } = "Match";
    public bool IsValid { get; set; } = true;
    public string DiscrepancyAmount { get; set; } = string.Empty;
}

public class DiscrepanciesViewModel
{
    public string AccNum { get; set; }
    public int ErrorCount { get; set; }
    public int TotalFields { get; set; } = 8; // Adjust based on how many fields you check

    public ValidationResult CustomerName { get; set; } = new();
    public ValidationResult FacilityType { get; set; } = new();
    public ValidationResult ApprovalLimit { get; set; } = new();
    public ValidationResult Interest { get; set; } = new();
    public ValidationResult MaturityDate { get; set; } = new();
    public ValidationResult Directorate { get; set; } = new();
    public ValidationResult DisbursmentRequired { get; set; } = new();
    public ValidationResult Fee { get; set; } = new();
    public ValidationResult ValueDate { get; set; } = new();
}