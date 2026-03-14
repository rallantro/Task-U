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
        public void VerPersonagens(AppDbContext context, User user)
        {
            var listaPersonagens = new List<PersonagemBase>();
            var personagemInventarios = context.InventarioPersonagens.GroupBy(x => x.PersonagemId).ToList();


            Console.WriteLine("----- INVENTÁRIO DE PERSONAGENS -----");
            Console.WriteLine("1 - Ver inventário | 2 - Trocar Personagem Ativo");

            switch (Console.ReadLine())
            {
                case "1":
                    MostrarPersonagens(context, true, listaPersonagens);
                break; 

                case "2":
                    MostrarPersonagens(context, false, listaPersonagens);
                    if (user.PersonagemAtivo == null) {
                        Console.WriteLine("Qual personagem deseja equipar? (Você não tem nenhum personagem ativo!)");
                    }
                    else
                    {
                        Console.WriteLine($"Qual personagem deseja equipar? (Personagem ativo no momento: {user.PersonagemAtivo.Name})");
                    }
                    var escolha = Console.ReadLine();
                    var personagemEscolhido = listaPersonagens[int.Parse(escolha) - 1];
                    Console.Clear();
                    Console.WriteLine("Personagem Selecionado:");
                    Console.WriteLine($"[{personagemEscolhido.Name}]");
                    Console.WriteLine($"--");
                    Console.WriteLine($"[{personagemEscolhido.Desc}]");
                    Console.WriteLine($"--");
                    Console.WriteLine("Confirma essa escolha?");
                    Console.WriteLine("1 - Equipar | Pressione qualquer coisa para voltar");
                    if (Console.ReadLine() == "1")
                    {
                        user.PersonagemAtivo = personagemEscolhido;
                        user.PersonagemAtivoId = personagemEscolhido.Id;
                        context.SaveChanges();
                    } else
                    {
                        return;
                    }
                    
                break;
                
                default:
                break;
            }

            
        }

        public void MostrarPersonagens(AppDbContext context, bool ShowDesc, List<PersonagemBase> listaPersonagens)
        {
            var personagemInventarios = context.InventarioPersonagens.GroupBy(x => x.PersonagemId).ToList();

            int contador = 1;

            foreach (var personagem in personagemInventarios)
                {
                var PersonagemOg = context.Personagens.Find(personagem.Key);
                listaPersonagens.Add(PersonagemOg);
                var quantidade = personagem.Count();
                if (PersonagemOg.Rarity == 3) {
                Console.ForegroundColor = ConsoleColor.Magenta;
                } else if (PersonagemOg.Rarity == 4)
                {
                Console.ForegroundColor = ConsoleColor.Yellow; 
                }
                Console.WriteLine("--");
                Console.WriteLine($"{contador} - [{PersonagemOg.Name}] x {quantidade}");
                Console.ResetColor();
                if (ShowDesc)
                {
                  Console.WriteLine($"Descrição: {PersonagemOg.Desc}");  
                }
                contador++;
            }

            
        }

        public void VerItens(AppDbContext context, User user)
        {
            var listaItens = new List<Item>();
            var itens = context.InventarioItens.GroupBy(x => x.ItemId).ToList();

            Console.WriteLine("----- INVENTÁRIO DE ITENS -----");
            Console.WriteLine("1 - Ver inventário | 2 - Trocar Item Equipado");

            switch (Console.ReadLine())
            {
                case "1":
                    MostrarItens(context, true, listaItens, 2);
                    MostrarItens(context, true, listaItens, 1);
                    Console.WriteLine("Pressione qualquer tecla para voltar");
                    Console.ReadKey();
                break; 

                case "2":
                    MostrarItens(context, false, listaItens, 2);
                    if (user.ItemAtivo == null) {
                        Console.WriteLine("Qual item deseja equipar? (Você não tem nenhum item equipado!)");
                    }
                    else
                    {
                        Console.WriteLine($"Qual item deseja equipar? (Item ativo no momento: {user.ItemAtivo.Name})");
                    }
                    var escolha = Console.ReadLine();
                    var itemEscolhido = listaItens[int.Parse(escolha) - 1];
                    Console.Clear();
                    Console.WriteLine("Item Selecionado:");
                    Console.WriteLine($"[{itemEscolhido.Name}]");
                    Console.WriteLine($"--");
                    Console.WriteLine($"[{itemEscolhido.Desc}]");
                    Console.WriteLine($"--");
                    Console.WriteLine("Confirma essa escolha?");
                    Console.WriteLine("1 - Equipar | Pressione qualquer coisa para voltar");
                    if (Console.ReadLine() == "1")
                    {
                        user.ItemAtivo = itemEscolhido;
                        user.ItemAtivoId = itemEscolhido.Id;
                        context.SaveChanges();
                    } else
                    {
                        return;
                    }
                    
                break;
                
                default:
                break;
            }

           
        }

        public void MostrarItens(AppDbContext context, bool ShowDesc, List<Item> listaItens, int TypeItem)
        {
            var itens = context.InventarioItens.GroupBy(x => x.ItemId).ToList();

            int contador = 1;
            if (TypeItem == 1)
            {
                Console.WriteLine("Itens Consumíveis:");
            }
            else
            {
                Console.WriteLine("Itens Equipáveis:");
            }

             foreach (var item in itens)
            {
                var ItemOg = context.Itens.Find(item.Key);
                if (ItemOg.Type == TypeItem)
                {
                    listaItens.Add(ItemOg);
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
                    Console.WriteLine($"{contador} - [{ItemOg.Name}] x {quantidade}");
                    Console.ResetColor();
                    if (ShowDesc)
                    {
                    Console.WriteLine($"Descrição: {ItemOg.Desc}");  
                    }
                    contador++;
                }
            }
            
        }
    }
}