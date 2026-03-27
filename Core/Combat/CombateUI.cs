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
            Console.WriteLine($"================== TURNO {turno} ==================");
            foreach (var personagem in equipe)
            {
                if (personagem.HpAtual <= 0) Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($" {personagem.Name} | HP: {personagem.HpAtual}");
                Console.ResetColor();
            }
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine($" INIMIGO: {inimigo.Name.ToUpper()} | HP: {inimigo.HpAtual}/{inimigo.HpMax}");
            Console.WriteLine("===================================================");
        }

        public void ExibirAcoes(PersonagemBase personagem, bool Atacou, bool UsouHabilidade, bool UsouItem)
        {
            Console.WriteLine($"\n --- TURNO DE {personagem.Name} ---");
            Console.WriteLine($"---- // ----");
            Console.WriteLine($"O que você deseja fazer?");
            if (Atacou)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("1 - Ataque Básico");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("1 - Ataque Básico");
            }
            if (UsouHabilidade)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("2 - Habilidade Especial");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("2 - Habilidade Especial");
            }
            if (UsouItem)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("3 - Item");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("3 - Item");
            }
            Console.WriteLine("4 - Encerrar Turno");
            Console.WriteLine($"---- // ----");
            Console.ResetColor();
        }


        public int EscolhaJogador(int minOpcao, int maxOpcao)
        {
            int escolha;
            while (true)
            {
                string ? entrada = Console.ReadLine();
                if (int.TryParse(entrada, out escolha) && escolha >= minOpcao && escolha <= maxOpcao)
                    return escolha;
                Console.WriteLine($"Opção inválida. Digite um número entre {minOpcao} e {maxOpcao}.");
            }
        }

        public void Chamada(InimigoBase inimigo)
        {
            Console.Clear();
            Console.WriteLine("===== ATENÇÃO PARA O COMBATE! =====");
            if (inimigo.Rarity == 2) Console.ForegroundColor = ConsoleColor.Blue;  
            else if (inimigo.Rarity == 3) Console.ForegroundColor = ConsoleColor.Magenta; 
            else if (inimigo.Rarity == 4) Console.ForegroundColor = ConsoleColor.Yellow;  
            Console.WriteLine($"Seu inimigo será {inimigo.Name}");
            Console.WriteLine("===================================");
            Console.WriteLine(inimigo.Desc);
            Console.WriteLine("===================================");
            Console.ResetColor();
            Console.ReadKey();
        }

        public void ExibirMensagem(string Mensagem, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(Mensagem);
            Console.ResetColor();
        }

        public void Vitoria(InimigoBase inimigo, Item ? item)
        {
            if (inimigo.DeathQuote != null)
                {
                   Console.WriteLine($"{inimigo.Name} grunhe antes de morrer:");
                   Console.WriteLine($"'{inimigo.DeathQuote}'"); 
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("--");
                Console.WriteLine($"Parabéns Você derrotou {inimigo.Name}");
                Console.WriteLine("--");
                if (inimigo.CrystalDrop > 0)
                {
                    Console.WriteLine($"Você recebeu {inimigo.CrystalDrop} Crystals!");
                }
                if (item != null)
                {
                    Console.WriteLine($"Você recebeu {item.Name}!");
                }
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ResetColor();
                Console.ReadKey();
        }

        public void Derrota(InimigoBase inimigo)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"==== DERROTA ====");
            Console.WriteLine($"{inimigo.Name} foi forte demais para você.");
            Console.WriteLine($"====    //   ====");
            Console.ResetColor();
            Console.WriteLine($"Dica: Tente trocar os itens e ou os personagens!");
            Console.ReadKey();
        }

        public void AguardarTecla() => Console.ReadKey();
    }
}