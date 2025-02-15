using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
