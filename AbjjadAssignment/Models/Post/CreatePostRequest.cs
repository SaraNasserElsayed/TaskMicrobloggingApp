

using Microsoft.AspNetCore.Http;

namespace Core.Models.Post;
public class CreatePostRequest
{
    public string Text { get; set; }
    public IFormFile ImageFile { get; set; } // Uploaded image file
}
