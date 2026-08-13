using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic;
using Task_U.Core.Combat;

namespace Task_U.Core.StatusEffects
{
    public class DebuffDMG : StatusEffect
    {
        public override void Aplicar(PersonagemBase personagem)
        {
            if (Duration > 0)
            {
                personagem.BuffAtk -= Mod.Value;
                Duration -= 1;
            }  
        }

        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                personagem.BuffAtk -= Mod.Value;
                Duration -= 1;
            }  
        }

        [SetsRequiredMembers]
        public DebuffDMG(string name, int duration, int mod) : base(name, duration, mod)
        {
            
        }
    }
}