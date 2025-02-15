using Core.Interfaces;
using Core.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public IActionResult GetAllPosts()
    {
        var posts = _postService.GetAllPosts();
        return Ok(posts);
    }

    [HttpPost]
    public IActionResult CreatePost([FromForm] CreatePostRequest request)
    {
        if (request.Text.Length > 140)
            return BadRequest("Post text cannot exceed 140 characters.");

        var post = _postService.CreatePost(request.Text, request.ImageFile, User.Identity.Name);
        return Ok(post);
    }
}
