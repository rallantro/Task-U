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
                    context.Tarefas.Add(new Tarefa { 
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
                    context.Tarefas.Add(new Tarefa { 
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
            Console.WriteLine("Seus Status atuais são:");
            Console.WriteLine($"Cristais disponíveis: {user.Crystals}");
            Console.WriteLine($"Pity para Épicos: {user.PityEpic}");
            if (user.PityLeg >= 75)
            {
                Console.WriteLine($"Pity para Lendário: {user.PityLeg}... Está no Soft Pity!");
            }
            else
            {
                Console.WriteLine($"Pity para lendário: {user.PityLeg}");
            }
        }

        public void verTarefas()
        {
            Console.Clear();
            Console.WriteLine ("==== TAREFAS DO DIA ====");
            using var context = new AppDbContext();
            foreach (Tarefa tarefa in context.Tarefas.Where(x => x.IsDone == false).ToList())
            {
                if (tarefa.Name.Contains("[SIDE]")) {
                    Console.ForegroundColor = ConsoleColor.DarkYellow; 
                } else {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("--");
                Console.WriteLine($"ID: {tarefa.Id} Nome:{tarefa.Name} | Descrição: {tarefa.Desc} | Dificuldade: {tarefa.Dif}");
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine("-- Concluídas --");
            foreach (Tarefa tarefa in context.Tarefas.Where(x => x.IsDone == true).ToList())
            {    
                Console.WriteLine($"ID: {tarefa.Id} Nome:{tarefa.Name} | Descrição: {tarefa.Desc} | Dificuldade: {tarefa.Dif}");
                Console.WriteLine("--");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Pressione qualque tecla para voltar...");
        }

        public void ConcluirTarefa(int TarefaId, Gacha gacha)
        {
            using var context = new AppDbContext();  
            if(context.Tarefas.FirstOrDefault(x => x.Id ==TarefaId) == null)
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
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine($"UUUUUAUUUUU! Uma tarefa épica foi concluída!");
                    Console.WriteLine($"Isso te deu {tarefa.Dif * 3} Crystals! Sinto a sorte se aproximando...");
                }
                else
                {
                    Console.WriteLine("...");
                    Thread.Sleep(1500);
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    user.Crystals += tarefa.Dif * 2;
                    Console.WriteLine($"Parabéns você ganhou {tarefa.Dif * 2} Crystals! Continue assim!");
                }

                context.Users.Update(user);
                
                tarefa.IsDone = true;
                context.Tarefas.Update(tarefa);
                context.SaveChanges();
            }

            
        } 
    }
}