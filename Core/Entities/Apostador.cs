using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Services;
using Todo_Gacha.Models;
using Todo_Gacha.Data;

namespace Todo_Gacha.Core
{
    public class Apostador : PersonagemBase
    {
        private static readonly Random random = new Random();
        public int BaseAtk {get; private set;}
        public int BonusDMG {get; private set;}
        public bool Win {get; set;}

        public override int Damage()
        {
            int dano = Atk + BaseAtk + BonusDMG;
            Console.WriteLine($"{Name} causou {dano} de dano!");
            return dano;
        }

        public override void Habilidade()
        {
            int cura = random.Next(1, Mod);
            if (random.Next(0, 100) < 30 && Hp > Mod) 
            {
                this.Hp -= cura; 
                Win = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [LET IT RIDE!] {Name} Teve azar!");
                Console.WriteLine($"> {Name} perdeu {cura} pontos de vida!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                this.Hp += cura;
                Win = true;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [LET IT RIDE!] {Name} Teve sorte!");
                Console.WriteLine($"> {Name} se curou em {cura} pontos de vida!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (Win == true)
            {
                BonusDMG += Atk;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [LET IT RIDE!] {Name} Tem um bônus de {BonusDMG} em cada ataque!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                BonusDMG = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [LET IT RIDE!] {Name} perdeu o bônus de seus ataques...");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public override void Passiva(User user)
        {
            
            if (user.PityEpic > 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [PASSIVA] O azar de hoje é o dano de amanhã! {Name} sente a sorte mudando.");
                Console.WriteLine($"> [PASSIVA] O ataque de {Name} foi dobrado!");
                Console.ResetColor();
                BaseAtk = Atk + BonusDMG;
            } else
            {
                BaseAtk = 0;
            }
        }
    }
}