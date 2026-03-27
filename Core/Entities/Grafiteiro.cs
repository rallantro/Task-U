using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Services;
using Task_U.Models;
using Task_U.Data;

namespace Task_U.Core
{
    public class Grafiteiro : PersonagemBase
    {
        private int BonusDMG;
        private int QuantDmg;
        private bool Paint;

        public override int Damage()
        {
            int dano = AtkTotal();
            if (!Paint && QuantDmg > 0)
            {
                dano = (AtkTotal() + BonusDMG) * QuantDmg;
                QuantDmg = 0;
                BonusDMG = 0;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> KABOOOOOOM! {Name} deu TONELADAS de dano! ({dano})");
                Console.ResetColor();
            }
            else
            {
                 Console.WriteLine($"{Name} causou {dano} de dano!");
            }
            return dano;
        }

        public override void Habilidade()
        {
            if (Paint)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [HABILIDADE:] EXPLOSÃO PREPARADA!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [HABILIDADE:] PREPARANDO PINTURA!");
                Console.ResetColor();
            }
            Paint = !Paint;  
        }

        public override void Passiva()
        {
            if (Paint)
            {
                if (QuantDmg > 5)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> [PASSIVA] A TINTA NÃO VAI AGUENTAR MAIS... EXPLOSÃO NO PRÓXIMO ATAQUE!");
                    Console.ResetColor();
                    Paint = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> [PASSIVA] COLOCANDO TINTA! Camada {QuantDmg+1} (+{BonusDMG+ModTotal()} de dano na explosão)!");
                    Console.ResetColor();
                    BonusDMG += ModTotal();
                    QuantDmg +=1; 
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [PASSIVA] Sem tinta!");
                Console.ResetColor();
            }
        }
    }
}
