using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Data;
using System.Text.RegularExpressions;
using Task_U.Core;
using Task_U.Models;

namespace Task_U.Services
{
    public class InventarioServices
    {
        public void VerPersonagens(AppDbContext context, User user)
        {
            var listaPersonagens = new List<PersonagemBase>();
            var personagemInventarios = context.InventarioPersonagens.GroupBy(x => x.PersonagemId).ToList();


            Console.WriteLine("\n ╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine(" ║                SEUS COMPANHEIROS                         ║");
            Console.WriteLine(" ╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine("\n [1] Ver Coleção | [2] Trocar Personagem Ativo | [Qualquer tecla] Voltar");
            var op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    MostrarPersonagens(context, true, listaPersonagens);
                    Console.WriteLine("[Qualquer tecla] Voltar");
                    Console.ReadKey();
                    break;

                case "2":
                    MostrarPersonagens(context, false, listaPersonagens);
                    if (user.Slot1_PersonagemAtivo == null && user.Slot2_PersonagemAtivo == null)
                    {
                        Console.WriteLine("Qual personagem deseja equipar? (Você não tem nenhum personagem ativo!)");
                    }
                    else
                    {
                        var PersonagensAtivos = new List<string>();
                        if (user.Slot1_PersonagemAtivo != null)
                        {
                            PersonagensAtivos.Add(user.Slot1_PersonagemAtivo.Name);
                        }

                        if (user.Slot2_PersonagemAtivo != null)
                        {
                            PersonagensAtivos.Add(user.Slot2_PersonagemAtivo.Name);
                        }
                        string result = String.Join(", ", PersonagensAtivos);
                        Console.WriteLine($"Qual personagem deseja equipar? (Ativo no momento: {result})");
                    }
                    var escolha = Console.ReadLine();
                    var escolhaInt = 0;
                    if (!int.TryParse(escolha, out escolhaInt) || escolhaInt < 0 || escolhaInt > listaPersonagens.Count())
                    {
                        Console.WriteLine(" >> Seleção cancelada ou inválida.");
                        return;
                    }
                    var personagemEscolhido = listaPersonagens[escolhaInt - 1];
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" ┌──────────────────────────────────────────────────────────┐");
                    Console.WriteLine($" │ SELECIONADO: {personagemEscolhido.Name.ToUpper().PadRight(44)}│");
                    Console.WriteLine(" └──────────────────────────────────────────────────────────┘");
                    Console.WriteLine($"[{personagemEscolhido.Desc}]");
                    Console.WriteLine($"────────────");
                    Console.WriteLine("Confirma essa escolha?");
                    Console.WriteLine("[1] Equipar no Slot 1| [2] - Equipar no Slot 2 |  [Qualquer Tecla] Para voltar");
                    escolha = Console.ReadLine();
                    if (escolha == "1")
                    {
                        if (personagemEscolhido.Id == user.Slot2_PersonagemAtivoId)
                        {
                            Console.WriteLine("Este personagem já está no Slot 2!");
                            Console.ReadKey();
                            return;
                        }
                        else
                        {
                            user.Slot1_PersonagemAtivo = personagemEscolhido;
                            user.Slot1_PersonagemAtivoId = personagemEscolhido.Id;
                            context.SaveChanges();
                        }

                    }
                    if (escolha == "2")
                    {
                        if (personagemEscolhido.Id == user.Slot1_PersonagemAtivoId)
                        {
                            Console.WriteLine("Este personagem já está no Slot 1!");
                            Console.ReadKey();
                            return;
                        }
                        else
                        {
                            user.Slot2_PersonagemAtivo = personagemEscolhido;
                            user.Slot2_PersonagemAtivoId = personagemEscolhido.Id;
                            context.SaveChanges();
                        }

                    }
                    else
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
                if (PersonagemOg.Rarity == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                else if (PersonagemOg.Rarity == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                Console.Write($"  [{contador}] ");
                Console.Write($"{PersonagemOg.Name.PadRight(25)}");
                Console.ResetColor();
                Console.WriteLine($" x{quantidade} unidades");

                if (ShowDesc)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    string curta = PersonagemOg.Desc.Length > 50 ? PersonagemOg.Desc.Substring(0, 47) + "..." : PersonagemOg.Desc;
                    Console.WriteLine($"   Descrição: {curta}");
                    Console.ResetColor();
                }
                contador++;
            }
            Console.WriteLine(" ─────────────────────────────────────────────────────────────");
            contador++;
        }


        public void VerItens(AppDbContext context, User user)
        {
            var listaItens = new List<Item>();
            var itens = context.InventarioItens.GroupBy(x => x.ItemId).ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n ╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine(" ║                       SEUS ITENS                         ║");
            Console.WriteLine(" ╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine("\n [1] Ver Coleção | [2] Trocar Itens  Ativo | [Qualquer tecla] Voltar");
            var op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    int contadorGlobal = 1;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n ╔══════════════════════════════════════════════════════════╗");
                    Console.WriteLine(" ║                       SEUS ITENS                         ║");
                    Console.WriteLine(" ╚══════════════════════════════════════════════════════════╝");
                    Console.ResetColor();
                    Console.WriteLine("Equipáveis:");
                    MostrarItens(context, true, listaItens, 2, ref contadorGlobal);
                    Console.WriteLine(" ─────────────────────────────────────────────────────────────");
                    Console.WriteLine("Consumíveis:");
                    MostrarItens(context, true, listaItens, 1, ref contadorGlobal);
                    Console.WriteLine("[Pressione qualquer tecla para voltar]");
                    Console.ReadKey();
                    break;

                case "2":
                    int contador = 1;
                    MostrarItens(context, false, listaItens, 2, ref contador);
                    var itensAtivos = new List<String>();
                    if (user.Slot1_ItemAtivo == null && user.Slot2_ItemAtivo == null)
                    {
                        Console.WriteLine("Qual item deseja equipar? (Você não tem nenhum personagem ativo!)");
                    }
                    else
                    {
                        if (user.Slot1_ItemAtivo != null)
                        {
                            itensAtivos.Add(user.Slot1_ItemAtivo.Name);
                        }

                        if (user.Slot2_ItemAtivo != null)
                        {
                            itensAtivos.Add(user.Slot2_ItemAtivo.Name);
                        }
                        string result = String.Join(", ", itensAtivos);
                        Console.WriteLine($"Qual item deseja equipar? (Ativo no momento: {result})");
                    }
                    var escolha = Console.ReadLine();
                    var escolhaInt = 0;
                    if (!int.TryParse(escolha, out escolhaInt) || escolhaInt < 0 || escolhaInt > listaItens.Count())
                    {
                        Console.WriteLine("Comando inválido!");
                        return;
                    }
                    var itemEscolhido = listaItens[escolhaInt - 1];
                    Console.Clear();
                    Console.WriteLine("Item Selecionado:");
                    Console.WriteLine($"[{itemEscolhido.Name}]");
                    Console.WriteLine($"--");
                    Console.WriteLine($"[{itemEscolhido.Desc}]");
                    Console.WriteLine($"--");
                    Console.WriteLine("Confirma essa escolha?");
                    Console.WriteLine("1 - Equipar no slot 1 | 2 - Equipar no slot 2 |Pressione qualquer coisa para voltar");
                    escolha = Console.ReadLine();
                    if (escolha == "1")
                    {
                        user.Slot1_ItemAtivo = itemEscolhido;
                        user.Slot1_ItemAtivoId = itemEscolhido.Id;
                        context.SaveChanges();
                    }
                    if (escolha == "2")
                    {
                        user.Slot2_ItemAtivo = itemEscolhido;
                        user.Slot2_ItemAtivoId = itemEscolhido.Id;
                        context.SaveChanges();
                    }
                    else
                    {
                        return;
                    }

                    break;

                default:
                    break;
            }


        }

        public void MostrarItens(AppDbContext context, bool ShowDesc, List<Item> listaItens, int TypeItem, ref int contador)
        {
            var itens = context.InventarioItens.GroupBy(x => x.ItemId).ToList();

            foreach (var item in itens)
            {
                var ItemOg = context.Itens.Find(item.Key);
                if (ItemOg.Type == TypeItem)
                {
                    listaItens.Add(ItemOg);
                    var quantidade = item.Count();
                    if (ItemOg.Rarity == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (ItemOg.Rarity == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (ItemOg.Rarity == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    else if (ItemOg.Rarity == 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write($"  [{contador}] ");
                    Console.Write($"{ItemOg.Name.PadRight(25)}");
                    Console.ResetColor();
                    Console.WriteLine($" x{quantidade} unidades");
                    if (ShowDesc)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        var att = Regex.Match(ItemOg.Desc, @"\((.*?)\)");
                        var bonus = att.Value;
                        string curta = ItemOg.Desc.Length > 50 ? $"{ItemOg.Desc.Substring(0, 46 - bonus.Length).Trim()}... {bonus}" : ItemOg.Desc;
                        Console.WriteLine($"   Descrição: {curta}");
                        Console.ResetColor();
                    }
                    contador++;
                }
            }

        }
    }
}