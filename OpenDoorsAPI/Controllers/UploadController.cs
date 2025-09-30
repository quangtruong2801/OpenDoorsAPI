using Microsoft.AspNetCore.Mvc;
using OpenDoorsAPI.Services;

namespace OpenDoorsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;

        public UploadController(CloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        // POST api/upload/upload
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] FileUploadRequest request)
        {
            var file = request.File;
            if (file == null || file.Length == 0)
                return BadRequest("File không được để trống.");

            using var stream = file.OpenReadStream();
            var result = await _cloudinaryService.UploadImageAsync(stream, file.FileName, "member_upload");

            return Ok(new
            {
                url = result.SecureUrl.ToString(),
                publicId = result.PublicId
            });
        }

        // DELETE api/upload/{publicId} - chỉ xóa ảnh trên Cloudinary
        [HttpDelete("{publicId}")]
        public async Task<IActionResult> DeleteImage(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return BadRequest("PublicId không được để trống.");

            var result = await _cloudinaryService.DeleteImageAsync(publicId);
            return Ok(result);
        }
    }

    // DTO dùng cho upload
    public class FileUploadRequest
    {
        public IFormFile File { get; set; }
    }
}
