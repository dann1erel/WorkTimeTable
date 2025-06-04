using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkTimeTable.Data;
using WorkTimeTable.Models;

namespace WorkTimeTable.Pages
{
    public class WorkersModel(ApplicationContext db) : PageModel
    {
        // для получения данных из бд
        public PaginatedList<Worker>? Workers { get; private set; }

        // для отправки данных в бд
        [BindProperty]
        public Worker Worker { get; set; } = new();

        // тег select
        public List<SelectListItem> Options { get; set; } = null!;

        // для удаления с помощью чекбоксов
        [BindProperty]
        public List<int> AreChecked { get; set; } = [];

        // для сортировки
        public string? CurrentSort { get; set; }
        public string NameSort => CurrentSort == "name_desc" ? "name_asc" : "name_desc";
        public string PositionSort => CurrentSort == "pos_desc" ? "pos_asc" : "pos_desc";
        public string DepartmentSort => CurrentSort == "dep_desc" ? "dep_asc" : "dep_desc";

        // для поиска
        public string? NameFilter {  get; set; }
        public string? PositionFilter { get; set; }
        public string? DepartmentFilter { get; set; }


        public async Task<IActionResult> OnGet( string sortOrder, string searchNameString,
                                                string searchPositionString, string searchDepartmentString,
                                                string currentNameFilter, string currentPositionFilter,
                                                string currentDepartmentFilter, int? pageIndex)
        {
            CurrentSort = sortOrder;
            
            NameFilter = !String.IsNullOrEmpty(searchNameString) ? searchNameString : currentNameFilter;
            PositionFilter = !String.IsNullOrEmpty(searchPositionString) ? searchPositionString : currentPositionFilter;
            DepartmentFilter = !String.IsNullOrEmpty(searchDepartmentString) ? searchDepartmentString : currentDepartmentFilter;

            IQueryable<Worker> workers = db.Worker.Include(w => w.Department);

            if (searchNameString != null || searchPositionString != null || searchDepartmentString != null) pageIndex = 1;

            if (!String.IsNullOrEmpty(searchNameString))
            {
                workers = workers.Where(w => w.Name.Contains(searchNameString));
            }

            if (!String.IsNullOrEmpty(searchPositionString))
            {
                workers = workers.Where(w => w.Position.Contains(searchPositionString));
            }

            if (!String.IsNullOrEmpty(searchDepartmentString))
            {
                workers = workers.Where(w => w.Name.Contains(searchDepartmentString));
            }

            workers = sortOrder switch
            {
                "name_desc" => workers.OrderByDescending(w => w.Name),
                "name_asc" => workers.OrderBy(w => w.Name),
                "pos_desc" => workers.OrderByDescending(w => w.Position),
                "pos_asc" => workers.OrderBy(w => w.Position),
                "dep_desc" => workers.OrderByDescending(w => w.Department.Name),
                "dep_asc" => workers.OrderBy(w => w.Department.Name),
                _ => workers
            };

            int pageSize = 5;
            Workers = await PaginatedList<Worker>.CreateAsync(workers.AsNoTracking(), pageIndex ?? 1, pageSize);

            Options = await db.Department
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                .AsNoTracking()
                .ToListAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostAddAsync()
        {
            db.Worker.Add(Worker);
            await db.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync()
        {
            var workersToDelete = await db.Worker.Where(w => AreChecked.Contains(w.Id)).ToListAsync();
            if (workersToDelete != null)
            {
                db.Worker.RemoveRange(workersToDelete);
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(int id, string name, string position, int? departmentId)
        {
            var worker = await db.Worker.FindAsync(id);
            if (worker != null)
            {
                worker.Name = name;
                worker.Position = position;
                worker.DepartmentId = departmentId;
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }

    }
}
