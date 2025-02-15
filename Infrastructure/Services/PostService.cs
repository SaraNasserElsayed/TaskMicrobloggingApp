using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;


namespace Infrastructure.Services;
public class PostService : IPostService
{
    private readonly AppDbContext _context;
    private readonly IBlobStorageService _blobStorageService;

    public PostService(AppDbContext context, IBlobStorageService blobStorageService)
    {
        _context = context;
        _blobStorageService = blobStorageService;
    }

    public List<Post> GetAllPosts()
    {
        return _context.Posts.OrderByDescending(p => p.CreatedAt).ToList();
    }

    public Post CreatePost(string text, IFormFile imageFile, string username)
    {
        var random = new Random();
        string imageUrl = imageFile != null ? _blobStorageService.UploadImageAsync(imageFile).Result : null;

        var post = new Post
        {
            Text = text,
            ImageUrl = imageUrl,
            Latitude = random.NextDouble() * 180 - 90,
            Longitude = random.NextDouble() * 360 - 180,
            Username = username
        };
        _context.Posts.Add(post);
        _context.SaveChanges();
        return post;
    }
}