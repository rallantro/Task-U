#nullable disable
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using Task_U.Models;
using Task_U.Core;
using Task_U.Services;
using Task_U.Data;
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

Console.OutputEncoding = System.Text.Encoding.UTF8;

while (MenuShow)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(@"
   _____         _           _   _ 
  |_   _|       | |         | | | |
    | | __ _ ___| | ________| | | |
    | |/ _` / __| |/ /______| | | |
    | | (_| \__ \   <       | |_| |
    \_/\__,_|___/_|\_\       \___/ 
");

    Console.ForegroundColor = ConsoleColor.DarkBlue;
    Console.WriteLine(" ╔══════════════════════════════════════════╗");
    Console.WriteLine(" ║            PAINEL DE CONTROLE            ║");
    Console.WriteLine(" ╠══════════════════════════════════════════╣");
    Console.WriteLine(" ║  [1] Ver Status                          ║");
    Console.WriteLine(" ║  [2] Ver Tarefas                         ║");
    Console.WriteLine(" ║  [3] Concluir Tarefas                    ║");
    Console.WriteLine(" ║  [4] Desejar (Gacha)                     ║");
    Console.WriteLine(" ║  [5] Iniciar Combate                     ║");
    Console.WriteLine(" ║  [6] Salvar e Sair                       ║");
    Console.WriteLine(" ╚══════════════════════════════════════════╝");
    Console.ResetColor();


    switch (Console.ReadLine())
    {
        case "1":
            service.verStatus();
            Console.WriteLine("\n >> O que deseja acessar?");
            Console.WriteLine(" [1] Ver Personagens  [2] Ver Itens  [Qualquer tecla] Voltar");
            Console.Write("\n >> Escolha: ");
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
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n ╔══════════════════════════════════════════╗");
            Console.WriteLine(" ║          RELATAR CONCLUSÃO               ║");
            Console.WriteLine(" ╚══════════════════════════════════════════╝");
            Console.ResetColor();
            Console.Write("\n >> Digite o ID da Tarefa Realizada: ");
            var EscolhaId = 0;
            if (int.TryParse(Console.ReadLine(), out EscolhaId))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("    Validando dados da missão...");
                Thread.Sleep(500);
                service.ConcluirTarefa(EscolhaId, gacha);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n [!] ERRO: ID Inválido. Digite apenas números.");
            }
            context.Entry(user).Reload();
            Console.ReadKey();
            break;

        case "4":
            context.Entry(user).Reload();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ╔══════════════════════════════════════════════════════════╗");
            string titulo = "EVENTO DA SEMANA";
            int largura = 60;
            int espacos = (largura - titulo.Length) / 2;
            Console.WriteLine(" ║" + new string(' ', espacos) + titulo + new string(' ', largura - espacos - titulo.Length - 2) + "║");
            Console.WriteLine(" ╠══════════════════════════════════════════════════════════╣");
            Console.ForegroundColor = ConsoleColor.Yellow;
            string textoLendario = $" ║  LENDÁRIO: {banner.rateUpLeg.Name.PadRight(20)} [RATE UP!]";
            Console.WriteLine(textoLendario.PadRight(60) + "║");
            Console.ForegroundColor = ConsoleColor.Magenta;
            string textoEpico = $" ║  ÉPICO:    {banner.rateUpEpic.Name.PadRight(20)} [RATE UP!]";
            Console.WriteLine(textoEpico.PadRight(60) + "║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ╟──────────────────────────────────────────────────────────╢");
            string dataUpdate = user.LastBannerUpdate.AddDays(7).ToString("dd/MM/yyyy");
            Console.WriteLine($" ║  Atualização em: {dataUpdate.PadRight(40)}║");
            Console.WriteLine($" ║  Seus Cristais:  {user.Crystals.ToString().PadRight(40)}║");
            Console.WriteLine(" ╚══════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n >> Custo: 10 Cristais por Desejo");
            Console.Write(" >> Quantos desejos deseja realizar? (ou letras para sair): ");

            var num = 0;
            var sucesso = int.TryParse(Console.ReadLine(), out num);
            var x = 0;
            if (sucesso && user.Crystals < num * 10)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n [!] Erro: Cristais insuficientes para esta quantidade.");
                Console.ResetColor();
                Console.WriteLine(" >> Pressione qualquer tecla para voltar...");
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
            Console.WriteLine("\n >> Invocação finalizada.");
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
            if (user.Slot1_PersonagemAtivo != null) equipe.Add(user.Slot1_PersonagemAtivo);
            if (user.Slot2_PersonagemAtivo != null) equipe.Add(user.Slot2_PersonagemAtivo);
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




