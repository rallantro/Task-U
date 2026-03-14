using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Todo_Gacha.Models;
using Todo_Gacha.Core;

namespace Todo_Gacha.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<BaseTarefas> BaseTarefas { get; set; }
        public DbSet<SideQuest> SideQuests { get; set; }
        public DbSet<User> Users {get; set;}

        public DbSet<PersonagemBase> Personagens { get; set; }
        public DbSet<PersonagemInventario> InventarioPersonagens  { get; set; }
        public DbSet<Banner> banners  { get; set; }
        public DbSet<Item> Itens  { get; set; }
        public DbSet<ItemInventario> InventarioItens  { get; set; }

        public DbSet<InimigoBase> Inimigos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moon>();
            modelBuilder.Entity<Barbaro>();
            modelBuilder.Entity<Apostador>();
            modelBuilder.Entity<Grafiteiro>();
            modelBuilder.Entity<Voodo>();
            modelBuilder.Entity<Aranha>();

            base.OnModelCreating(modelBuilder);
        }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=gacha_database.db");
        }
    }
}