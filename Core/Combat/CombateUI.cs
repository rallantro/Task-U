using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo_Gacha.Core.Combat
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
                string entrada = Console.ReadLine();
                if (int.TryParse(entrada, out escolha) && escolha >= minOpcao && escolha <= maxOpcao)
                    return escolha;
                Console.WriteLine($"Opção inválida. Digite um número entre {minOpcao} e {maxOpcao}.");
            }
        }

        public void ExibirMensagem(string Mensagem, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(Mensagem);
            Console.ResetColor();
        }
    }
}