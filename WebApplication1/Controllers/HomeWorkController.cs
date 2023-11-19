using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT153Site.Models;
using MSIT153Site.Models.ViewModel;
using System.Diagnostics;
using WebApplication1.Models;

namespace MSIT153Site.Controllers
{   
    public class HomeWorkController : Controller
    {  private readonly DemoContext _context;
       private readonly IWebHostEnvironment _host;

        public HomeWorkController(DemoContext context, IWebHostEnvironment host) 
        {
            _context = context;
            _host = host;
        }

        public IActionResult HW1()
        {

            return View();
        }

        public IActionResult HW2() 
        {
        return View();
        }

        public IActionResult CheckAccount(MemberViewModel vm)
        {
            if (_context.Members.Any(p => p.Name == vm.name))
            {
                return Content("此名稱已被使用");
            }
            else
            {
                return Content("此名稱可使用");
            }
        }

        public IActionResult HW3()
        {
            return View();
        }



    }
}
