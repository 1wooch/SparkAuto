using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Model;
using SparkAuto.Model.ViewModel;
using SparkAuto.Utility;

namespace SparkAuto.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public UserListViewModel UserListVM { get; set; }

        public async Task<IActionResult> OnGet(int productPage=1)
        {
            UserListVM = new UserListViewModel()
            {
                ApplicationUserList = await _db.ApplicationUser.ToListAsync()
            };

            StringBuilder param = new StringBuilder();
            param.Append("/Users?productPage=:");

            var count = UserListVM.ApplicationUserList.Count;

            UserListVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = SD.PaginationUsersPageSize,
                TotalItems = count,
                UrlParam = param.ToString()
            };

            UserListVM.ApplicationUserList = UserListVM.ApplicationUserList.OrderBy(p => p.Email)
                .Skip((productPage - 1 * SD.PaginationUsersPageSize)).Take(SD.PaginationUsersPageSize).ToList();

            return Page();
        }
    }
}
