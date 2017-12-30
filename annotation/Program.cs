using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotation
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new MyContext();
            
            var p = db.Projects.FirstOrDefault();

            var res = db.Entry(p).Collection(q => q.Tasks).Query().Count();


        }

        public class Person
        {
            public int PersonId { get; set; }
            public string Title { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public DateTime BirthDate { get; set; }
            public List<Project> Projects { get; set; }
            public List<Task> Tasks { get; set; }

        }
        public class Project
        {
            public int ProjectId { get; set; }
            [MaxLength(255)]
            [Required]
            public string Name { get; set; }
            public int PersonId { get; set; }
            [ForeignKey("PersonId")]
            public Person AssignedTo { get; set; }
            public int ManagerId { get; set; }
            [ForeignKey("ManagerId")]
            public Person Manager { get; set; }

            public List<Task> Tasks { get; set; }
        }

        public class Task
        {
            public int TaskId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int PersonId { get; set; }
            [ForeignKey("PersonId")]
            public Person AssignedTo { get; set; }
            public int ProjectId { get; set; }
            [ForeignKey("ProjectId")]
            public Project Project { get; set; }
        }

        //public class ProjectTask
        //{
        //    public int ProjectId { get; set; }
        //    [MaxLength(255)]
        //    [Required]
        //    public string Name { get; set; }
        //    public int ManagerId { get; set; }
        //    [ForeignKey("ManagerId")]
        //    public Person Manager { get; set; }

        //    public List<Task> Tasks { get; set; }
        //}

        public class MyContext : DbContext
        {
            public MyContext() : base("demo.annotation")
            { }

            public DbSet<Person> Persons { get; set; }
            public DbSet<Project> Projects { get; set; }
            public DbSet<Task> Tasks { get; set; }


            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {

                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Task>().HasRequired(t => t.Project).WithMany(
                  p => p.Tasks).HasForeignKey(t => t.ProjectId).WillCascadeOnDelete(false);

                modelBuilder.Entity<Task>().HasRequired(t => t.AssignedTo).WithMany(
                  p => p.Tasks).HasForeignKey(t => t.PersonId).WillCascadeOnDelete(false);

                modelBuilder.Entity<Project>().HasRequired(t => t.Manager).WithMany(
                  p => p.Projects).HasForeignKey(t => t.ManagerId).WillCascadeOnDelete(false);


                modelBuilder.Entity<Task>().HasRequired(p => p.AssignedTo);
            }
        }
    }
}
