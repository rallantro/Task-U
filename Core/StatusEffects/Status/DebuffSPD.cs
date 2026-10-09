using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic;
using Task_U.Core.Combat;

namespace Task_U.Core.StatusEffects
{
    public class DebuffSPD : StatusEffect
    {
        public override void Aplicar(PersonagemBase personagem)
        {
            if (Duration > 0)
            {
                double porcentagem = Math.Round(Mod.Value / personagem.SpeedTotal() * 100.0, 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{personagem.Name} recebeu +{porcentagem:F1}% (-{Mod.Value}) de velocidade por {Name}!");
                Console.ResetColor();
                Duration -= 1;
            }
        }

        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                double porcentagem = Math.Round(Mod.Value / personagem.SpeedTotal() * 100.0, 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{personagem.Name} recebeu +{porcentagem:F1}% (-{Mod.Value}) de velocidade por {Name}!");
                Console.ResetColor();
                Duration -= 1;
            }
        }

        [SetsRequiredMembers]
        public DebuffSPD(string name, int duration, int mod) : base(name, duration, mod)
        {
            isBeneficial = false;
        }
    }
}