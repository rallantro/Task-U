using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Services;
using Todo_Gacha.Models;
using Todo_Gacha.Data;

namespace Todo_Gacha.Core
{
    public class Grafiteiro : PersonagemBase
    {
        private int BonusDMG;
        private int QuantDmg;
        private bool Paint;

        public override int Damage()
        {
            int dano = Atk;
            if (!Paint && QuantDmg > 0)
            {
                dano = (Atk + BonusDMG) * QuantDmg;
                QuantDmg = 0;
                BonusDMG = 0;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> KABOOOOOOM! {Name} deu TONELADAS de dano!");
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

        public override void Passiva(User user)
        {
            if (Paint)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [PASSIVA] COLOCANDO TINTA! Camada {QuantDmg+1} (+{BonusDMG+Mod} de dano na explosão)!");
                Console.ResetColor();
                BonusDMG += Mod;
                QuantDmg +=1;
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
