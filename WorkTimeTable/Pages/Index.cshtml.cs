using System.Reflection.Metadata.Ecma335;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WorkTimeTable.DataBase;

namespace WorkTimeTable.Pages
{
    public class IndexModel(ApplicationContext db) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? WorkerShowId { get; set; }
        public string? WorkerShowName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? MonthShow { get; set; }
        public int? HoursByContract { get; set; }
        public List<Timetable> Timetables { get; set; } = [];
        public List<SelectListItem> OptionsWorkers { get; set; } = null!;
        public List<SelectListItem> OptionsContracts { get; set; } = null!;
        public SelectList OptionsMonths {  get; set; } = null!;
        private static readonly string[] items = ["Май", "Июнь", "Июль"]; // заменить на что-то умное

        public async Task<IActionResult> OnGetAsync()
        {

            await CreateOptionsAsync();

            Timetables = await db.Timetable
                .Where(t => (t.WorkerId == WorkerShowId) && (t.Month == MonthShow))
                .Include(t => t.Contract)
                .Include(t => t.Worker)
                .AsNoTracking()
                .ToListAsync(); // группировка, чтобы выводилась сумма по одному документу

            HoursByContract = Timetables.Sum(t => t.Hours);

            WorkerShowName = await db.Worker
                .Where(w => w.Id == WorkerShowId)
                .Select(w => w.Name)
                .FirstOrDefaultAsync();

            return Page();
        }

        public IActionResult OnPostShowWorker()
        {
            if (WorkerShowId == null || MonthShow == null)
            {
                return RedirectToPage("./Index");
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
            if (timetable != null) timetable.Hours = hours;
            await db.SaveChangesAsync();

            return RedirectToPage("./Index", new { workerShowId = WorkerShowId, monthShow = MonthShow });
        }

        public async Task<IActionResult> OnPostRemoveAsync(int id)
        {
            var timetable = await db.Timetable.FindAsync(id);
            if (timetable != null)
            {
                db.Timetable.Remove(timetable);
                await db.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { workerShowId = WorkerShowId, monthShow = MonthShow });
        }

        public async Task CreateOptionsAsync()
        {
            OptionsWorkers = await db.Worker
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                .AsNoTracking()
                .ToListAsync();
            OptionsContracts = await db.Contract
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                .AsNoTracking()
                .ToListAsync();
            OptionsMonths = new SelectList(items); // заменить на что-нибудь более-умное
        }

    }
}
