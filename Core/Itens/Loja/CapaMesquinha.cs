using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core.Itens
{
    public class CapaMesquinha : Item
    {
        private int vidaOld;
        public override void Effect(PersonagemBase personagem)
        {
            if (personagem.HpAtual >= vidaOld)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"> [AUTO PRESERVAÇÃO] {personagem.Name} é fortalecido, por não ter se ferido.");
                Console.ResetColor();
                int buff = (int)Math.Ceiling(0.50 * personagem.AtkTotal());
                personagem.BuffAtk += buff;
                Console.WriteLine($"> {personagem.Name} recebe {buff} de bônus de ataque!");
            }
            vidaOld = personagem.HpAtual;
        }

        public override void Resetar()
        {
            vidaOld = 0;
            base.Resetar();
        }
    }
}