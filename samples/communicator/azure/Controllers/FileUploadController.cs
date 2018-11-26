using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using ChatRoom.Model;

namespace ChatRoom.Controllers
{
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
 
        public FileUploadController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
 
        [Route("files")]
        [HttpPost]
        [ServiceFilter(typeof(ValidateMimeMultipartContentFilter))]
        public async Task<IActionResult> UploadFiles(FileDescriptionShort fileDescriptionShort)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
 
                            var imageMessage = new ImageMessage
                            {
                                ImageHeaders = "data:" + file.ContentType + ";base64,",
                                ImageBinary = memoryStream.ToArray()
                            };
 
                            await _hubContext.Clients.All.SendAsync("ImageMessage", imageMessage);
                        }
                    }
                }
            }
 
            return Redirect("/FileClient/Index");
        }
    }
}