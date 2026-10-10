using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Task_U.Models;
using Task_U.Core;
using Task_U.Core.Entities;
using Task_U.Core.Enemies;
using Task_U.Core.Itens;

namespace Task_U.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<BaseTarefas> BaseTarefas { get; set; }
        public DbSet<SideQuest> SideQuests { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<PersonagemBase> Personagens { get; set; }
        public DbSet<PersonagemInventario> InventarioPersonagens { get; set; }
        public DbSet<Banner> banners { get; set; }

        public DbSet<Loja> loja { get; set; }
        public DbSet<Item> Itens { get; set; }
        public DbSet<ItemInventario> InventarioItens { get; set; }

        public DbSet<InimigoBase> Inimigos { get; set; }

        public DbSet<Config> Config { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moon>();
            modelBuilder.Entity<Barbaro>();
            modelBuilder.Entity<Apostador>();
            modelBuilder.Entity<Grafiteiro>();
            modelBuilder.Entity<Voodo>();
            modelBuilder.Entity<Vampire>();
            modelBuilder.Entity<Star>();
            modelBuilder.Entity<Domina>();
            modelBuilder.Entity<Police>();
            modelBuilder.Entity<SlimeA>();
            modelBuilder.Entity<Lab>();
            modelBuilder.Entity<Exorcist>();
            modelBuilder.Entity<Soul>();
            modelBuilder.Entity<Ladra>();
            modelBuilder.Entity<Cleaner>();
            modelBuilder.Entity<Atacante>();
            modelBuilder.Entity<Bennu>();
            modelBuilder.Entity<Priest>();
            modelBuilder.Entity<Scribe>();
            modelBuilder.Entity<Fire>();
            modelBuilder.Entity<TechGoblin>();
            modelBuilder.Entity<Oni>();
            modelBuilder.Entity<DragaoEgito>();
            modelBuilder.Entity<GreedFollower>();
            modelBuilder.Entity<Mumia>();
            modelBuilder.Entity<DronEscaravelho>();
            modelBuilder.Entity<Fada>();
            modelBuilder.Entity<FenrirF>();
            modelBuilder.Entity<FadaRa>();
            modelBuilder.Entity<Gargula>();
            modelBuilder.Entity<Banshee>();
            modelBuilder.Entity<Aranha>();
            modelBuilder.Entity<Karakasa>();
            modelBuilder.Entity<Kappa>();
            modelBuilder.Entity<oniHeart>();
            modelBuilder.Entity<fadaNucleo>();
            modelBuilder.Entity<aranhaItem>();
            modelBuilder.Entity<grilhaoGreed>();
            modelBuilder.Entity<AdagaDoSacrificio>();
            modelBuilder.Entity<FragmentoEstelar>();
            modelBuilder.Entity<MantoDoSacrificio>();
            modelBuilder.Entity<AdagaDeVidro>();
            modelBuilder.Entity<MoedaDaSorte>();
            modelBuilder.Entity<AnkhBronze>();
            modelBuilder.Entity<presaFernir>();
            modelBuilder.Entity<CapaMesquinha>();
            modelBuilder.Entity<LuvaImpiedosa>();
            modelBuilder.Entity<AmpulhetaAreia>();
            modelBuilder.Entity<resquicioDragao>();

            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=gacha_database.db");
        }
    }
}