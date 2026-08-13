using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;
using Task_U.Data;

namespace Task_U.Core
{
    public class Moon : PersonagemBase
    {
        private static readonly Random random = new Random();
        public int BonusDMG {get; private set;}

        public bool MoonState {get; private set;}
        private int CountDown {get; set;}

        public override int Damage()
        {
            if (BonusDMG == (AtkTotal()  + (ModTotal() * 3)) * 2)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("> [RAJADA LUNAR] É LUA CHEIA! Shion e Shun se elevam aos céus para um ataque arrasador!");
                Console.ResetColor();
            } else if (BonusDMG > 0)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("> Shun: Hahahahahahahahaha!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("> Shion: Tudo o que vai, volta...");
                Console.ResetColor();
            }
            var dano = AtkTotal() + BonusDMG;
            Console.WriteLine($"> A legião ataca e casua {dano} pontos de dano!");
            return dano;
        }

        public override void Habilidade()
        {
            if(MoonState == true)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("> Shun: Mas já?! Eu acabei de começar!");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("> Shion: Eu preciso assumir o controle, irmão...");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("> Shion: Pressinto seu retorno, irmão...");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("> Shun: E eu sinto o gosto da vitória!");
                Console.ResetColor();   
            }
            Console.WriteLine("> [FASES DA LUA] A lua tem mais de uma face...");
            Console.WriteLine($"> {Name} irá alternar de fase no início do próximo turno.");
            MoonState = !MoonState;
            CountDown++;
        }

        public override void Passiva()
        {
            if(CountDown < 4)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> [PASSIVA] A lua cheia se aproxima... Faltam {4 - CountDown} para ela chegar...");
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (CountDown == 4)
            {
                BonusDMG = (AtkTotal()  + (ModTotal() * 3)) * 3;
                HpAtual += Damage()/3;
                CountDown = 0;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("> [PASSIVA] É LUA CHEIA! Shion e Shun se fundem em poder absoluto!");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("> Shion: Ela sempre estará...");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("> Shun: CHEIA!");
                Console.ResetColor();
                Console.WriteLine($"{Name} recuperou {Damage()/2} pontos de vida!");
                Console.WriteLine($"O próximo ataque de {Name} dará dano aumentado.");
            }
            else if (MoonState == true)
            {
                BonusDMG = (AtkTotal() + ModTotal()) * 3;
                int perda = HpAtual/4;
                HpAtual -= perda;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("> [PASSIVA] Lua Crescente: A legião assume um sorriso sádico. Shun proporciona o dano aumentado!");
                Console.WriteLine("> Shun: Hora de massacrar!!");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Name} perdeu {perda} pontos de vida!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                BonusDMG = 0;
                int cura = HpMax/5;
                HpAtual += cura;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("> [PASSIVA] Lua Minguante: Shion assume o controle. Recuperando energias...");
                Console.WriteLine("> Shion: Quanto sofrimento causou, meu irmão....");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{Name} recuperou {cura} pontos de vida!");
            }
        }

        public override void Resetar()
        {
            BonusDMG = 0;
            MoonState = false;
            CountDown = 0;
            base.Resetar();
        }
        
    }
}