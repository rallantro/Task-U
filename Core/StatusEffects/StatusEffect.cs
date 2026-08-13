using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;

namespace Task_U.Core.StatusEffects
{
    public abstract class StatusEffect
    {
        public required string Name;
        public required int Duration;
        public int? Mod;

        public bool isBeneficial = false;

        public StatusEffect(string name, int duration, int? mod)
        {
            Name = name;
            Duration = duration;
            if (mod != null)
            {
                Mod = mod;
            }
        }

        public virtual void Aplicar(PersonagemBase personagem) {
            
        }
        public virtual void Aplicar(InimigoBase personagem) {
            
        }
    }
}