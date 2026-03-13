using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Core;
using Todo_Gacha.Data;
using Todo_Gacha.Models;

namespace Todo_Gacha.Services
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
            Console.WriteLine($"Cristayls disponíveis: {user.Crystals}");
            Console.WriteLine($"Pity para Épicos: {user.PityEpic}");
            if (user.PityLeg >= 75)
            {
                Console.WriteLine($"Pity para Lendário: {user.PityLeg}... Está no Soft Pity!");
            }
            else
            {
                Console.WriteLine($"Pity para lendário: {user.PityLeg}");
            }
            Console.WriteLine("--");
            Console.WriteLine("Seus Itens:");

            var itens = context.InventarioItens.ToList();

            foreach (var item in itens)
            {
                var ItemOg = context.Itens.Find(item.ItemId);
                if (ItemOg.Rarity == 1) {
                    Console.ForegroundColor = ConsoleColor.White; 
                } else if (ItemOg.Rarity == 2){
                    Console.ForegroundColor = ConsoleColor.Blue;
                } else if (ItemOg.Rarity == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                } else if (ItemOg.Rarity == 4)
                {
                   Console.ForegroundColor = ConsoleColor.Yellow; 
                }
                Console.WriteLine("--");
                Console.WriteLine($"{ItemOg.Name}, {ItemOg.Desc}");
            }
        }

        public void verTarefas()
        {
            using var context = new AppDbContext();
            foreach (Tarefa tarefa in context.Tarefas.Where(x => x.IsDone == false).ToList())
            {
                if (tarefa.Name.Contains("[SIDE]")) {
                    Console.ForegroundColor = ConsoleColor.DarkYellow; 
                } else {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("--");
                Console.WriteLine($"ID: {tarefa.Id} Nome:{tarefa.Name} Descrição: {tarefa.Desc} Dificuldade: {tarefa.Dif}");
            }
            foreach (Tarefa tarefa in context.Tarefas.Where(x => x.IsDone == true).ToList())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-- Concluídas --");
                Console.WriteLine($"ID: {tarefa.Id} Nome:{tarefa.Name} Descrição: {tarefa.Desc} Dificuldade: {tarefa.Dif}");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void ConcluirTarefa(int TarefaId, Gacha gacha)
        {
            using var context = new AppDbContext();  
            var tarefa = context.Tarefas.Find(TarefaId);
            var user = context.Users.Find(1);

            if (!tarefa.IsDone)
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