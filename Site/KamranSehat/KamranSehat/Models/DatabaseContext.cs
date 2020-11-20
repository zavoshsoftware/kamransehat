using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Models
{
   public class DatabaseContext:DbContext
    {
        static DatabaseContext()
        {
             System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Models.ProductGroup> ProductGroups { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogGroup> BlogGroups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public System.Data.Entity.DbSet<Models.Province> Provinces { get; set; }
        public System.Data.Entity.DbSet<Models.City> Cities { get; set; }
        public System.Data.Entity.DbSet<Models.DiscountCode> DiscountCodes { get; set; }
        public DbSet<ZarinpallAuthority> ZarinpallAuthorities { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }

        public DbSet<Radio> Radios { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<CustomerGroup> CustomerGroups { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ServiceGroup> ServiceGroups { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }

        public System.Data.Entity.DbSet<Models.Course> Courses { get; set; }

        public System.Data.Entity.DbSet<Models.CourseDetail> CourseDetails { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Question> Questions { get; set; }

        public System.Data.Entity.DbSet<Models.UserCourseDetail> UserCourseDetails { get; set; }
    }
}
