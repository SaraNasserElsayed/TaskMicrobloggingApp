using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;
public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Users.Any())
        {
               context.Users.AddRange(
                new User { Username = "admin", Password = BCrypt.Net.BCrypt.HashPassword("password") },
                new User { Username = "user1", Password = BCrypt.Net.BCrypt.HashPassword("1234") }
            );
        }

        if (!context.Posts.Any())
        {
            context.Posts.AddRange(
                new Post { Text = "Hello World!", Username = "admin", CreatedAt = DateTime.UtcNow },
                new Post { Text = "This is a seeded post.", Username = "user1", CreatedAt = DateTime.UtcNow }
            );
        }

        context.SaveChanges();
    }
}

