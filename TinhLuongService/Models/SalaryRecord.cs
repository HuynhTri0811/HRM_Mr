namespace TinhLuongService.Models
{
    public class SalaryRecord
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Bonus { get; set; }
    }
}
