using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkTimeTable.DataBase;

namespace WorkTimeTable.Pages
{
    [IgnoreAntiforgeryToken]
    public class DepartmentsModel(ApplicationContext db) : PageModel
    {
        // ��� ��������� ������ �� ��
        public List<Department> Departments { get; private set; } = [];
        // ��� �������� ������ � ��
        [BindProperty]
        public Department Department { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            Departments = await db.Department.AsNoTracking().ToListAsync();
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
                db.Department.Remove(Department);
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(int id, string name, string position) // �������� department
        {
            var department = await db.Department.FindAsync(id);
            if (department != null)
            {
                department.Name = name;
                db.Department.Update(department);
                await db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
