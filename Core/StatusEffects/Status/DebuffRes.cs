using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic;
using Task_U.Core.Combat;

namespace Task_U.Core.StatusEffects
{
    public class DebuffRes : StatusEffect
    {
        private double modificador;
        public override void Aplicar(PersonagemBase personagem)
        {
            
            if (Duration > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{personagem.Name} foi afetado por {Name}! (recebe +{modificador * 100}% de dano)");
                Console.ResetColor();
                personagem.debuffRes += modificador;
                Duration -= 1;
            }  
        }

        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{personagem.Name} foi afetado por {Name}! (recebe +{modificador * 100}% de dano)");
                Console.ResetColor();
                personagem.debuffRes += modificador;
                Duration -= 1;
            }  
        }

        [SetsRequiredMembers]
        public DebuffRes(string name, int duration, double mod) : base(name, duration, null)
        {
           modificador = mod; 
        }
    }
}