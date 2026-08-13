using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Core.StatusEffects;
using Task_U.Data;

namespace Task_U.Core.Itens
{
    public class aranhaItem : Item
    {
        public override void Uso(PersonagemBase Usuario, AppDbContext context)
        {
            if (Usuario.inimigoAlvo != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [ITEM] {Usuario.Name} lançou uma Aranha de Caça!");
                Console.WriteLine($"> A aranha picou {Usuario.inimigoAlvo.Name}, injetando toxinas necrotizantes.");
                Console.ResetColor();
                var poison = new PoisonMaxStack("Envenenamento Sintético", 5, 3);
                Usuario.inimigoAlvo.status.Add(poison);
            }
            var itemInv = context.InventarioItens.Where(x => x.ItemId == Id).FirstOrDefault();
            if (itemInv != null) context.InventarioItens.Remove(itemInv);
            context.SaveChanges();
        }
    }
}