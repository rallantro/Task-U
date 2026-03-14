using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Models;
using Todo_Gacha.Data;

namespace Todo_Gacha.Core
{
    public class Barbaro : PersonagemBase
    {
        private static readonly Random random = new Random();
        public int BaseAtk {get; private set;}

        public int HpMax {get; set;}

        public override int Damage()
        {
            var dano = Atk + BaseAtk;
            Console.WriteLine($"{Name} causou {dano} de dano!");
            return Atk + BaseAtk;
        }

        public override void Habilidade()
        {
            
            if (Hp > 4)
            {
                Hp -= 4;
                BaseAtk += 5; 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [HABILIDADE:] A fúria de {Name} aumenta! +{BaseAtk} de ATK neste turno pelo custo de {4} pontos de vida");
                Console.ResetColor();
            }           
        }

        public override void Passiva(User user)
        {
            BaseAtk = (HpMax - Hp) * Mod/2;
            if (BaseAtk > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [PASSIVA] A fúria de {Name} aumenta! (+{BaseAtk} de ATK)");
                Console.ResetColor();
            }
        }
        
    }
}