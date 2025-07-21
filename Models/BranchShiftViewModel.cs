namespace BranchesApp.Models
{
    public class BranchShiftViewModel
    {
        public string? BranchName { get; set; }
        public string? DayName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsOpen { get; set; }
    }
}
