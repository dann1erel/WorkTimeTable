using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkTimeTable.DataBase;

namespace WorkTimeTable.Pages
{
    [IgnoreAntiforgeryToken]
    public class ContractsModel(ApplicationContext db) : PageModel
    {
        // ��� ��������� ������ �� ��
        public List<Contract> Contracts { get; private set; } = [];
        // ��� �������� ������ � ��
        [BindProperty]
        public Contract Contract { get; set; } = new();

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

        public async Task<IActionResult> OnPostRemoveAsync(int id)
        {
            var contract = await db.Contract.FindAsync(id);
            if (contract != null)
            {
                db.Contract.Remove(contract);
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
                db.Contract.Update(contract);
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
