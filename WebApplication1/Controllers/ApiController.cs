﻿using Microsoft.AspNetCore.Mvc;
using MSIT153Site.Models;
using MSIT153Site.Models.ViewModel;
using System.IO;
using WebApplication1.Models;

namespace MSIT153Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private readonly DemoContext _context;
        public ApiController(IWebHostEnvironment host, DemoContext context)
        {
            _host = host;
            _context = context;
        }
        //  public IActionResult Index(string name, int age=30)
        public IActionResult Index(UserViewModel user)
        {
            System.Threading.Thread.Sleep(1000);
            if (string.IsNullOrEmpty(user.name))
            {
                user.name = "guest";
            }
            //return Content("<h2>Ajax 你好 !!</h2>","text/html", System.Text.Encoding.UTF8);
            return Content($"Hello {user.name}， You are {user.age} years old.");
        }

        public IActionResult Register(Members member, IFormFile formFile)
        {
            //實際路徑
            //C:\Users\User\Documents\workspace\MSIT153Site\wwwroot\uploads\abc.jpg
            //專案根目錄的實際路徑
            //string strPath = _host.ContentRootPath; //C:\Users\User\Documents\workspace\MSIT153Site
            //wwwroot的實際路徑
            //tring strPath = _host.WebRootPath; //C:\Users\User\Documents\workspace\MSIT153Site\wwwroot
            string strPath = Path.Combine(_host.WebRootPath, "uploads", formFile.FileName);
            //將檔案存到uploads資料夾中
            using (var fileStream = new FileStream(strPath, FileMode.Create))
            {
                formFile.CopyTo(fileStream);
            }

            member.FileName = formFile.FileName;
            //將上傳的圖轉成二進位
            byte[]? imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            member.FileData = imgByte;

            //將資料寫進資料庫中
            _context.Members.Add(member);
            _context.SaveChanges();

            return Content("新增成功");
            //檔案名稱、檔案大小、檔案類型
            //string fileInfo = $"{formFile?.FileName} - {formFile?.Length} - {formFile?.ContentType}";
            //return Content(fileInfo);
            //return Content("<h2>Ajax 你好 !!</h2>","text/html", System.Text.Encoding.UTF8);
            //return Content($"Hello {member.name}，{member.email},  You are {member.age} years old.");
        }
        public IActionResult Cities() 
        {
           var city= _context.Address.Select(c => c.City).Distinct();
            return Json(city);
        }

        public IActionResult districts(string city)
        {
            var districts = _context.Address.Where(d=>d.City==city).Select(a=>a.SiteId).Distinct();
            return Json(districts);
        }
        public IActionResult road(string districts)
        {
            var road = _context.Address.Where(d => d.SiteId == districts).Select(a => a.Road).Distinct();
            return Json(road);
        }

        public IActionResult Pic(int id=1)
        {
          Members? members= _context.Members.Find(id);//找PK專用的語法 其他不行
            byte[]? img = members?.FileData;
            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            return NotFound();
        }

   
    }
}
