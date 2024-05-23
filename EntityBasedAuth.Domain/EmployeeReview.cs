namespace EntityBasedAuth.Domain
{
    public class EmployeeReview
    {
        public int CreatorId { get; protected set; }
        public int EmployeeId { get; protected set; }
        public string EmployeeName { get; protected set; } = string.Empty;
        public int Grade { get; protected set; }

        public EmployeeReview(int creatorId, int employeeId, string employeeName, int grade)
        {
            CreatorId = creatorId;
            EmployeeId = employeeId;
            EmployeeName = employeeName;
            Grade = grade;
        }
    }
}
