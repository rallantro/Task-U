using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Task_U.Core.Entities;


namespace Task_U.Core.StatusEffects
{
    public class PriestShield : StatusEffect
    {
        private double modificador;
        public override void Aplicar(PersonagemBase personagem)
        {
            PersonagemBase? priest = null;
            if (personagem.GetType() == typeof(Priest))
            {
                priest = personagem;
            }
            else if (personagem.aliado != null && personagem.aliado.GetType() == typeof(Priest))
            {
                priest = personagem.aliado;
            }

            if (personagem.Shield <= 0)
            {
                int cura = (int)Math.Ceiling(personagem.HpMax * modificador);
                personagem.curar(Name, cura);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> [RUPTURA DO ORVALHO] O {Name} foi rompido, transbordando de vida!");
                if (priest != null)
                {
                    Console.WriteLine($"> {priest.Name} O que era barreira, agora se torna vida! Transborde e cure-se!");
                }
                Console.ResetColor();
                Console.WriteLine($"{personagem.Name} recuperou {cura} de vida!");
                if (personagem.aliado != null)
                {
                    personagem.aliado.curar(Name, cura);
                    Console.WriteLine($"> {personagem.aliado.Name} recuperou {cura} de vida!");
                }
                Duration = 0;
            }
        }


        [SetsRequiredMembers]
        public PriestShield(string name, int duration, double cura) : base(name, duration, null)
        {
            modificador = cura;
            isBeneficial = true;
        }
    }
}