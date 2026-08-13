using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core.Itens
{
    public class resquicioDragao : Item
    {
        private Random rand = new Random();
        public override void Effect(PersonagemBase personagem)
        {
            if (personagem.inimigoAlvo != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"> [RESQUÍCIO DO CAOS] Fragmentos de realidade corrompida orbitam {personagem.Name}!");
                Console.WriteLine($"> O caos se intensifica conforme {personagem.inimigoAlvo.Name} sangra!");
                Console.ResetColor();
                double vidaPerdida = 1 - (double)personagem.inimigoAlvo.HpAtual / personagem.inimigoAlvo.HpMax;
                int dano = (int)Math.Ceiling(personagem.inimigoAlvo.HpMax * 0.04 * vidaPerdida);
                if (personagem.inimigoAlvo.HpAtual < personagem.inimigoAlvo.HpMax * 0.1)
                {

                    int chance = rand.Next(1, 101);
                    if (chance < 20)
                    {
                        string[] frames = { "---", "- -", " x ", " X ", "ERR", "010" };
                        Console.ForegroundColor = ConsoleColor.Magenta;

                        for (int i = 0; i < frames.Length; i++)
                        {
                            Console.Write($"\r> [INTERFERÊNCIA ABISSAL]: {frames[i]}");
                            Thread.Sleep(80); 
                        }
                        Console.WriteLine();


                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"> [O FIM CHEGOU!] Os resquícios do caos de {personagem.Name} destroem {personagem.inimigoAlvo.Name}!");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"> Uma voz... Um sussuro... Horripilante... Consome a mente de {personagem.Name}.");
                        Console.ResetColor();
                        personagem.inimigoAlvo.HpAtual = 1;
                        personagem.HpAtual = Math.Min(personagem.HpAtual, (int)Math.Ceiling(personagem.HpMax * 0.05));
                    }

                }
                personagem.BuffAtk += dano;
                Console.WriteLine($"> {personagem.Name} extrai {dano} de força do caos!");
            }
        }
    }
}