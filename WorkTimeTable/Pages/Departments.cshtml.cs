using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkTimeTable.DataBase;

namespace WorkTimeTable.Pages
{
    [IgnoreAntiforgeryToken]
    public class DepartmentsModel(ApplicationContext db) : PageModel
    {
        // для получения данных из бд
        public List<Department> Departments { get; private set; } = [];
        // для отправки данных в бд
        [BindProperty]
        public Department Department { get; set; } = new();
        public List<SelectListItem> Options { get; set; } = null!;

        public async Task<IActionResult> OnGet()
        {
            Departments = await db.Department
                .Include(d => d.Leader)
                .AsNoTracking()
                .ToListAsync();
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

        public async Task<IActionResult> OnPostRemoveAsync(int id)
        {
            var department = await db.Department.FindAsync(id);
            if (department != null)
            {
                db.Department.Remove(department);
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
                db.Department.Update(department);
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
