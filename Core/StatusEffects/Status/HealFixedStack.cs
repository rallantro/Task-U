using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Task_U.Core.Combat;
using Task_U.Core;

namespace Task_U.Core.StatusEffects
{
    public class HealFixedStack : StatusEffect
    {

        public override void Aplicar(PersonagemBase personagem)
        {
            if (Duration > 0)
            {
                int cura = Mod.Value;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{personagem.Name} está se regenerando!");
                Console.ResetColor();
                Console.WriteLine($"{personagem.Name} recuperou {cura} de vida!");
                personagem.HpAtual = Math.Min(personagem.HpMax, personagem.HpAtual + cura);
                Duration -= 1;
            }  
        }

        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                int cura = Mod.Value;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{personagem.Name} está envenenado!");
                Console.ResetColor();
                Console.WriteLine($"{personagem.Name} tomou {cura} de vida!");
                personagem.HpAtual = Math.Min(personagem.HpMax, personagem.HpAtual + cura);
                Duration -= 1;
            }  
        }

        [SetsRequiredMembers]
        public HealFixedStack(string name, int duration, int mod) : base(name, duration, mod)
        {
            isBeneficial = true;
        }
    }
}