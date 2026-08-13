using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core.Itens
{
    public class oniHeart : Item
    {
        public override void Effect(PersonagemBase personagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"> [FULGOR DA BATALHA] {personagem.Name} fica mais forte com base na sua vida perdida.");
            Console.ResetColor();
            personagem.BuffAtk += (int)Math.Ceiling(40.0 * (1 - personagem.HpAtual / personagem.HpMax));
        }
    }
}