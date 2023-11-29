using Bloggie.Models.Domain;
using Bloggie.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Blog.Web.Data

{
    public class BlogDBContext : DbContext
    {
        public BlogDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}