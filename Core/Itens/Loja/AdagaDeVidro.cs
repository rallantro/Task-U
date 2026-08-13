using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core.Itens
{
    public class AdagaDeVidro : Item
    {
        public override void Effect(PersonagemBase personagem)
        {
            if (personagem.inimigoAlvo != null && personagem.inimigoAlvo.Shield > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"> [PERFURAÇÃO] A adaga dá a {personagem.Name} o poder de perfurar as defesas inimigas");
                Console.ResetColor();
                int buff = (int)Math.Ceiling(0.50 * personagem.inimigoAlvo.Shield);
                personagem.BuffAtk += buff;
                Console.WriteLine($"> {personagem.Name} recebe {buff} de bônus de ataque!");
            }
        }
    }
}