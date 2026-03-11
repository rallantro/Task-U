using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Todo_Gacha.Models;

namespace Todo_Gacha.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<User> Users {get; set;}


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=gacha_database.db");
        }
    }
}