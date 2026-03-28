using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel.Resolution;
using Task_U.Data;

namespace Task_U.Core.Combat
{
    public class CombateUI
    {
        public void Cabecalho(List<PersonagemBase> equipe, InimigoBase inimigo, int turno)
        {
            Console.Clear();
            string titulo = $"[TURNO {turno:D3}]";
            int larguraTotal = 60; // Defina uma largura fixa para sua caixa
            int bordas = (larguraTotal - titulo.Length) / 2;
            Console.Write("╔" + new string('═', bordas));
            Console.Write(titulo);
            Console.WriteLine(new string('═', larguraTotal - bordas - titulo.Length) + "╗");
            foreach (var personagem in equipe)
            {
                if (personagem.HpAtual <= personagem.HpMax / 5) Console.ForegroundColor = ConsoleColor.Red;
                else if (personagem.HpAtual <= 0) Console.ForegroundColor = ConsoleColor.DarkRed;
                string info = $"{personagem.Name.PadRight(15)} │ HP: {personagem.HpAtual,3} / {personagem.HpMax,3}";
                Console.WriteLine("║" + info.PadRight(larguraTotal) + "║");
                Console.ResetColor();
            }
            Console.WriteLine("╟" + new string('─', larguraTotal) + "╢");
            string infoInimigo = $"INIMIGO: {inimigo.Name.ToUpper().PadRight(10)} │ HP: {inimigo.HpAtual,3} / {inimigo.HpMax,3}";
            Console.WriteLine("║" + infoInimigo.PadRight(larguraTotal) + "║");
            Console.WriteLine("╚" + new string('═', larguraTotal) + "╝");
            Console.ResetColor();
        }

