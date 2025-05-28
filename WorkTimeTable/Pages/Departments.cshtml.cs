using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkTimeTable.DataBase;

namespace WorkTimeTable.Pages
{
    public class DepartmentsModel(ApplicationContext db) : PageModel
    {
        // для получения данных из бд
        public List<Department> Departments { get; private set; } = [];
        // для отправки данных в бд
        [BindProperty]
        public Department Department { get; set; } = new();

        // для select
        public List<SelectListItem> Options { get; set; } = null!;

        // для удаления с помощью чекбоксов
        [BindProperty]
        public List<int> AreChecked { get; set; } = [];

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
