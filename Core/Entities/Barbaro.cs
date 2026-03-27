using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;
using Task_U.Data;

namespace Task_U.Core
{
    public class Barbaro : PersonagemBase
    {
        private static readonly Random random = new Random();
        public int BaseAtk {get; private set;}
        private bool shildou {get; set;}

        public override int Damage()
        {
            var dano = AtkTotal() + BaseAtk;
            Console.WriteLine($"{Name} causou {dano} de dano!");
            return AtkTotal() + BaseAtk;
        }

        public override void Habilidade()
        {
            
            if (HpAtual > 4)
            {
                HpAtual -= 4;
                BaseAtk += 5; 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [HABILIDADE:] A fúria de {Name} aumenta! +{BaseAtk} de ATK neste turno pelo custo de {4} pontos de vida");
                Console.ResetColor();
            }           
        }

        public override void Passiva()
        {
            BaseAtk = (HpMax - HpAtual) * ModTotal()/4;
            if (BaseAtk > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [PASSIVA] A fúria de {Name} aumenta! (+{BaseAtk} de ATK)");
                Console.ResetColor();
            }
            if (shildou == false && HpAtual <= HpMax/8)
            {
                Shield += HpMax / 5;
                shildou = true;
            }
            if (shildou && Shield == 0)
            {
                shildou = false;
            }
        }
        
    }
}