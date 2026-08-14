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
                UpdateVersion(oldVersion, context, ActualVersion);
            }
        }

        public void UpdateVersion(Config oldVersion, AppDbContext context, Config Actual)
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("    ATUALIZANDO SEU JOGO...     ");
            Console.WriteLine("=================================\n");

            var version = context.Config.Find(1) ?? baseConfig;
            List<(string name, string version, Action<AppDbContext> action)> supportVersions = new List<(string name, string, Action<AppDbContext>)>{
                {("Os Tempos Caídos","1.4.8", UpdatePatch_1_4_8)},
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
            //Mudanças do Patch
        }
    }
}