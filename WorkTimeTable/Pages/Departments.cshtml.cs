using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkTimeTable.Data;
using WorkTimeTable.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WorkTimeTable.Pages
{
    public class DepartmentsModel(ApplicationContext db) : PageModel
    {
        // для получения данных из бд
        public PaginatedList<Department>? Departments { get; private set; }
        // для отправки данных в бд
        [BindProperty]
        public Department Department { get; set; } = new();

        // для select
        public List<SelectListItem> Options { get; set; } = null!;

        // для удаления с помощью чекбоксов
        [BindProperty]
        public List<int> AreChecked { get; set; } = [];

        // для сортировки
        public string? CurrentSort { get; private set; }
        public string NameSort => CurrentSort == "name_desc" ? "name_asc" : "name_desc";
        public string LeaderSort => CurrentSort == "leader_desc" ? "leader_asc" : "leader_desc";

        // для поиска
        public string? NameFilter { get; set; }
        public string? LeaderFilter { get; set; }


        public async Task<IActionResult> OnGetAsync(string sortOrder, string searchNameString, 
                                                    string searchLeaderString, string currentNameFilter,
                                                    string currentLeaderFilter, int? pageIndex)
        {
            CurrentSort = sortOrder;

            NameFilter = !String.IsNullOrEmpty(searchNameString) ? searchNameString : currentNameFilter;
            LeaderFilter = !String.IsNullOrEmpty(searchLeaderString) ? searchLeaderString : currentLeaderFilter;

            IQueryable<Department> deps = db.Department.Include(d => d.Leader);

            if (searchNameString != null || searchLeaderString != null) pageIndex = 1;

            if (!String.IsNullOrEmpty(searchNameString) )
            {
                deps = deps.Where(d => d.Name.Contains(searchNameString));
            }

            if (!String.IsNullOrEmpty(searchLeaderString))
            {
                deps = deps.Where(d => d.Leader.Name.Contains(searchLeaderString));
            }

            deps = sortOrder switch
            {
                "name_desc" => deps.OrderByDescending(d => d.Name),
                "name_asc" => deps.OrderBy(d => d.Name),
                "lead_desc" => deps.OrderByDescending(d => d.Leader.Name),
                "lead_asc" => deps.OrderBy(d => d.Leader.Name),
                _ => deps
            };

            int pageSize = 5;
            Departments = await PaginatedList<Department>.CreateAsync(deps.AsNoTracking(), pageIndex ?? 1, pageSize);

            Options = await db.Worker
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name + " " + a.Position })
                .AsNoTracking()
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            db.Department.Add(Department);
            await db.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync() // можно вынести в отдельную функцию и отдельный класс
        {
            var departmentsToDelete = await db.Department.Where(d => AreChecked.Contains(d.Id)).ToListAsync();
            if (departmentsToDelete != null)
            {
                db.Department.RemoveRange(departmentsToDelete);
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(int id, string name, int? leaderId) // добавить leader
        {
            var department = await db.Department.FindAsync(id);
            if (department != null)
            {
                department.Name = name;
                department.LeaderId = leaderId;
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
