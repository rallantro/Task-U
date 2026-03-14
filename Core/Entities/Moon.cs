using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Models;
using Todo_Gacha.Data;

namespace Todo_Gacha.Core
{
    public class Moon : PersonagemBase
    {
        private static readonly Random random = new Random();
        public int BonusDMG {get; private set;}

        public bool MoonState {get; private set;}
        private int CountDown {get; set;}

        public override int Damage()
        {
            var dano = AtkTotal() + BonusDMG;
            Console.WriteLine($"> A legião ataca e casua {dano} pontos de dano!");
            return dano;
        }

        public override void Habilidade()
        {
            Console.WriteLine("> [FASES DA LUA] A lua tem mais de uma face...");
            MoonState = !MoonState;
            CountDown++;
        }

        public override void Passiva()
        {
            if(CountDown < 7)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> [PASSIVA] A lua cheia se aproxima... Faltam {7 - CountDown} para ela chegar...");
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (CountDown == 7)
            {
                BonusDMG = (AtkTotal()  + (ModTotal() * 3)) * 2;
                HpAtual += Damage()/2;
                CountDown = 0;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("> [PASSIVA] É LUA CHEIA! Shion e Shun se fundem em poder absoluto!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{Name} recuperou {Damage()/2} pontos de vida!");
            }
            else if (MoonState == true)
            {
                BonusDMG = (AtkTotal() + ModTotal()) * 2;
                int perda = HpAtual/4;
                HpAtual -= perda;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("> [PASSIVA] Lua Crescente: A legião assume um sorriso sádico. Shun proporciona o dano aumentado!");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Name} perdeu {perda} pontos de vida!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                BonusDMG = 0;
                int curaRan = random.Next(3, 6);
                int cura = (AtkTotal() * 2 + ModTotal()) / curaRan;
                HpAtual += cura;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("> [PASSIVA] Lua Minguante: Shion assume o controle. Recuperando energias...");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{Name} recuperou {cura} pontos de vida!");
            }
        }
        
    }
}