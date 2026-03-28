using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Core;
using Task_U.Data;
using Task_U.Models;

namespace Task_U.Services
{
    public class TarefaService
    {

        public void AtualizarTarefas()
        {
            using var context = new AppDbContext();
            var hoje = DateTime.Now.DayOfWeek;
            var user = context.Users.Find(1);

            if (user.lastLogin.Date < DateTime.Now.Date)
            {
                var TarefasDoDia = context.BaseTarefas.Where(t => (DayOfWeek)t.DiaSemana == hoje).ToList();
                context.Tarefas.RemoveRange(context.Tarefas);

                foreach (var tf in TarefasDoDia)
                {
                    context.Tarefas.Add(new Tarefa
                    {
                        Name = tf.Name,
                        Desc = tf.Desc,
                        Dif = tf.Dif
                    });
                }

                Random rand = new Random();
                int quantidade = rand.Next(2, 5);
                var sideSorteada = context.SideQuests.ToList().OrderBy(s => Guid.NewGuid()).Take(quantidade);
                foreach (var side in sideSorteada)
                {
                    context.Tarefas.Add(new Tarefa
                    {
                        Name = "[SIDE] " + side.Name,
                        Desc = side.Desc,
                        Dif = side.Dif
                    });
                }

                user.lastLogin = DateTime.Now;
                context.Users.Update(user);
                context.SaveChanges();
            }

        }

        public void verStatus()
        {
            using var context = new AppDbContext();
            var user = context.Users.Find(1);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine(" ║                    STATUS DO VIAJANTE                    ║");
            Console.WriteLine(" ╠══════════════════════════════════════════════════════════╣");
            Console.ForegroundColor = ConsoleColor.White;
            string cristais = $" Cristais: {user.Crystals}";
            Console.WriteLine(" ║" + cristais.PadRight(58) + "║");
            Console.ForegroundColor = ConsoleColor.Magenta;
            string pSR = $" Pity [SR]: {user.PityEpic} / 10";
            Console.WriteLine(" ║" + pSR.PadRight(58) + "║");
            if (user.PityLeg >= 75)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                string pSSR = $" Pity [SSR]: {user.PityLeg} / 100 [SOFT PITY ATIVO!]";
                Console.WriteLine(" ║" + pSSR.PadRight(58) + "║");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                string pSSR = $" Pity [SSR]: {user.PityLeg} / 100";
                Console.WriteLine(" ║" + pSSR.PadRight(58) + "║");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public void verTarefas()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine(" ║                       TAREFAS DO DIA                     ║");
            Console.WriteLine(" ╚══════════════════════════════════════════════════════════╝");
            using var context = new AppDbContext();
            foreach (Tarefa tarefa in context.Tarefas.Where(x => x.IsDone == false).ToList())
            {
                bool isSide = tarefa.Name.Contains("[SIDE]");
                if (tarefa.Name.Contains("[SIDE]"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    isSide = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    isSide = false;
                }
                string tipo = isSide ? "[SIDE]" : "[PRINCIPAL]";
                Console.WriteLine($"\n  ID #{tarefa.Id:D3} - {tipo}");
                Console.WriteLine($"  Título: {tarefa.Name.Replace("[SIDE]", "").Trim()}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"  ├─ Desc: {tarefa.Desc}");
                Console.WriteLine($"  └─ Rank: {tarefa.Dif} ");
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n ─────────────────── MISSÕES FINALIZADAS ───────────────────");
            foreach (Tarefa tarefa in context.Tarefas.Where(x => x.IsDone == true).ToList())
            {
                Console.WriteLine($"  [✔] {tarefa.Name} (Dificuladade: {tarefa.Dif})");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" [Pressione qualque tecla para voltar...] ");
        }

        public void ConcluirTarefa(int TarefaId, Gacha gacha)
        {
            using var context = new AppDbContext();
            if (context.Tarefas.FirstOrDefault(x => x.Id == TarefaId) == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Tarefa inválida!");
                Console.ResetColor();
                return;
            }
            var tarefa = context.Tarefas.Find(TarefaId);
            var user = context.Users.Find(1);

            if (tarefa != null && !tarefa.IsDone)
            {
                if (tarefa.Dif >= 6)
                {
                    user.Crystals += tarefa.Dif * 3;
                    gacha.luckEvent = true;
                    Console.Clear();
                    Console.WriteLine("...");
                    Thread.Sleep(1500);
                    Console.BackgroundColor = ConsoleColor.White; Console.Clear(); Thread.Sleep(50);
                    Console.BackgroundColor = ConsoleColor.Green; Console.Clear(); Thread.Sleep(50);
                    Console.BackgroundColor = ConsoleColor.Black; Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" ╔══════════════════════════════════════════════════════════╗");
                    Console.WriteLine(" ║       TAREFA ÉPICA CONCLUÍDA!                            ║");
                    Console.WriteLine(" ╠══════════════════════════════════════════════════════════╣");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($" ║ Recompensa: {tarefa.Dif * 3,-4} Crystals                            ║");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(" ║ BÔNUS: Sinto a sorte fluindo... (Luck Event Ativo!)      ║");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" ╚══════════════════════════════════════════════════════════╝");
                }
                else
                {
                    Console.WriteLine("\n...");
                    Thread.Sleep(800);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($" [✔] Bom trabalho! +{tarefa.Dif * 2} Crystals adicionados à sua conta.");
                    Console.ResetColor();
                    user.Crystals += tarefa.Dif * 2;
                }

                context.Users.Update(user);

                tarefa.IsDone = true;
                context.Tarefas.Update(tarefa);
                context.SaveChanges();
            }


        }
    }
}