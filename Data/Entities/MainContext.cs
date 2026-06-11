using Microsoft.EntityFrameworkCore;

namespace Talkable.Data.Entities
{
    public class MainContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public MainContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public virtual DbSet<User> Tb_Users { get; set; }
        public virtual DbSet<Courses> Tb_Courses { get; set; }
        public virtual DbSet<CourseFeedback> Tb_CourseFeedback { get; set; }
        public virtual DbSet<UserCourses> Tb_UserCourses { get; set; } // done
        public virtual DbSet<Signs> Tb_Signs { get; set; }
        public virtual DbSet<OTP> Tb_OTP { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Mg13
                optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:GharebConnection"]);

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCourses>(entity =>
            {
                entity.HasOne(a=>a.Tbuser).WithMany(a=>a.User_Courses).HasForeignKey(a=>a.User_Id);
                entity.HasOne(a => a.Tbcourse).WithMany(a => a.User_courses).HasForeignKey(a => a.Course_Id);
            });

            modelBuilder.Entity<CourseFeedback>(entity =>
            {
                entity.HasOne(a => a.user).WithMany(a => a.User_Feedback).HasForeignKey(a => a.User_Id);
                entity.HasOne(a => a.Courses).WithMany(a => a.User_Feedback).HasForeignKey(a => a.Course_Id);
            });
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveMaxLength(400);
            configurationBuilder.Properties<decimal>().HaveColumnType("decimal(8,2)");
            configurationBuilder.Properties<DateTime>().HaveColumnType("DateTime");
        }
    }
}

