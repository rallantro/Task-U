using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic;
using Task_U.Core.Combat;

namespace Task_U.Core.StatusEffects
{
    public class Blind : StatusEffect
    {
        public override void Aplicar(PersonagemBase personagem)
        {
            if (Duration > 0)
            {
                personagem.Blinded = true;
                Duration -= 1;
            }  
        }

        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                personagem.Blinded = true;
                Duration -= 1;
            }  
        }

        [SetsRequiredMembers]
        public Blind(string name, int duration) : base(name, duration, null)
        {
            
        }
    }
}