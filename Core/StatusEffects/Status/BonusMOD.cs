using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic;
using Task_U.Core.Combat;

namespace Task_U.Core.StatusEffects
{
    public class BonusMOD : StatusEffect
    {
        public override void Aplicar(PersonagemBase personagem)
        {
            if (Duration > 0)
            {
                personagem.BuffMod += Mod.Value;
                Duration -= 1;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{personagem.Name} foi fortalecido por: '{Name}', recebendo {Mod.Value} no modificador.");
                Console.ResetColor();
            }  
        }

        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                personagem.BuffMod += Mod.Value;
                Duration -= 1;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{personagem.Name} foi fortalecido por: '{Name}', recebendo {Mod.Value} no modificador.");
                Console.ResetColor();
            }  
        }

        [SetsRequiredMembers]
        public BonusMOD(string name, int duration, int mod) : base(name, duration, mod)
        {
            isBeneficial = true;
        }
    }
}