using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Task_U.Core.StatusEffects;
using Task_U.Data;

namespace Task_U.Core.Itens
{
    public class MoedaDaSorte : Item
    {
        public override void Uso(PersonagemBase Usuario, AppDbContext context)
        {

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"> [DESTINO] {Usuario.Name} joga a moeda para o alto");
            Console.ResetColor();
            Random random = new Random();
            int sorte = random.Next(1, 101);
            int chance = 50;
            if (Usuario.Name == "Lurios")
            {
                chance = 60;
            }
            if (sorte <= chance)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [FORTUNA] {Usuario.Name} Recebeu a sorte grande!");
                Console.ResetColor();
                Usuario.HpAtual = Usuario.HpMax;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [DESVENTURA] {Usuario.Name} perdeu tudo!");
                Console.ResetColor();
                Usuario.HpAtual = 1;
            }
            var itemInv = context.InventarioItens.Where(x => x.ItemId == Id).FirstOrDefault();
            if (itemInv != null) context.InventarioItens.Remove(itemInv);
            context.SaveChanges();
        }
    }
}