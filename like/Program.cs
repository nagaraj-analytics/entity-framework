using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demos
{
    public class Program
    {
        private static void Main()
        {
            SetupDatabase();

            using (var db = new BloggingContext())
            {
                var service = new BlogService(db);

                //var l = db.Blogs.Include(p => p.Posts).FirstAsync();
                //l.Wait();

                var blogs = service.SearchBlogs("cat");

               

                var d = db.Posts;
                foreach (var blog in blogs)
                {
                    db.Entry(blog).Collection(b => b.Posts).Query().CountAsync();
                    Console.WriteLine(blog.Url);
                }
            }
        }

        private static void SetupDatabase()
        {
            using (var db = new BloggingContext())
            {
                
                if (db.Database.EnsureCreated())
                {
                    var blog01 = new Blog { Url = "http://sample.com/blogs/fishcat" };
                    Post p1 = new Post { Content = "content 01", Title = "Title 01", Blog = blog01 };
                    Post p2 = new Post { Content = "content 02", Title = "Title 02", Blog = blog01 };

                    db.Posts.Add(p1);
                    db.Posts.Add(p2);
                    //db.Blogs.Add(blog01);
                    db.Blogs.Add(new Blog { Url = "http://sample.com/blogs/catfish" });
                    db.Blogs.Add(new Blog { Url = "http://sample.com/blogs/cats" });
                    db.SaveChanges();
                }
            }
        }
    }

    public class BloggingContext : DbContext
    {
        public BloggingContext()
        {
        }

        public BloggingContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                        @"Server=(localdb)\mssqllocaldb;Database=Demo.Like;Trusted_Connection=True;ConnectRetryCount=0")
                    .UseLoggerFactory(new LoggerFactory().AddConsole((s, l) => l == LogLevel.Information && !s.EndsWith("Connection")));
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
