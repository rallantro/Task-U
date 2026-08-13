using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Core.StatusEffects;
using Task_U.Data;

namespace Task_U.Core.Itens
{
    public class AdagaDoSacrificio : Item
    {
        public override void Uso(PersonagemBase Usuario, AppDbContext context)
        {

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"> [CONSUMIR] A adaga de {Usuario.Name} o consome para o fortalecer.");
            Console.ResetColor();
            int hpPerdido = (int)Math.Ceiling(Usuario.HpAtual * 0.2);
            Usuario.HpAtual -= hpPerdido;
            Usuario.BuffAtk += hpPerdido * 2;
            Console.WriteLine($"> {Usuario.Name} perde {hpPerdido} de vida, para receber {hpPerdido * 2} de bônus de ataque!");
        }
    }
}