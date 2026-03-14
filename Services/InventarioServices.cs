using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Data;
using Todo_Gacha.Core;
using Todo_Gacha.Models;

namespace Todo_Gacha.Services
{
    public class InventarioServices
    {
        public void VerPersonagens(AppDbContext context)
        {
            var personagemInventarios = context.InventarioPersonagens.GroupBy(x => x.PersonagemId).ToList();

            foreach (var personagem in personagemInventarios)
            {
                var PersonagemOg = context.Personagens.Find(personagem.Key);
                var quantidade = personagem.Count();
                if (PersonagemOg.Rarity == 3) {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                } else if (PersonagemOg.Rarity == 4)
                {
                   Console.ForegroundColor = ConsoleColor.Yellow; 
                }
                Console.WriteLine("--");
                Console.WriteLine($"[{PersonagemOg.Name}] x {quantidade}");
                Console.ResetColor();
                Console.WriteLine($"Descrição: {PersonagemOg.Desc}");
            }
        }

        public void VerItens(AppDbContext context)
        {
            var itens = context.InventarioItens.GroupBy(x => x.ItemId).ToList();

            foreach (var item in itens)
            {
                var ItemOg = context.Itens.Find(item.Key);
                var quantidade = item.Count();
                if (ItemOg.Rarity == 1) {
                    Console.ForegroundColor = ConsoleColor.White; 
                } else if (ItemOg.Rarity == 2){
                    Console.ForegroundColor = ConsoleColor.Blue;
                } else if (ItemOg.Rarity == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                } else if (ItemOg.Rarity == 4)
                {
                   Console.ForegroundColor = ConsoleColor.Yellow; 
                }
                Console.WriteLine("--");
                Console.WriteLine($"[{ItemOg.Name}] x {quantidade}");
                Console.WriteLine($"Descrição: {ItemOg.Desc}");
            }
        }
    }
}