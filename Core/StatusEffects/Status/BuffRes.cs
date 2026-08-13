using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic;
using Task_U.Core.Combat;

namespace Task_U.Core.StatusEffects
{
    public class BuffRes : StatusEffect
    {
        private double modificador;
        public override void Aplicar(PersonagemBase personagem)
        {
            
            if (Duration > 0)
            {
                personagem.debuffRes -= modificador;
                Duration -= 1;
            }  
        }

        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                personagem.debuffRes -= modificador;
                Duration -= 1;
            }  
        }

        [SetsRequiredMembers]
        public BuffRes(string name, int duration, double mod) : base(name, duration, null)
        {
           modificador = mod; 
           isBeneficial = true;
        }
    }
}