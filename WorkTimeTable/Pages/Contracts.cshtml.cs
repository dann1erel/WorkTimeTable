using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkTimeTable.Data;
using WorkTimeTable.Models;

namespace WorkTimeTable.Pages
{
    public class ContractsModel(ApplicationContext db) : PageModel
    {
        // для получения данных из бд
        public PaginatedList<Contract>? Contracts { get; private set; }
        // для отправки данных в бд
        [BindProperty]
        public Contract Contract { get; set; } = new();

        // для удаления с помощью чекбоксов
        [BindProperty]
        public List<int> AreChecked { get; set; } = [];

        // для сортировки
        public string? CurrentSort { get; set; }
        public string? NameSort => CurrentSort == "name_desc" ? "name_asc" : 
                                   CurrentSort == "name_asc" ? null : "name_desc";

        // для поиска
        public string? NameFilter { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder, string searchNameString, 
                                                    string currentNameFilter, int? pageIndex)
        {
            CurrentSort = sortOrder;

            NameFilter = !String.IsNullOrEmpty(searchNameString) ? searchNameString : currentNameFilter;

            IQueryable<Contract> contracts = db.Contract;

            if (searchNameString != null) pageIndex = 1;

            if (!String.IsNullOrEmpty(searchNameString))
            {
                contracts = contracts.Where(d => d.Name.Contains(searchNameString));
            }

            contracts = sortOrder switch
            {
                "name_desc" => contracts.OrderByDescending(d => d.Name),
                "name_asc" => contracts.OrderBy(d => d.Name),
                _ => contracts 
            };

            int pageSize = 5;
            Contracts = await PaginatedList<Contract>.CreateAsync(contracts.AsNoTracking(), pageIndex ?? 1, pageSize);

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
