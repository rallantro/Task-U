using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Task_U.Services;
using Task_U.Models;
using Task_U.Data;

namespace Task_U.Core
{
    public class Apostador : PersonagemBase
    {
        private static readonly Random random = new Random();
        public int BaseAtk {get; private set;}
        
        [NotMapped]
        public int BonusDMG {get; private set;}
        public bool Win {get; set;}

        public override int Damage()
        {
            int dano = AtkTotal() + BaseAtk + BonusDMG;
            Console.WriteLine($"{Name} causou {dano} de dano!");
            return dano;
        }

        public override void Habilidade()
        {
            int cura = random.Next(1, ModTotal());
            if (random.Next(0, 100) < 30 && HpAtual > ModTotal()) 
            {
                this.HpAtual -= cura; 
                Win = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [LET IT RIDE!] {Name} Teve azar!");
                Console.WriteLine($"> {Name} perdeu {cura} pontos de vida!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                this.HpAtual += cura;
                Win = true;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [LET IT RIDE!] {Name} Teve sorte!");
                Console.WriteLine($"> {Name} se curou em {cura} pontos de vida!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (Win == true)
            {
                BonusDMG += AtkTotal();
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

        public override void Passiva( )
        {
            
            if (user.PityEpic > 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [PASSIVA] O azar de hoje é o dano de amanhã! {Name} sente a sorte mudando.");
                Console.WriteLine($"> [PASSIVA] O ataque de {Name} foi dobrado!");
                Console.ResetColor();
                BaseAtk = AtkTotal() + BonusDMG;
            } else
            {
                BaseAtk = 0;
            }
        }
    }
}