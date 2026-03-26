#nullable disable
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
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
AdventureService adventure = new AdventureService();
using var context = new AppDbContext();

var user = context.Users.Include(u => u.Slot1_PersonagemAtivo).Include(u => u.Slot2_PersonagemAtivo).Include(u => u.Slot1_ItemAtivo).FirstOrDefault(u => u.Id == 1);
//adventure.AtualizarInimigo(context);
banner.AtualizarBanner(context);
service.AtualizarTarefas();
adventure.AtualizarInimigo(context);

CreateService create = new CreateService();
//create.CreateCharacter(context);





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
            Console.WriteLine("--");
            Console.WriteLine("Deseja ver seu inventário?");
            Console.WriteLine("1 - Ver Personagens | 2 - Ver Itens | Digite qualquer coisa para voltar");
            var escolha = Console.ReadLine();
            if (escolha == "1")
            {
                Console.Clear();
                inventario.VerPersonagens(context, user);
            }
            else if (escolha == "2")
            {
                Console.Clear();
                inventario.VerItens(context, user);
            }
        break;

        case "2":
            service.verTarefas();
            Console.ReadLine();
        break;

        case "3":
            context.Entry(user).Reload();
            Console.WriteLine("--");
            Console.WriteLine("Digite o ID da Tarefa Realizada:");
            var EscolhaId = 0;
            if(int.TryParse(Console.ReadLine(), out EscolhaId))
            {
                service.ConcluirTarefa(EscolhaId, gacha);
            }
            else
            {
                Console.WriteLine("Comando Inválido!");
            }
            context.Entry(user).Reload();
            Console.ReadKey();
        break;

        case "4":
            context.Entry(user).Reload();
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
            Console.WriteLine($"Quantos desejos deseja fazer? (1 desejo = 10 Cristais) - {user.Crystals} Cristais Restantes");
            Console.WriteLine("Digite qualquer coisa que não seja um número para voltar ao menu...");
            var num = 0;
            var sucesso = int.TryParse(Console.ReadLine(), out num);
            var x = 0;
            if (sucesso && user.Crystals < num * 10)
            {
                Console.WriteLine("Você não possui Cristais suficientes!");
                Console.WriteLine("--");
                Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
                break;
            }
            else if (!sucesso)
            {
                break;
            }
            while (sucesso && x < num)
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
            var equipe = new List<PersonagemBase>();
            var inimigo = context.Inimigos.FirstOrDefault(x => x.Id == user.InimigoId);
            if (user.Slot1_PersonagemAtivo == null && user.Slot2_PersonagemAtivo == null)
            {
                Console.WriteLine("Você não possui personagens equipados!");
                Console.ReadKey();
                break;
            }
            if(user.Slot1_PersonagemAtivo != null) equipe.Add(user.Slot1_PersonagemAtivo);
            if(user.Slot2_PersonagemAtivo != null) equipe.Add(user.Slot2_PersonagemAtivo);
            combat.Combate(inimigo, equipe, context, adventure);
            context.Entry(user).Reload();
        break;

        case "6":
            Environment.Exit(0);
        break;

        default:
            Console.WriteLine("Por favor digite apenas uma das opções acima!");
            Console.ReadKey();
        break;
    }
}




