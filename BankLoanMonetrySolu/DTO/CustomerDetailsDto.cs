using BankLoanMonetrySolu.Model;

namespace BankLoanMonetrySolu.DTO
{
    public class CustomerDetailsDto
    {
        public Guid Id { get; set; }
        public string? Tittle { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Middlename { get; set; }
        public string? Email { get; set; }
        public string? DOB { get; set; }
        public string? ResidentStatus { get; set; }
        public string? gender { get; set; }
        public string? NumbOfDependant { get; set; }
        public string? Nationallity { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Age { get; set; }
        public string? phoneNum { get; set; }
        public string? NameOfEmployer { get; set; }
        public string? ApplicantType { get; set; }
       // public FinacleData? loanDetails { get; set; }
    }
}
