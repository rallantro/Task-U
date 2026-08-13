using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Task_U.Core.StatusEffects
{
    public class Stun : StatusEffect
    {
        public override void Aplicar(PersonagemBase personagem)
        {
            if (Duration > 0)
            {
                personagem.Stuneed = true;
                Duration -= 1;
            }  
        }
        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                personagem.Stuneed = true;
                Duration -= 1;
            }  
        }

        [SetsRequiredMembers]
        public Stun(string name, int duration) : base(name, duration, null)
        {
            
        }
    }
}