// See https://aka.ms/new-console-template for more information
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.VisualBasic;
using Todo_Gacha.Models;
using Todo_Gacha.Core;
using Todo_Gacha.Services;
using Todo_Gacha.Data;
using System.Diagnostics;

Gacha gacha = new Gacha();
BannerService banner = new BannerService();
TarefaService service = new TarefaService();
CombatService combat = new CombatService();
InventarioServices inventario = new InventarioServices();
using var context = new AppDbContext();

var user = context.Users.Find(1);
banner.AtualizarBanner(context);
service.AtualizarTarefas();

CreateService create = new CreateService();


bool MenuShow = true;

while (MenuShow)
{
    Console.Clear();
    Console.WriteLine("===============================");
    Console.WriteLine("MENU PRINCIPAL");
    Console.WriteLine("===============================");
    Console.WriteLine(" ");
    Console.WriteLine("Digite a sua opção:");
    Console.WriteLine("1 - Ver Status");
    Console.WriteLine("2 - Ver Tarefas");
    Console.WriteLine("3 - Concluir Tarefas");
    Console.WriteLine("4 - Desejar");
    Console.WriteLine("5 - Combate");
    Console.WriteLine("6 - Encerrar");


    switch (Console.ReadLine())
    {
        case "1" :
            service.verStatus();
            inventario.VerPersonagens(context);
            Console.ReadLine();
        break;

        case "2":
            service.verTarefas();
            Console.ReadLine();
        break;

        case "3":
            Console.WriteLine("--");
            Console.WriteLine("Digite o ID da Tarefa Realizada:");
            service.ConcluirTarefa(int.Parse(Console.ReadLine()), gacha);
            Console.ReadLine();
        break;

        case "4":
            Console.Clear();
            Console.WriteLine("--- EVENTO DA SEMANA ---");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Lendário: {banner.rateUpLeg.Name} (Chance Aumentada!!)");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Épico: {banner.rateUpEpic.Name} (Chance Aumentada!!)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-------------------------");
            Console.WriteLine($"O banner será atualizado no dia: {user.LastBannerUpdate.AddDays(7)}");
            Console.WriteLine("--");
            Console.WriteLine($"Quantos desejos deseja fazer? (1 desejo = 10 Crystals) - {user.Crystals} Crystals Restantes");
            var num = int.Parse(Console.ReadLine());
            var x = 0;
            if (user.Crystals < num * 10)
            {
                Console.WriteLine("Você não possui Crystals suficientes!");
                Console.WriteLine("--");
                Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
                break;
            }
            while (x < num)
            {
              gacha.Pull(banner);
              x++;  
            }
            context.Entry(user).Reload();
            Console.WriteLine("--");
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        break;

        case "5":
            combat.Combate(context.Inimigos.Find(1), context.Personagens.Find(3));
            context.Entry(user).Reload();
        break;

        case "6":
            Environment.Exit(0);
        break;

        default:
            Console.WriteLine("Por favor digite apenas uma das opções acima!");
            Console.ReadLine();
        break;
    }
}




