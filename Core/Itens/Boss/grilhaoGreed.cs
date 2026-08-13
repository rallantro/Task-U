using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core.Itens
{
    public class grilhaoGreed : Item
    {
        public override void Effect(PersonagemBase personagem)
        {
            if (personagem.aliado != null && personagem.aliado.status.Count(x => x.isBeneficial == true) < personagem.status.Count(x => x.isBeneficial == true)|| personagem.aliado == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"> [EXCLUSIVIDADE] {personagem.Name} sente-se melhor, por ter mais do que os outros... As correntes o fortalece... Mas o aprisiona...");
                Console.ResetColor();
                personagem.BuffAtk += (int)Math.Ceiling(0.2 * personagem.AtkTotal());
            }

        }
    }
}