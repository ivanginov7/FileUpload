using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Files.Controllers
{
    public class UploadFilesController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        public UploadFilesController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        [HttpPost]
        public  IActionResult Index(IFormFile file)
        {
            //< check if a file is uploaded >
            if (file == null || file.Length == 0) return Content("File not selected");
            //</ check if a file is uploaded >

            //<get Path>
            string path_Root = _appEnvironment.WebRootPath;
            string path_to_images = path_Root + "\\uploads\\" + file.FileName;
            //</get Path>

            //< Copy File to Target >
            using (var stream=new FileStream(path_to_images, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            //</ Copy file to Target>

            ViewData["FilePath"] = path_to_images;
            return View();
            //var numOfFiles =Request.Form.Files.Count;
            //var path =Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\uploads");
            //var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            //ViewData["path"] = path.ToString();
            //ViewData["filePath"] = filePath.ToString();
            //foreach(var file in Request.Form.Files)
            //{
            //    if (file.Length > 0 && file != null)
            //    {
            //        using (var stream = new FileStream(filePath, FileMode.Create))
            //        {
            //            await file.CopyToAsync(stream);
            //        }
            //    }
            //}
            //return View(numOfFiles);
        }

        public IActionResult Multiple(List<IFormFile> files)
        {
            if (files.Count == 0) return Content("No files selected");
            int filesUploaded = 0;
            string path_Root = _appEnvironment.WebRootPath;
            List<string> filePaths = new List<string>();
            foreach (var file in files)
            {
                if(file!= null && file.Length != 0)
                {
                    string path_to_images = path_Root + "\\uploads\\" + file.FileName;
                    using(var stream = new FileStream(path_to_images, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    filesUploaded++;
                    filePaths.Add(path_to_images);
                }
            }
            ViewData["filesUploaded"] = filesUploaded;
            ViewData["filePaths"] = filePaths;
            return View();
        }
    }
}