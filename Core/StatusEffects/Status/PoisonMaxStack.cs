using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Task_U.Core.Combat;
using Task_U.Core;

namespace Task_U.Core.StatusEffects
{
    public class PoisonMaxStack : StatusEffect
    {

        private double modificador;
        public override void Aplicar(PersonagemBase personagem)
        {
            if (Duration > 0)
            {
                int dano = (int)Math.Ceiling(personagem.HpMax * modificador / 100.0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{personagem.Name} está envenenado!");
                Console.ResetColor();
                Console.WriteLine($"{personagem.Name} tomou {dano} de dano de {Name}!");
                personagem.HpAtual -= dano;
                Duration -= 1;
            }
        }

        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                int dano = (int)Math.Ceiling(personagem.HpMax * modificador / 100.0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{personagem.Name} está envenenado!");
                Console.ResetColor();
                Console.WriteLine($"{personagem.Name} tomou {dano} de dano de {Name}!");
                personagem.HpAtual -= dano;
                Duration -= 1;
            }
        }

        [SetsRequiredMembers]
        public PoisonMaxStack(string name, int duration, double mod) : base(name, duration, null)
        {
            modificador = mod;
        }
    }
}