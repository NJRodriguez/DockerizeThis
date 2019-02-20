using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DockerizeThis.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            //Path where you want to save the uploaded file.
            //Hardcoded for now, since all docker images should be using the same directory.
            var filePath = @"/ws_persist";

            //Should validate if directory above exists...
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //Upload file to directory with same filename.
                    using (var stream = new FileStream(Path.Combine(filePath,formFile.FileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { FileUploadedCount = files.Count, TotalFileSize = size, FileSaveDirectory = filePath });
        }
    }
}