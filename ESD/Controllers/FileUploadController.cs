using ESD.Data;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ESD.Controllers
{
    public class FileUploadController : Controller
    {
        private ApplicationDbContext context;
        public FileUploadController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        public IActionResult FileUploadPage(int? Id)
        {
            TempData["id"] = Id;
            
            return View();
        }

        [Authorize(Roles = "Admin,QA Manager")]
        public IActionResult FileDownloadPage(string filePath, int? Id)
        {
            string[] filePaths = Directory.GetFiles(filePath);
            var filename = Id+".zip";
            var tempOutput = filePath + filename;
            using (ZipOutputStream oZipOutputStream = new ZipOutputStream(System.IO.File.OpenWrite(tempOutput)))
            {
                oZipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];

                foreach (var file in filePaths)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    oZipOutputStream.PutNextEntry(entry);

                    using (FileStream oFileStream = System.IO.File.OpenRead(file))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = oFileStream.Read(buffer, 0, buffer.Length);
                            oZipOutputStream.Write(buffer, 0, sourceBytes);
                        }while(sourceBytes > 0);
                    }
                }

                oZipOutputStream.Finish();
                oZipOutputStream.Flush();
                oZipOutputStream.Close();
            }

            byte[] finalResult= System.IO.File.ReadAllBytes(tempOutput);
            if (System.IO.File.Exists(tempOutput))
            {
                System.IO.File.Delete(tempOutput);
            }
            if (finalResult == null || !finalResult.Any())
            {
                throw new Exception(String.Format("No file Uploaded"));
            }

            //return RedirectToAction("Details", "Ideas", new { id = Id });
            return File(finalResult, "application/zip", filename);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveFile(IFormFile File1)
        {
            //int? tempId = (int?)TempData["id"];

            int Id = (int)TempData["id"];

            string FileName = File1.FileName;
            if (File1 != null)
            {
                string folder = "wwwroot/files/" + Id + "/";
                if (Directory.Exists(folder))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), folder, FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    File1.CopyToAsync(stream);
                }
                else
                {
                    Directory.CreateDirectory(folder);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), folder, FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    File1.CopyToAsync(stream);
                }

                var currentIdea = context.Ideas.Where(i => i.Id == Id).FirstOrDefault();
                currentIdea.FilePath = folder;
                context.Update(currentIdea);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Edit", "Ideas", new { id = Id });
        }
    }
}
