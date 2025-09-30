using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OpenDoorsAPI.Models
{
    public class FileUploadRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
