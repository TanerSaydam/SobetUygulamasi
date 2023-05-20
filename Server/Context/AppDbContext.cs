using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Server.Context;

public sealed class AppDbContext : IdentityDbContext<AppUser,AppRole,int>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<IdentityUserLogin<int>>();
        builder.Ignore<IdentityUserRole<int>>();
        builder.Ignore<IdentityUserClaim<int>>();
        builder.Ignore<IdentityUserToken<int>>();
        builder.Ignore<IdentityRoleClaim<int>>();
        builder.Ignore<IdentityRole<int>>();
    }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
}

public sealed class AppUser: IdentityUser<int>{}

public sealed class AppRole : IdentityRole<int>{}

public sealed class Chat
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }

    public ICollection<Message> Messages { get; set; }    
}

public sealed class Message
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
    public int UserId { get; set; }
    public int ChatId { get; set; }

    public AppUser User { get; set; }
    public Chat Chat { get; set; }
}


