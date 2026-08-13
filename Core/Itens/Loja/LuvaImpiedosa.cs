using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core.Itens
{
    public class LuvaImpiedosa : Item
    {
        public override void Effect(PersonagemBase personagem)
        {
            if (personagem.inimigoAlvo != null && personagem.inimigoAlvo.HpAtual < personagem.inimigoAlvo.HpMax * 0.4)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"> [IMPIEDADE] A luva sussura, pede e provoca {personagem.Name}, para atacar com mais força os mais fracos.");
                Console.ResetColor();
                int buff = (int)Math.Ceiling(0.25 * personagem.AtkTotal());
                personagem.BuffAtk += buff;
                Console.WriteLine($"> {personagem.Name} recebe {buff} de bônus de ataque!");
            }
        }
    }
}