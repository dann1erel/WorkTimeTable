using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkTimeTable.DataBase;

namespace WorkTimeTable.Pages
{
    [IgnoreAntiforgeryToken]
    public class WorkersModel(ApplicationContext db) : PageModel
    {
        // для получения данных из бд
        public List<Worker> Workers { get; private set; } = [];
        // для отправки данных в бд
        [BindProperty]
        public Worker Worker { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            Workers = await db.Worker.AsNoTracking().ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostAddAsync()
        {
            db.Worker.Add(Worker);
            await db.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int id)
        {
            var worker = await db.Worker.FindAsync(id);
            if (worker != null)
            {
                db.Worker.Remove(worker);
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(int id, string name, string position) // добавить department
        {
            var worker = await db.Worker.FindAsync(id);
            if (worker != null)
            {
                worker.Name = name;
                worker.Position = position;
                db.Worker.Update(worker);
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }

    }
}
