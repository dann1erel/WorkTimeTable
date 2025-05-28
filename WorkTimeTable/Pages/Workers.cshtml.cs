using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkTimeTable.DataBase;

namespace WorkTimeTable.Pages
{
    public class WorkersModel(ApplicationContext db) : PageModel
    {
        // для получения данных из бд
        public List<Worker> Workers { get; private set; } = [];

        // для отправки данных в бд
        [BindProperty]
        public Worker Worker { get; set; } = new();

        // тег select
        public List<SelectListItem> Options { get; set; } = null!;

        // для удаления с помощью чекбоксов
        [BindProperty]
        public List<int> AreChecked { get; set; } = [];

        public async Task<IActionResult> OnGet()
        {
            Workers = await db.Worker
                .Include(w => w.Department)
                .AsNoTracking()
                .ToListAsync();
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
