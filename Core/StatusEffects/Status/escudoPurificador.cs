using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;


namespace Task_U.Core.StatusEffects
{
    public class escudoPurificador : StatusEffect
    {
        public override void Aplicar(PersonagemBase personagem)
        {
            if (personagem.Shield <= 0)
            {
                Duration = 0;
            }
            var debuffs = personagem.status.Where(x => x.isBeneficial == false).ToList();
            foreach (var debuff in debuffs)
            {
                personagem.status.Remove(debuff);
                Console.WriteLine($"> [{Name.ToUpper()}] Purificou {debuff.Name} de {personagem.Name}!");
            }
        }


        [SetsRequiredMembers]
        public escudoPurificador(string name, int duration) : base(name, duration, null)
        {
            isBeneficial = true;
        }
    }
}