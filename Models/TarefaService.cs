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

        public void AtualizarTarefas()
        {
            using var context = new AppDbContext();
            var hoje = DateTime.Now.DayOfWeek;

            context.Tarefas.RemoveRange(context.Tarefas);

            if (hoje == DayOfWeek.Monday) 
            {
                context.Tarefas.Add(new Tarefa { Name = "Vídeo-Aula: PT/Mat", Desc = "30min de Português e 30min de Matemática", Dif = 3, IsDone = false });
                context.Tarefas.Add(new Tarefa { Name = "Exercícios: Português", Desc = "Bloco de exercícios de Linguagens", Dif = 4, IsDone = false });
                context.Tarefas.Add(new Tarefa { Name = "Estudo Pesado: Álgebra", Desc = "Álgebra e Funções (Foco total)", Dif = 5, IsDone = false });
                context.Tarefas.Add(new Tarefa { Name = "Revisão Noturna", Desc = "Revisão do conteúdo do dia", Dif = 2, IsDone = false });
            }
            else if (hoje == DayOfWeek.Tuesday) 
            {
                context.Tarefas.Add(new Tarefa { Name = "Vídeo-Aula: His/Fis", Desc = "30min de História e 30min de Física", Dif = 3, IsDone = false });
                context.Tarefas.Add(new Tarefa { Name = "Exercícios: História", Desc = "Prática de Humanas", Dif = 4, IsDone = false });
                context.Tarefas.Add(new Tarefa { Name = "Estudo Pesado: Física I", Desc = "Mecânica e Leis de Newton", Dif = 5, IsDone = false });
            }
            else if (hoje == DayOfWeek.Wednesday) 
            {

                context.Tarefas.Add(new Tarefa { Name = "Vídeo-Aula: Bio/Mat", Desc = "30min de Biologia e 30min de Matemática", Dif = 3, IsDone = false });
                context.Tarefas.Add(new Tarefa { Name = "Exercícios: Citologia/Eco", Desc = "Biologia celular e ecologia", Dif = 4, IsDone = false });
                context.Tarefas.Add(new Tarefa { Name = "Estudo Pesado: Geometria", Desc = "Geometria Plana/Espacial", Dif = 5, IsDone = false });
            }
            else if (hoje == DayOfWeek.Thursday) 
            {
                context.Tarefas.Add(new Tarefa { Name = "Vídeo-Aula: Geo/Qui", Desc = "30min de Geografia e 30min de Química", Dif = 3, IsDone = false });
                context.Tarefas.Add(new Tarefa { Name = "Exercícios: Geografia", Desc = "Prática de Geografia Geral/Brasil", Dif = 4, IsDone = false });
                context.Tarefas.Add(new Tarefa { Name = "Estudo Pesado: Física I", Desc = "Reforço em Mecânica", Dif = 5, IsDone = false });
            }
            else if (hoje == DayOfWeek.Friday) 
            {
                context.Tarefas.Add(new Tarefa { Name = "Redação: Criação", Desc = "Desenvolvimento de tema para vestibular", Dif = 6, IsDone = false }); // DIF 6 ativa LuckEvent!
                context.Tarefas.Add(new Tarefa { Name = "Estudo Pesado: Física II", Desc = "Eletricidade e Ondas", Dif = 5, IsDone = false });
                context.Tarefas.Add(new Tarefa { Name = "Química/Biologia", Desc = "Bloco de Natureza vespertino", Dif = 4, IsDone = false });
            }
            else if (hoje == DayOfWeek.Saturday) 
            {
                context.Tarefas.Add(new Tarefa { Name = "SIMULADO GERAL", Desc = "Execução do simulado cronometrado", Dif = 10, IsDone = false }); // Muitos cristais aqui!
                context.Tarefas.Add(new Tarefa { Name = "Revisão do Simulado", Desc = "Análise de erros e acertos", Dif = 5, IsDone = false });
                context.Tarefas.Add(new Tarefa { Name = "Filosofia/Sociologia", Desc = "Leitura de Humanas", Dif = 3, IsDone = false });
            }
        }

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
                Console.WriteLine($"ID: {tarefa.Id} Nome:{tarefa.Name} Descrição: {tarefa.Desc} Dificuldade: {tarefa.Dif}");
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