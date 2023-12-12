namespace WebApplication3.Models
{
    using Microsoft.EntityFrameworkCore;

    public class ModelContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public ModelContext(DbContextOptions<ModelContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasKey(q => q.Id); // первичный ключ

            modelBuilder.Entity<Question>()
                .Property(q => q.Matter)
                .IsRequired(); // поле Matter обязательно

            modelBuilder.Entity<Question>()
                .Property(q => q.Answer)
                .IsRequired(); // поле Answer обязательно

            // Пример других конфигураций
            modelBuilder.Entity<Question>()
                .Property(q => q.Matter)
                .HasMaxLength(255); // Указываем максимальную длину для Matter

            // Пример других конфигураций
            modelBuilder.Entity<Question>()
                .Property(q => q.Answer)
                .HasMaxLength(255); // Указываем максимальную длину для Matter


            base.OnModelCreating(modelBuilder);
        }
    }
}
