using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core.Itens
{
    public class AmpulhetaAreia : Item
    {
        public override void Effect(PersonagemBase personagem)
        {
            if (personagem != null)
            {
                if (personagem.HpAtual > personagem.HpMax * 0.5)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> [FLUXO NORTE] A areia da ampulheta de {personagem.Name} desafia a gravidade.");
                    Console.ResetColor();
                    int buff = (int)Math.Ceiling(0.10 * personagem.AtkTotal());
                    personagem.BuffAtk += buff;
                    Console.WriteLine($"> {personagem.Name} recebe {buff} de bônus de dano!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> [FLUXO SUL] A areia da ampulheta de {personagem.Name} finalmente começa a cair.");
                    Console.ResetColor();
                    int shield = (int)Math.Ceiling(0.10 * personagem.HpMax);
                    personagem.Shield += shield;
                    Console.WriteLine($"> {personagem.Name} recebe {shield} de escudo!");
                }
            }
        }
    }
}