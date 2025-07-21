namespace BranchesApp.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? BranchAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
