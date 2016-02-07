using ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using WebApplicationppp.Models;

namespace WebApplicationppp.Controllers
{
    public class SaveImagesController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();
        // POST: api/SaveImages + ?id!
        //[Route("api/SaveImages/{id:int}")]
        public async Task<IHttpActionResult> PostSalonImage(int id, Boolean main, [FromUri]string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the request contains multipart/form-data.
            if (Request.Content.IsMimeMultipartContent())
            {
                var streamProvider = new MultipartMemoryStreamProvider();
                streamProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);

                foreach (var item in streamProvider.Contents.Where(c => !string.IsNullOrEmpty(c.Headers.ContentDisposition.FileName)))
                {
                    Stream stPictureSource = new MemoryStream(await item.ReadAsByteArrayAsync());

                    // Resize for Picture
                    MemoryStream stPictureDest = new MemoryStream();
                    var pictureSettings = new ResizeSettings
                    {
                        MaxWidth = 1000,
                        MaxHeight = 1000,
                        Mode = FitMode.Max
                    };
                    ImageBuilder.Current.Build(stPictureSource, stPictureDest, pictureSettings);

                    string fileName = item.Headers.ContentDisposition.FileName;

                    fileName = fileName.Replace("\"", string.Empty);

                    string ext = Path.GetExtension(fileName);

                    string newFileName = Guid.NewGuid().ToString() + ext;

                    File.WriteAllBytes(HostingEnvironment.MapPath("~/Images/SalonImages/" + newFileName), stPictureDest.ToArray());

                    db.SalonImages.Add(new SalonImage
                    {
                        AddedOn = DateTime.Now,
                        CreateBy = username,
                        ImageName = fileName,
                        ImagePath = newFileName,
                        IsDeleted = false,
                        IsMain = main,
                        SalonID = id,

                    });
                }

                await db.SaveChangesAsync();
                return Ok();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

        }

        // POST: api/SaveImageEmpl + ?id!
        [Route("api/SaveImageEmpl")]
        public async Task<IHttpActionResult> PostEmplImage(int id, [FromUri]string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the request contains multipart/form-data.
            if (Request.Content.IsMimeMultipartContent())
            {
                Employee empl = await db.Employees.FindAsync(id);
                if (empl != null)
                {
                    var streamProvider = new MultipartMemoryStreamProvider();
                    streamProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);

                    foreach (var item in streamProvider.Contents.Where(c => !string.IsNullOrEmpty(c.Headers.ContentDisposition.FileName)))
                    {
                        Stream stPictureSource = new MemoryStream(await item.ReadAsByteArrayAsync());

                        // Resize for Picture
                        MemoryStream stPictureDest = new MemoryStream();
                        var pictureSettings = new ResizeSettings
                        {
                            MaxWidth = 1000,
                            MaxHeight = 1000,
                            Mode = FitMode.Max
                        };
                        ImageBuilder.Current.Build(stPictureSource, stPictureDest, pictureSettings);

                        string fileName = item.Headers.ContentDisposition.FileName;

                        fileName = fileName.Replace("\"", string.Empty);

                        string ext = Path.GetExtension(fileName);

                        string newFileName = Guid.NewGuid().ToString() + ext;

                        File.WriteAllBytes(HostingEnvironment.MapPath("~/Images/EmplImages/" + newFileName), stPictureDest.ToArray());

                        empl.ImageUrl = newFileName;
                    }

                    await db.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

        }

        // POST: api/SaveImageEmpl + ?id!
        [Route("api/SaveImageService")]
        public async Task<IHttpActionResult> PostServiceImage(int id, [FromUri]string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the request contains multipart/form-data.
            if (Request.Content.IsMimeMultipartContent())
            {
                Service service = await db.Services.FindAsync(id);
                if (service != null)
                {
                    var streamProvider = new MultipartMemoryStreamProvider();
                    streamProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);

                    foreach (var item in streamProvider.Contents.Where(c => !string.IsNullOrEmpty(c.Headers.ContentDisposition.FileName)))
                    {
                        Stream stPictureSource = new MemoryStream(await item.ReadAsByteArrayAsync());

                        // Resize for Picture
                        MemoryStream stPictureDest = new MemoryStream();
                        var pictureSettings = new ResizeSettings
                        {
                            MaxWidth = 1000,
                            MaxHeight = 1000,
                            Mode = FitMode.Max
                        };
                        ImageBuilder.Current.Build(stPictureSource, stPictureDest, pictureSettings);

                        string fileName = item.Headers.ContentDisposition.FileName;

                        fileName = fileName.Replace("\"", string.Empty);

                        string ext = Path.GetExtension(fileName);

                        string newFileName = Guid.NewGuid().ToString() + ext;

                        File.WriteAllBytes(HostingEnvironment.MapPath("~/Images/ServiceImages/" + newFileName), stPictureDest.ToArray());

                        service.ImageUrl = newFileName;
                    }

                    await db.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

        }
    }
}
