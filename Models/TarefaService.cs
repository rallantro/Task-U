using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Data;
using Todo_Gacha.Models;

namespace Todo_Gacha.Models
{
    public class TarefaService
    {
        private AppDbContext context;

        public void verStatus()
        {
            using var context = new AppDbContext();
            var user = context.Users.Find(1);
            Console.WriteLine();
        }

        public void verTarefas()
        {
            using var context = new AppDbContext();
            foreach (Tarefa tarefa in context.Tarefas.Where(x => x.IsDone == false).ToList())
            {
                Console.WriteLine("--");
                Console.WriteLine($"Nome:{tarefa.Name} Descrição: {tarefa.Desc} Dificuldade: {tarefa.Dif}");
            }
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
                    Console.WriteLine($"UUUUUAUUUUU! Uma tarefa épica foi concluída!");
                    Console.WriteLine($"Isso te deu {tarefa.Dif * 3} Crystals! Sinto a sorte se aproximando...");
                }
                else
                {
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