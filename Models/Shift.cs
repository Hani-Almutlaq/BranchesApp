namespace BranchesApp.Models
{
    public class Shift
    {
        public int ShiftId { get; set; }
        
        // Foreign Keys
        public int BranchId { get; set; }
        public int DayId { get; set; }

        // Navigation Properties (EF Core purpose)
        public Branch? Branch { get; set; }
        public Day? Day { get; set; }

        // If both TimeSpan are null, Closed all day
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public bool OpenAllDay { get; set; }
    }
}
