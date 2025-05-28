using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkTimeTable.DataBase;

namespace WorkTimeTable.Pages
{
    public class ContractsModel(ApplicationContext db) : PageModel
    {
        // для получения данных из бд
        public List<Contract> Contracts { get; private set; } = [];
        // для отправки данных в бд
        [BindProperty]
        public Contract Contract { get; set; } = new();

        // для удаления с помощью чекбоксов
        [BindProperty]
        public List<int> AreChecked { get; set; } = [];

        public async Task<IActionResult> OnGet()
        {
            Contracts = await db.Contract.AsNoTracking().ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            db.Contract.Add(Contract);
            await db.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync()
        {
            var contractsToDelete = await db.Contract.Where(c => AreChecked.Contains(c.Id)).ToListAsync();
            if (contractsToDelete != null)
            {
                db.Contract.RemoveRange(contractsToDelete);
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(int id, string name)
        {
            var contract = await db.Contract.FindAsync(id);
            if (contract != null)
            {
                contract.Name = name;
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