        public void ExibirAcoes(PersonagemBase personagem, bool Atacou, bool UsouHabilidade, bool UsouItem)
        {
            Console.WriteLine($"\n ▶ AGINDO AGORA: {personagem.Name.ToUpper()}");
            Console.WriteLine("  ┌──────────────────────────────────────────┐");
            Console.WriteLine("  │ O que você deseja fazer?                 │");
            Console.WriteLine("  ├──────────────────────────────────────────┤");
            if (Atacou)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  │  [1] Ataque Básico                       │");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("  │  [1] Ataque Básico                       │");
            }
            if (UsouHabilidade)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  │  [2] Habilidade Especial                 │");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("  │  [2] Habilidade Especial                 │");
            }
            if (UsouItem)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  │  [3] Usar Item                           │");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("  │  [3] Usar Item                           │");
            }
            Console.WriteLine("  │  [4] Encerrar Turno                      │");
            Console.WriteLine("  └──────────────────────────────────────────┘");
            Console.Write("  >> Opção: ");
            Console.ResetColor();
        }


        public int EscolhaJogador(int minOpcao, int maxOpcao)
        {
            int escolha;
            while (true)
            {
                string? entrada = Console.ReadLine();
                if (int.TryParse(entrada, out escolha) && escolha >= minOpcao && escolha <= maxOpcao)
                    return escolha;
                Console.WriteLine($"Opção inválida. Digite um número entre {minOpcao} e {maxOpcao}.");
            }
        }

        public void Chamada(InimigoBase inimigo)
        {
            Console.Clear();
            int larguraCard = 70;
            string alerta = "⚠  ALERTA DE COMBATE  ⚠";
            Console.WriteLine(new string(' ', (larguraCard - alerta.Length) / 2) + alerta);
            if (inimigo.Rarity == 2) Console.ForegroundColor = ConsoleColor.Blue;
            else if (inimigo.Rarity == 3) Console.ForegroundColor = ConsoleColor.Magenta;
            else if (inimigo.Rarity == 4) Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔" + new string('═', larguraCard - 2) + "╗");
            string nomeInimigo = $" INIMIGO: {inimigo.Name.ToUpper()} ";
            int espacoNome = (larguraCard - 2 - nomeInimigo.Length) / 2;
            Console.WriteLine("║" + new string(' ', espacoNome) + nomeInimigo + new string(' ', larguraCard - 2 - espacoNome - nomeInimigo.Length) + "║");
            Console.WriteLine("╟" + new string('─', larguraCard - 2) + "╢");
            string[] palavras = inimigo.Desc.Split(' ');
            string linhaAtual = "║  ";

            foreach (var palavra in palavras)
            {
                if ((linhaAtual + palavra).Length > larguraCard - 4)
                {
                    Console.WriteLine(linhaAtual.PadRight(larguraCard - 1) + "║");
                    linhaAtual = "║  ";
                }
                linhaAtual += palavra + " ";
            }
            Console.WriteLine(linhaAtual.PadRight(larguraCard - 1) + "║");

            Console.ResetColor();
            Console.WriteLine("╚" + new string('═', larguraCard - 2) + "╝");
            Console.ResetColor();
            Console.WriteLine("\n[ Pressione qualquer tecla para iniciar a batalha... ]");
            Console.ReadKey();
        }

        public void ExibirMensagem(string Mensagem, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(Mensagem);
            Console.ResetColor();
        }

        public void Vitoria(InimigoBase inimigo, Item? item)
        {
            Console.Clear();
            int larguraVitoria = 60;
            if (inimigo.DeathQuote != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\n  {inimigo.Name} balbucia suas últimas palavras:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"  \"{inimigo.DeathQuote}\"");
                Console.WriteLine(new string('─', larguraVitoria));
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            string vitoriaMsg = "VITÓRIA!!";
            Console.WriteLine("\n" + new string(' ', (larguraVitoria - vitoriaMsg.Length) / 2) + vitoriaMsg);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(new string(' ', (larguraVitoria - 30) / 2) + "==============================");
            if (inimigo.CrystalDrop > 0 || item != null)
            {
                int larguraTotal = 58;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("    RECOMPENSAS DO COMBATE:");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("  ┌──────────────────────────────────────────────────────────┐");
                if (inimigo.CrystalDrop > 0)
                {
                    string texto = $"• {inimigo.CrystalDrop} Cristais";
                    Console.WriteLine($"  │ {texto.PadRight(larguraTotal - 1)}│");
                }

                if (item != null)
                {
                    string texto = $"• {item.Name} (Novo Item!)";
                    Console.WriteLine($"  │ {texto.PadRight(larguraTotal)} │");
                }
                Console.WriteLine("  └──────────────────────────────────────────────────────────┘");
            }
            Console.WriteLine("\n [ Pressione qualquer tecla para voltar ao menu... ]");
            Console.ResetColor();
            Console.ReadKey();
        }

        public void Derrota(InimigoBase inimigo)
        {
            Console.Clear();
            int larguraDerrota = 60;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(new string(' ', (larguraDerrota - 22) / 2) + "▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄");
            Console.ForegroundColor = ConsoleColor.Red;
            string msgDerrota = "█    VOCÊ CAIU...    █";
            Console.WriteLine(new string(' ', (larguraDerrota - msgDerrota.Length) / 2) + msgDerrota);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(new string(' ', (larguraDerrota - 22) / 2) + "▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            string contexto = $"{inimigo.Name} superou sua equipe desta vez.";
            Console.WriteLine(new string(' ', (larguraDerrota - contexto.Length) / 2) + contexto);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  DICAS DE COMBATE:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ┌──────────────────────────────────────────────────────────┐");
            Console.WriteLine("  │ • Verifique se seus itens equipados auxiliam sua equipe. │");
            Console.WriteLine("  │ • Alguns personagens têm vantagem contra esse inimigo.   │");
            Console.WriteLine("  │ • Use a Habilidade Especial no momento certo!            │");
            Console.WriteLine("  └──────────────────────────────────────────────────────────┘");
            Console.ReadKey();
        }

        public void TurnoInimigo(InimigoBase inimigo)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n ▶ AGINDO AGORA: {inimigo.Name.ToUpper()}");
            Console.ResetColor();
        }

        public void AguardarTecla() => Console.ReadKey();
    }
}