public class ResolveDiscrepancyDto
{
    public Guid Id { get; set; }
    public Guid AppId { get; set; }

    public string ResolutionAction { get; set; } = string.Empty;
    public string? ResolutionRemarks { get; set; }
}