using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisWeb.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile csvFile)
        {
            if (csvFile != null)
            {
                string csvTempName = Guid.NewGuid() + Path.GetExtension(csvFile.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", csvTempName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    csvFile.CopyTo(stream);
                }

                return RedirectToAction("Index", "Home");
            }

            return null;
        }
    }
}
