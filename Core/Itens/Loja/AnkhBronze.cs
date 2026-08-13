using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Task_U.Core.StatusEffects;
using Task_U.Data;

namespace Task_U.Core.Itens
{
    public class AnkhBronze : Item
    {
        public override void Uso(PersonagemBase Usuario, AppDbContext context)
        {

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"> [SÚPLICA] {Usuario.Name} suplica para trazerem seu aliado de volta!");
            Console.ResetColor();
            if (Usuario.aliado != null && Usuario.aliado.HpAtual <= 0)
            {
                Usuario.aliado.HpAtual = (int)Math.Ceiling(Usuario.aliado.HpMax * 0.25);
                Console.WriteLine($"> {Usuario.aliado.Name} volta a vida!");
                var itemInv = context.InventarioItens.Where(x => x.ItemId == Id).FirstOrDefault();
                if (itemInv != null) context.InventarioItens.Remove(itemInv);
                context.SaveChanges();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [FALHA] As condições para o uso do item (Um aliado morto) não foram atingidas.");
                Console.ResetColor();
                return;
            }


        }
    }
}