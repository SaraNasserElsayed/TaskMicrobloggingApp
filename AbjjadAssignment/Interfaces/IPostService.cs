using Core.Entities;
using Microsoft.AspNetCore.Http;
namespace Core.Interfaces;

public interface IPostService
{
    List<Post> GetAllPosts();
    Post CreatePost(string text, IFormFile imageFile, string username);
}
