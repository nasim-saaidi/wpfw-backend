using Microsoft.EntityFrameworkCore;
using Models.project;
using Models.blog;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Models.user;



namespace main.db;
public sealed class PortDB : IdentityDbContext<User>
{

         public PortDB(DbContextOptions<PortDB> options) : base(options) {}
         public DbSet<Project> Projects => Set<Project>();
         public DbSet<Blog> Blogs => Set<Blog>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Project>(project =>
        {
        project.ToTable("Projects");
        project.HasKey(p => p.Id);
        project.Property(p => p.Repo_link).IsRequired();
        project.Property(p => p.Title).IsRequired();
        project.Property(p => p.Description).IsRequired();
        project.Property(p => p.Category).IsRequired();
        project.Property(p => p.Date).IsRequired();
        project.Property(p => p.Img_alt).IsRequired();
        project.Property(p => p.Img_src).IsRequired();
        });

        modelBuilder.Entity<Blog>(blog =>
        {
        blog.ToTable("Blogs");
        blog.HasKey(p => p.Id);
        blog.Property(p => p.Title).IsRequired();
        blog.Property(p => p.Content).IsRequired();
        blog.Property(p => p.Date).IsRequired();
        blog.Property(p => p.InternalNotes);
        });
        
    }
}