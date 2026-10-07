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
        private bool aplicado = false;
        public override void Aplicar(PersonagemBase personagem)
        {

            if (Duration > 0)
            {
                if (!aplicado)
                {
                    personagem.debuffRes += modificador;
                    aplicado = true;
                }
                Duration--;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{personagem.Name} recebeu {Name}! (+{modificador * 100:F2}% de dano)");
                Console.ResetColor();

                if (Duration == 0 && aplicado)
                {
                    personagem.debuffRes -= modificador;
                    aplicado = false;
                }
            }
        }

        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                if (!aplicado)
                {
                    personagem.debuffRes += modificador;
                    aplicado = true;
                }
                Duration--;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{personagem.Name} recebeu {Name}! (+{modificador * 100:F2}% de dano)");
                Console.ResetColor();
                if (Duration == 0 && aplicado)
                {
                    personagem.debuffRes -= modificador;
                    aplicado = false;
                }
            }
        }

        [SetsRequiredMembers]
        public DebuffRes(string name, int duration, double mod) : base(name, duration, null)
        {
            modificador = mod;
        }
    }
}