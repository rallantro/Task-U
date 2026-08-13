using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core.Itens
{
    public class MantoDoSacrificio : Item
    {
        public override void Effect(PersonagemBase personagem)
        {
            if (personagem.aliado != null && personagem.aliado.HpAtual < personagem.aliado.HpMax * 0.3)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"> [AUTO SACRIFÍCIO] O manto de {personagem.Name} se abre para proteger os mais fracos.");
                Console.ResetColor();
                int shield = (int)Math.Ceiling(0.15 * personagem.HpMax);
                personagem.chanceAlvo += 300;
                personagem.Shield += shield;
                Console.WriteLine($"> {personagem.Name} recebe {shield} de escudo!");
            }
        }
    }
}