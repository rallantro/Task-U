using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Task_U.Core.StatusEffects
{
    public class MoonMark : StatusEffect
    {
        public int Stacks;
        public override void Aplicar(PersonagemBase personagem)
        {

        }
        public override void Aplicar(InimigoBase personagem)
        {
            
        }

        [SetsRequiredMembers]
        public MoonMark(string name, int duration, int stacks) : base(name, duration, null)
        {
            Stacks = stacks;
        }
    }
}