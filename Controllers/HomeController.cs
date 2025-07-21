using BranchesApp.Data;
using BranchesApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BranchesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var branches = await _context.Branches.ToListAsync();
            var viewModelList = new List<BranchShiftViewModel>();

            foreach (var branch in branches)
            {
                var shift = await GetNextOrCurrentShift(branch.BranchId);

                viewModelList.Add(new BranchShiftViewModel
                {
                    BranchName = branch.BranchName,
                    DayName = shift?.DayId switch
                    {
                        1 => "Sunday",
                        2 => "Monday",
                        3 => "Tuesday",
                        4 => "Wednesday",
                        5 => "Thursday",
                        6 => "Friday",
                        7 => "Saturday",
                        _ => "empty"
                    },
                    StartTime = shift?.StartTime ?? TimeSpan.Zero,
                    EndTime = shift?.EndTime ?? TimeSpan.Zero,
                    IsOpen = await GetAvailability(branch.BranchId)
                });
            }

            return View(viewModelList);
        }

        public async Task<Shift?> GetNextOrCurrentShift(int branchId)
        {
            var now = DateTime.Now;
            var currentDayOfWeek = (int)now.DayOfWeek + 1;
            var currentTimeOfDay = now.TimeOfDay;

            // Get today’s shifts for this branch
            var shiftsList = await _context.Shifts
                .Where(s => s.BranchId == branchId && s.DayId == currentDayOfWeek)
                .ToListAsync();

            // Check for current shift
            var currentShift = shiftsList.FirstOrDefault(s => s.StartTime <= currentTimeOfDay && s.EndTime >= currentTimeOfDay);

            if (currentShift != null)
                return currentShift;

            // Check for next shift today
            var upcomingShiftsToday = shiftsList
                .Where(s => s.StartTime > currentTimeOfDay)
                .OrderBy(s => s.StartTime)
                .ToList();

            var nextTodayShift = upcomingShiftsToday.FirstOrDefault();

            if (nextTodayShift != null)
                return nextTodayShift;

            // If there is no shifts today, move to the next day
            var nextDay = currentDayOfWeek + 1;

            if (nextDay == 8)
                nextDay = 1;

            var upcomingShiftsList = await _context.Shifts
                .Where(s => s.BranchId == branchId && s.DayId == nextDay)
                .OrderBy(s => s.DayId)
                .ToListAsync();

            upcomingShiftsList = upcomingShiftsList.OrderBy(s => s.StartTime).ToList();

            if (!upcomingShiftsList.Any())
            {
                int counter = 0;
                do
                {
                    if (nextDay == 7)
                        nextDay = 0;

                    nextDay++;

                    upcomingShiftsList = await _context.Shifts
                        .Where(s => s.BranchId == branchId && s.DayId == nextDay)
                        .OrderBy(s => s.DayId)
                        .ToListAsync();

                    upcomingShiftsList = upcomingShiftsList.OrderBy(s => s.StartTime).ToList();

                    if (upcomingShiftsList.Any())
                        break;

                    // To break the loop in case the branch has no shifts
                    counter++;
                    if (counter == 9)
                        break;

                } while (!upcomingShiftsList.Any());
            }

            var upcommingShift = upcomingShiftsList.FirstOrDefault();

            return upcommingShift;
        }

        public async Task<bool> GetAvailability(int branchId)
        {
            var now = DateTime.Now;
            var currentTimeOfDay = now.TimeOfDay;

            // Get the shift
            var shift = await GetNextOrCurrentShift(branchId);

            TimeSpan start = shift?.StartTime ?? TimeSpan.Zero;
            TimeSpan end = shift?.EndTime ?? TimeSpan.Zero;

            if (shift == null)
                return false;

            // Check if the branch is open now
            bool isOpen = currentTimeOfDay >= start && currentTimeOfDay <= end;
            return isOpen;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
