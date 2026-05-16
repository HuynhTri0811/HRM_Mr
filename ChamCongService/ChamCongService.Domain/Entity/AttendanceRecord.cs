using System;

namespace ChamCongService.Domain.Entity
{
    public class AttendanceRecord
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }
}
