using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WorkTimeTable.Data;
using WorkTimeTable.Models;

namespace WorkTimeTable.Pages
{
    public class IndexModel(ApplicationContext db) : PageModel
    {
        // параметры маршрута, потом формируется запрос к бд с такими параметрами
        [BindProperty(SupportsGet = true)]
        public int? WorkerShowId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? MonthShow { get; set; }
        
        // для получения данных из бд
        public List<Timetable> Timetables { get; set; } = [];

        // для тега select
        public List<SelectListItem> OptionsWorkers { get; set; } = null!;
        public List<SelectListItem> OptionsContracts { get; set; } = null!;
        public SelectList OptionsMonths {  get; set; } = null!;

        // для контрольной суммы
        private static readonly Dictionary<string, int> _months2Days = new Dictionary<string, int>()
        {
            {"Май", 18},
            {"Июнь", 19},
            {"Июль", 23},
            {"Август", 21}
        };
        public int DaysInMonth
        {
            get
            {
                if(MonthShow != null && _months2Days.TryGetValue(MonthShow, out int d)) return d;
                return 0;
            }
        }
        public int? HoursByContract { get; set; }

        // для удаления с помощью чекбоксов
        [BindProperty]
        public List<int> AreChecked { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {

            await CreateOptionsAsync();
            await GetDataAsync();

            return Page();
        }

        public IActionResult OnPostShowWorker()
        {
            if (WorkerShowId == null || MonthShow == null)
            {
                return RedirectToPage();
            }
            return RedirectToPage("./Index", new { workerShowId = WorkerShowId, monthShow = MonthShow });
        }

        public async Task<IActionResult> OnPostAddAsync(int contractId, int hours, int workerId, string month)
        {
            Timetable? timetableToAdd = await db.Timetable.FirstOrDefaultAsync(t => t.ContractId == contractId
                                                                    && t.WorkerId == workerId
                                                                    && t.Month == month);

            if (timetableToAdd == null)
            {
                timetableToAdd = new()
                {
                    ContractId = contractId,
                    Hours = hours,
                    Month = month,
                    WorkerId = workerId
                };
                db.Timetable.Add(timetableToAdd);
            }
            else
            {
                timetableToAdd.Hours += hours;
                db.Timetable.Update(timetableToAdd);
            }

            await db.SaveChangesAsync();
            return RedirectToPage("./Index", new { workerShowId = WorkerShowId, monthShow = MonthShow });
        }

        public async Task<IActionResult> OnPostEditAsync(int id, int hours)
        {
            var timetable = await db.Timetable.FindAsync(id);
            if (timetable != null) 
            { 
                timetable.Hours = hours;
                await db.SaveChangesAsync();
            }
            
            return RedirectToPage("./Index", new { workerShowId = WorkerShowId, monthShow = MonthShow });
        }

        public async Task<IActionResult> OnPostRemoveAsync()
        {
            var timetablesToDelete = await db.Timetable.Where(t => AreChecked.Contains(t.Id)).ToListAsync();
            if (timetablesToDelete != null)
            {
                db.Timetable.RemoveRange(timetablesToDelete);
                await db.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { workerShowId = WorkerShowId, monthShow = MonthShow });
        }

        private async Task CreateOptionsAsync()
        {
            OptionsWorkers = await db.Worker
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                .AsNoTracking()
                .ToListAsync();
            OptionsContracts = await db.Contract
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                .AsNoTracking()
                .ToListAsync();

            List<string> months = [];
            foreach (var month in _months2Days)
            {
                months.Add(month.Key);
            }

            OptionsMonths = new SelectList(months);
        }

        private async Task GetDataAsync()
        {
            Timetables = Timetables = await db.Timetable
                .Where(t => (t.WorkerId == WorkerShowId) && (t.Month == MonthShow))
                .Include(t => t.Contract)
                .Include(t => t.Worker)
                .AsNoTracking()
                .ToListAsync();
            if(MonthShow != null) HoursByContract = _months2Days[MonthShow!] * 8 - Timetables.Sum(t => t.Hours);
        }
    }
}

