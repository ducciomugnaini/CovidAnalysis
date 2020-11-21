using System;
using System.IO;
using System.Threading.Tasks;
using DataAnalysisWeb.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualBasic.FileIO;

namespace DataAnalysisWeb.Controllers
{
    public class FileController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile csvFile)
        {
            if (csvFile != null)
            {
                string csv = Guid.NewGuid() + Path.GetExtension(csvFile.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", csv);

                var delimiter = Request.Form["delimiter"][0];
                if(delimiter.Equals("?"))
                    delimiter = Request.Form["otherDelimiter"][0];

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    // copy file in local directory
                    csvFile.CopyTo(stream);

                    // extract fields from header (first row)
                    Utilities.ExtractFields(stream, delimiter);
                }

                return RedirectToAction("Configurator", "Home", new { csv });
            }

            return null;
        }
    }
}
