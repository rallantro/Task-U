using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Data;
using Task_U.Core;
using Task_U.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Task_U.Services
{
    public class LojaService
    {
        public Dictionary<PersonagemBase, int> produtos = new Dictionary<PersonagemBase, int>();
        public Dictionary<Item, int> itensProdutos = new Dictionary<Item, int>();
        private Random random = new Random();

        public void atualizarLoja(AppDbContext context)
        {
            var hoje = DateTime.Now.Date;
            var user = context.Users.Find(1);

            if (user != null && user.LastLojaUpdate.Month != hoje.Month)
            {
                var antigos = context.loja;
                context.loja.RemoveRange(antigos);
                bool promocao = false;
                bool SSR = false;
                int maxQuant = context.Personagens.Count(x => x.Rarity == 3);
                int quant = random.Next(1, Math.Min(maxQuant, Math.Max(2, maxQuant / 2)));
                int chance = random.Next(1, 100);
                var alvosSR = context.Personagens.Where(x => x.Rarity == 3).OrderBy(x => EF.Functions.Random()).ToList();
                var alvoSRR = context.Personagens.Where(x => x.Rarity == 4).OrderBy(x => EF.Functions.Random()).FirstOrDefault();
                produtos.Clear();
                for (int i = 0; i < quant; i++)
                {

                    if (chance < 10 && alvoSRR != null && !SSR)
                    {
                        produtos[alvoSRR] = 1000;
                        SSR = true;
                    }
                    else
                    {
                        int desconto = random.Next(25, 51);
                        var alvo = alvosSR[i];
                        if (alvo != null && !produtos.ContainsKey(alvo))
                        {
                            if (!promocao)
                            {
                                produtos[alvo] = (int)Math.Round((decimal)200 * (100 - desconto) / 100);
                                promocao = true;
                            }
                            else
                            {
                                produtos[alvo] = 200;
                            }

                        }
                    }
                }

                foreach (var personagem in produtos.ToList())
                {
                    var produto = new Loja
                    {
                        personagemId = personagem.Key.Id,
                        preco = personagem.Value,
                        personagem = true
                    };
                    context.loja.Add(produto);
                }

                var itensAlvo = context.Itens.Where(x => x.exclLoja == true).OrderBy(x => EF.Functions.Random()).ToList();
                int maxItens = itensAlvo.Count;
                int quantItens = random.Next(1, Math.Min(maxItens, Math.Max(2, maxItens / 2)));
                itensProdutos.Clear();
                for (int i = 0; i < quantItens; i++)
                {
                    var alvo = itensAlvo[i];
                    itensProdutos[alvo] = 75;
                }
                foreach (var item in itensProdutos.ToList())
                {
                    var produto = new Loja
                    {
                        personagemId = item.Key.Id,
                        preco = item.Value,
                        personagem = false
                    };
                    context.loja.Add(produto);
                }


                user.LastLojaUpdate = hoje;

                context.SaveChanges();
            }
        }

        public void exibirLoja(AppDbContext context)
        {
            int bitsUsuario = context.Users.First(x => x.Id == 1).Bits;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
            Console.Write("║ ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("PROTOCOLO DE AQUISIÇÃO // TERMINAL_ACCESS");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(55, 1);
            Console.WriteLine($"BITS: {bitsUsuario:D5} ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("\n ID   SIGILOS             RARIDADE    CUSTO");
            Console.WriteLine(" ───  ───────────────────  ──────────  ───────────");

            var produtos = context.loja.Where(x => x.personagem).ToList();

            foreach (var item in produtos)
            {
                PersonagemBase? personagem = context.Personagens.Find(item.personagemId);

                if (personagem != null)
                {
                    if (item.comprado)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(string.Format(" [{0}]  {1,-20} {2,-11} {3}",
                            item.id,
                            personagem.Name.ToUpper(),
                            personagem.Rarity == 4 ? "SSR" : "SR",
                            "[ADQUIRIDO]"));
                    }
                    else
                    {
                        Console.ForegroundColor = personagem.Rarity == 3 ? ConsoleColor.Magenta : ConsoleColor.Yellow;
                        if (item.preco < 200)
                        {
                            Console.WriteLine(string.Format(" [{0}]  {1,-20} {2,-11} {3} Bits",
                                item.id,
                                $"{personagem.Name.ToUpper()} ↓%",
                                personagem.Rarity == 4 ? "SSR" : "SR",
                                item.preco));
                        }
                        else
                        {
                            Console.WriteLine(string.Format(" [{0}]  {1,-20} {2,-11} {3} Bits",
                                item.id,
                                personagem.Name.ToUpper(),
                                personagem.Rarity == 4 ? "SSR" : "SR",
                                item.preco));
                        }

                    }
                }

            }

            Console.ResetColor();


            Console.WriteLine("\n ID   ARTEFATOS           RARIDADE    CUSTO");
            Console.WriteLine(" ───  ───────────────────  ──────────  ───────────");

            var itensLista = context.loja.Where(x => x.personagem == false).ToList();

            foreach (var item in itensLista)
            {
                Item? artefato = context.Itens.Find(item.personagemId);

                if (artefato != null)
                {
                    if (item.comprado)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(string.Format(" [{0}]  {1,-20} {2,-11} {3}",
                            item.id,
                            artefato.Name.ToUpper(),
                            artefato.Rarity == 4 ? "SSR" : "SR",
                            "[ADQUIRIDO]"));
                    }
                    else
                    {
                        Console.ForegroundColor = artefato.Rarity == 3 ? ConsoleColor.Magenta : ConsoleColor.Yellow;

                        Console.WriteLine(string.Format(" [{0}]  {1,-20} {2,-11} {3} Bits",
                            item.id,
                            artefato.Name.ToUpper(),
                            artefato.Rarity == 4 ? "SSR" : "SR",
                            item.preco));
                    }
                }

            }

            Console.ResetColor();
            Console.WriteLine("\n [0] Voltar");
            Console.Write("\n > SELECIONE O ID PARA SINCRONIZAR: ");
            var escolha = Console.ReadLine();
            if (int.TryParse(escolha, out int numero))
            {
                if (numero != 0)
                {
                    processarCompra(numero, context);
                }
            }
            else
            {
                Console.WriteLine("Dígito inválido!");
            }
        }

        public void processarCompra(int produtoId, AppDbContext context)
        {
            var usuario = context.Users.First(x => x.Id == 1);
            var itemLoja = context.loja.Find(produtoId);

            if (itemLoja == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Id inválido!");
                Console.ResetColor();
            }
            else
            {
                if (itemLoja.comprado == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Você já conseguiu a aquisição!");
                    Console.ResetColor();
                    return;
                }

                if (usuario.Bits < itemLoja.preco)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Você não possui bits o suficiente para isso!");
                    Console.ResetColor();
                    return;
                }

                usuario.Bits -= itemLoja.preco;
                if (itemLoja.personagem)
                {
                    var existente = context.InventarioPersonagens.FirstOrDefault(p => p.UserId == usuario.Id && p.PersonagemId == itemLoja.personagemId);
                    if (existente != null)
                    {
                        existente.Quantidade++;
                    }
                    else
                    {
                        var personagemNew = new PersonagemInventario
                        {
                            UserId = usuario.Id,
                            PersonagemId = itemLoja.personagemId,
                            Quantidade = 1,
                            NodesNivel = 0
                        };
                        context.InventarioPersonagens.Add(personagemNew);
                    }
                    PersonagemBase? personagem = context.Personagens.FirstOrDefault(x => x.Id == itemLoja.personagemId);
                    if (personagem != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n {personagem.Name} adquirido com sucesso!");
                        Console.ResetColor();
                    }
                }
                else
                {
                    var existente = context.InventarioItens.FirstOrDefault(p => p.UserId == usuario.Id && p.ItemId == itemLoja.personagemId);
                    var personagemNew = new ItemInventario
                    {
                        UserId = usuario.Id,
                        ItemId = itemLoja.personagemId,
                    };
                    context.InventarioItens.Add(personagemNew);
                    Item? personagem = context.Itens.FirstOrDefault(x => x.Id == itemLoja.personagemId);
                    if (personagem != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n {personagem.Name} adquirido com sucesso!");
                        Console.ResetColor();
                    }
                }

                itemLoja.comprado = true;
                context.SaveChanges();
            }

        }


    }
}