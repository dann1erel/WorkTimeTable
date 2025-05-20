using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkTimeTable.DataBase;

namespace WorkTimeTable.Pages
{
    [IgnoreAntiforgeryToken]
    public class ContractsModel(ApplicationContext db) : PageModel
    {
        // для получения данных из бд
        public List<Contract> Contracts { get; private set; } = [];
        // для отправки данных в бд
        [BindProperty]
        public Contract Contract { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            Contracts = await db.Contract.AsNoTracking().ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            db.Contract.Add(Contract);
            await db.SaveChangesAsync();
            return RedirectToPage();
        }

    }
}
