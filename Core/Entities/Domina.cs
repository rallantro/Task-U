using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Todo_Gacha.Core.Entities
{
    public class Domina : PersonagemBase
    {

        public override void tomarDano(string inimigo, int dano)
        {
            int danoTotal = Math.Max(0, dano - Shield *2);
            int danoShield = Math.Min(Shield*2, dano);
            Shield -= danoShield/2;
            HpAtual -= danoTotal;
            if (danoShield > 0 && danoTotal == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"> {Name} bloqueou completamente o ataque de {inimigo} com seu escudo!");
                Console.WriteLine($"> {Name}: Tenta com mais força, fofo...");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"{inimigo} atacou {Name} e causou {danoTotal} de dano!");
                Console.WriteLine($"> {Name}: Você não sabe com quem tá mexendo...");
            }
        }

        public override void Habilidade()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"> {Name}: Arg... Essa tecnologia vale milhões, tome cuidado.");
            Console.ResetColor();
            if (aliado != null)
            {
                Console.WriteLine($"Para quem {Name} deve dar o escudo?");
                Console.WriteLine($"1 - {Name} | 2 - {aliado.Name}");
                bool escolheu = false;
                while (!escolheu)
                {
                    switch (Console.ReadLine())
                    {
                        case "1":
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.WriteLine($"> [HABILIDADE] {Name} recebe {(HpMax / 4) + (ModTotal() * 2)} de escudo!");
                            Console.WriteLine($"> {Name}: Ah! Não toca em mim!");
                            Console.ResetColor();
                            Shield = (HpMax / 4) + ModTotal() * 2;
                            escolheu = true;
                        break;
                        case "2":
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.WriteLine($"> [HABILIDADE] {aliado.Name} recebe {(HpMax / 4) + (ModTotal() * 2)} de escudo!");
                            Console.WriteLine($"> {Name}: Não despreze um presente como este.");
                            Console.ResetColor();
                            aliado.Shield = (HpMax / 4) + ModTotal() * 2;
                            escolheu = true;
                        break;
                        default:
                            Console.WriteLine($"> {Name}: A escolha não é muito difícil...");
                        break;
                    }
                }
                
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"> [HABILIDADE] {Name} recebe {(HpMax / 3) + (ModTotal() * 2)} de escudo!");
                Console.WriteLine($"> {Name}: Arg... Acabei de fazer a unha... Tenta não manchar, queridinho...");
                Console.ResetColor();
                Shield = (HpMax / 3) + (ModTotal() * 2);
            }
            
            
        }

        public override void Passiva()
        {
            if (aliado != null && aliado.Shield > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"> [PASSIVA] {Name} recebe {HpMax/ModTotal()} de dano adicional em seus ataques!");
                Console.WriteLine($"> {Name}: Se você ganha alguma coisa, eu também vou ganhar.");
                Console.ResetColor();
                BuffAtk += HpMax/ModTotal();
            }
            else if (aliado != null && Shield > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"> [PASSIVA] {aliado.Name} recebe {HpMax/ModTotal()} de dano adicional em seus ataques!");
                Console.WriteLine($"> {Name}: Considere isso um investimento.");
                Console.ResetColor();
                aliado.BuffAtk += HpMax/ModTotal();
            }
            else
            {
                BuffAtk = 0;
                if (aliado != null) aliado.BuffAtk = 0;
            }
        }
        
    }
}