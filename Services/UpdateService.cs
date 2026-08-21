using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Task_U.Data;
using Task_U.Models;
using Task_U.Core;
using Task_U.Services;
using Task_U.Core.Entities;
using Task_U.Core.Enemies;
using Task_U.Core.Itens;
using System.Linq.Expressions;

namespace Task_U.Services
{
    public class UpdateService
    {
        private Config baseConfig = new Config { Name = "A Brasa Ardente", Value = "1.6.0" };
        public void Verify(AppDbContext context, Config ActualVersion)
        {
            var oldVersion = context.Config.Find(1);
            if (oldVersion == null)
            {
                oldVersion = baseConfig;
                context.Config.Add(oldVersion);
                context.SaveChanges();
            }

            if (oldVersion.Value != ActualVersion.Value)
            {
                UpdateVersion(oldVersion, context);
            }
        }

        public void UpdateVersion(Config oldVersion, AppDbContext context)
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("    ATUALIZANDO SEU JOGO...     ");
            Console.WriteLine("=================================\n");

            var version = context.Config.Find(1) ?? baseConfig;
            List<(string name, string version, Action<AppDbContext> action)> supportVersions = new List<(string name, string, Action<AppDbContext>)>{
                {("Os Tempos Caídos","1.4.8", UpdatePatch_1_4_8)},
                {("A Brasa Ardente","1.6.0", UpdatePatch_1_6_0)}
            };
            int oldPostion = supportVersions.FindIndex(x => x.version == oldVersion.Value);
            if (oldPostion == -1)
            {
                throw new ArgumentException($"A sua versão ({oldVersion.Value}) não é suportada para atualização. Por favor, reinstale o jogo a partir da versão mínima (1.4.8).");
            }
            for (int i = oldPostion + 1; i < supportVersions.Count; i++)
            {
                var patch = supportVersions[i];
                Console.WriteLine($"-> Aplicando patch {patch.version}: {patch.name}...");
                supportVersions[i].action(context);
                version.Name = patch.name;
                version.Value = patch.version;
                context.SaveChanges();
                Console.WriteLine($"   [✔] Sucesso!\n");
            }
            Console.WriteLine("Atualização concluída! Iniciando o jogo...");
            Thread.Sleep(1500);
        }

        static void UpdatePatch_1_4_8(AppDbContext context)
        {
            // versão base do jogo
        }
        static void UpdatePatch_1_6_0(AppDbContext context)
        {
            var jax = context.Personagens.FirstOrDefault(x => x.Name == "Jax");
            if (jax == null)
            {
                jax = new Grafiteiro
                {
                    Name = "Jax",
                    Atk = 12,
                    HpMax = 300,
                    Mod = 6,
                    Speed = 85,
                    Desc = "Jax é um adolescente de pele clara e cabelos espetados em um tom de vermelho vibrante, combinando com seus olhos cor de âmbar que brilham com travessura. Ele ostenta um estilo Y2K com calças cargo largas, um cinto de utilidades cheio de sprays e uma camisa oversized vermelha, sempre exibindo um sorriso descontraído enquanto desliza com seu skate pelas ruas. \r\n[Habilidade: Muralha de Tinta] Jax usa tinta acumulada para: Explosão (dano massivo), Debuff (cegueira) ou Buff (Ataque e Velocidade para si e aliado).\r\n[Passiva: Camadas de Tinta] No modo de pintura, acumula camadas a cada turno. Se ultrapassar o limite, explode automaticamente no próximo ataque, causando um dano massivo.",
                    SummonQuote = "Minha arte logo vai fazer KABOM!"
                };
                context.Personagens.Add(jax);
            }
            else
            {
                jax.Desc = "Jax é um adolescente de pele clara e cabelos espetados em um tom de vermelho vibrante, combinando com seus olhos cor de âmbar que brilham com travessura. Ele ostenta um estilo Y2K com calças cargo largas, um cinto de utilidades cheio de sprays e uma camisa oversized vermelha, sempre exibindo um sorriso descontraído enquanto desliza com seu skate pelas ruas. \r\n[Habilidade: Muralha de Tinta] Jax usa tinta acumulada para: Explosão (dano massivo), Debuff (cegueira) ou Buff (Ataque e Velocidade para si e aliado).\r\n[Passiva: Camadas de Tinta] No modo de pintura, acumula camadas a cada turno. Se ultrapassar o limite, explode automaticamente no próximo ataque, causando um dano massivo.";
                jax.Atk = 12;
                jax.HpMax = 300;
                jax.Mod = 6;
                jax.Speed = 105;
            }
            context.SaveChanges();
        }
    }
}