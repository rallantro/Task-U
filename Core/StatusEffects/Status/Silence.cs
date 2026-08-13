using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Task_U.Core.StatusEffects
{
    public class Silence : StatusEffect
    {
        public override void Aplicar(PersonagemBase personagem)
        {
            if (Duration > 0)
            {
                personagem.Silenced = true;
                Duration -= 1;
            }  
        }

        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                personagem.Silenced = true;
                Duration -= 1;
            }  
        }

        [SetsRequiredMembers]
        public Silence(string name, int duration) : base(name, duration, null)
        {
            
        }
    }
}