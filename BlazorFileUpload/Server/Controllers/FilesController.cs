using BlazorFileUpload.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BlazorFileUpload.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        public FilesController()
        {
        } //FilesController


        [HttpPost("UploadFileChunk")]
        public bool UploadFileChunk([FromBody] FileChunkDto fileChunkDto)
        {
            try
            {
                // get the local filename
                string filePath = Environment.CurrentDirectory + "\\StaticFiles\\";
                string fileName = filePath + fileChunkDto.FileName;

                // delete the file if necessary
                if (fileChunkDto.FirstChunk && System.IO.File.Exists(fileName))
                    System.IO.File.Delete(fileName);

                // open for writing
                using (var stream = System.IO.File.OpenWrite(fileName))
                {
                    stream.Seek(fileChunkDto.Offset, SeekOrigin.Begin);
                    stream.Write(fileChunkDto.Data, 0, fileChunkDto.Data.Length);
                }

                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return false;
            }
        } //UploadFileChunk


        [HttpGet("GetFiles")]
        public List<string> GetFiles()
        {
            var result = new List<string>();
            var files = Directory.GetFiles(Environment.CurrentDirectory + "\\StaticFiles", "*.*");
            foreach (var file in files)
            {
                var justTheFileName = Path.GetFileName(file);
                result.Add($"files/{justTheFileName}");
            }

            return result;
        } //GetFiles

        [HttpGet("Download")]
        public IEnumerable<DownloadDto> DownloadableFiles(int page, int items)
        {
            var currentUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            var result = new List<DownloadDto>();
            var files = Directory.GetFiles(Environment.CurrentDirectory + "\\StaticFiles", "*.*");
            foreach (var file in files)
            {
                var justTheFileName = Path.GetFileName(file);
                var url = currentUrl + $"/files/{justTheFileName}";
                result.Add(new DownloadDto
                {
                    Time = DateTime.Now,
                    FileName= justTheFileName,
                    FileUrl = url
                });
            }


            return result.OrderByDescending(X=> X.Time).Skip((page * items) - items).Take(items).ToList();

        }




    }
}
