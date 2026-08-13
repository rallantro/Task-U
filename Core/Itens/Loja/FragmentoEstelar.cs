using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core.Itens
{
    public class FragmentoEstelar : Item
    {
        public override void Effect(PersonagemBase personagem)
        {
            if (personagem.inimigoAlvo != null && personagem.inimigoAlvo.HpAtual > personagem.inimigoAlvo.HpMax * 0.7)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> [RESSONÂNCIA ASTRAL] A luz fria de {personagem.Name} brilha intensamente contra os que ainda têm fôlego.");
                Console.ResetColor();
                int buff = (int)Math.Ceiling(0.15 * personagem.AtkTotal());
                personagem.BuffAtk += buff;
                Console.WriteLine($"> {personagem.Name} recebe {buff} de bônus de ataque!");
            }
        }
    }
}