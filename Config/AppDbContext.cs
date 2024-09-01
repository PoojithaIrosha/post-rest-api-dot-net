using Microsoft.EntityFrameworkCore;
using PostCrud.Model;

namespace PostCrud.Config;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
}