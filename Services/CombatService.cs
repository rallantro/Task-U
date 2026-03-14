using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo_Gacha.Data;
using Todo_Gacha.Core;
using Todo_Gacha.Services;
using System.Diagnostics;

namespace Todo_Gacha.Services
{
    public class CombatService
    {
        public void Combate(InimigoBase inimigo, PersonagemBase personagem, AppDbContext context)
        {
            var user = context.Users.Find(1);
            personagem.user = user;
            var x = 1;
            var inventario = new InventarioServices();

            Console.WriteLine("Atenção para o combate!");
            Console.WriteLine($"Seu inimigo será {inimigo.Name}");
            Console.WriteLine("--");
            Console.WriteLine(inimigo.Desc);
            Thread.Sleep(1500);
            personagem.HpAtual = personagem.HpMax;
            inimigo.HpAtual = inimigo.HpMax;

            while (personagem.HpAtual > 0 && inimigo.HpAtual > 0)
            {
                Console.Clear();
                personagem.Passiva();
                Cabecalho(personagem, inimigo, x);
                //Turno do Player
                bool Atacou = false;
                bool UsouHabilidade = false;
                bool UsouItem = false;
                bool MenuShow = true;
                Console.ForegroundColor = ConsoleColor.White;
                


                while (MenuShow)
                {
                    Console.Clear();
                    Cabecalho(personagem, inimigo, x);
                    Console.WriteLine($"---- // ----");
                    Console.WriteLine($"O que você deseja fazer?");
                    if (Atacou)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("1 - Ataque Básico");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine("1 - Ataque Básico");
                    }
                    if (UsouHabilidade)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("2 - Habilidade Especial");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine("2 - Habilidade Especial");
                    }
                    if (UsouItem)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("3 - Item");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine("3 - Item");
                    }
                    Console.WriteLine("4 - Encerrar Turno");
                    Console.WriteLine($"---- // ----");
                    switch (Console.ReadLine())
                    {
                        case "1" :
                            if (!Atacou)
                            {
                                int dano = personagem.Damage();
                                inimigo.HpAtual -= dano;
                                Atacou = true;  
                            }
                            Thread.Sleep(1000);                    
                        break;
                        case "2":
                            if (!UsouHabilidade)
                            {
                                personagem.Habilidade();
                                UsouHabilidade = true;
                            }
                            Thread.Sleep(1000);
                        break;
                        case "3":
                            Console.Clear();
                            Cabecalho(personagem, inimigo, x);
                            var consumiveis = new List<Item>();
                            inventario.MostrarItens(context, true, consumiveis, 1); 
                            if (consumiveis.Count == 0) 
                            {
                                Console.WriteLine("Você não tem itens consumíveis!");
                                break;
                            } 
                            Console.WriteLine("--");
                            Console.WriteLine($"Qual item deseja usar?");
                        
                            var num = 0;
                            var sucesso = int.TryParse(Console.ReadLine(), out num);
                            if (sucesso && num > 0 && num <= consumiveis.Count())
                            {
                                var itemEscolhido = consumiveis[num - 1];
                                Console.WriteLine("--");
                                Console.WriteLine($"{personagem.Name} usou {itemEscolhido.Name}!");
                                itemEscolhido.Uso(personagem, context);
                                UsouItem = true;
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                Console.WriteLine("Opção Inválida!");
                            }
                                                                                    
                        break;
                        case "4":
                            Console.WriteLine("Seu turno acabou.");
                            Thread.Sleep(1000);
                            MenuShow = false;
                        break;
                        
                    }
                }

                if(inimigo.HpAtual > 0)
                {
                    // Turno do Inimigo 
                    inimigo.Passiva(user); 
                    int danoInimigo = inimigo.Damage();
                    personagem.HpAtual -= danoInimigo;
                    Console.WriteLine($"{inimigo.Name} atacou e causou {danoInimigo} de dano!");

                    Console.WriteLine("\nPressione qualquer tecla para o próximo turno...");
                    Console.ReadKey();
                    x++;
                }
                
            }

            if (inimigo.HpAtual <= 0)
            {
                if (inimigo.DeathQuote != null)
                {
                   Console.WriteLine($"{inimigo.Name} grunhe antes de morrer:");
                   Console.WriteLine($"'{inimigo.DeathQuote}'"); 
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("--");
                Console.WriteLine($"Parabéns Você derrotou {inimigo.Name}");
                Console.WriteLine("--");
                if (inimigo.CrystalDrop > 0)
                {
                    user.Crystals += inimigo.CrystalDrop;
                    Console.WriteLine($"Você recebeu {inimigo.CrystalDrop} Crystals!");
                }
                if (inimigo.ItemDropId != null)
                {
                    var item = context.Itens.Find(inimigo.ItemDropId);
                    var reward = new ItemInventario();
                    reward.ItemId = item.Id;
                    reward.UserId = user.Id;
                    context.InventarioItens.Add(reward);
                    Console.WriteLine($"Você recebeu {item.Name}!");
                }
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.White;
                context.SaveChanges();
            }
        }
        private void Cabecalho(PersonagemBase personagem, InimigoBase inimigo, int turno)
        {
            Console.WriteLine($"---- TURNO {turno} ----");
            personagem.HpAtual = Math.Min(personagem.HpAtual, personagem.HpMax);
            inimigo.HpAtual = Math.Min(inimigo.HpAtual, inimigo.HpMax);
            Console.WriteLine($"{personagem.Name}: {personagem.HpAtual}");
            Console.WriteLine($"{inimigo.Name}: {inimigo.HpAtual}");
        }
    } 
}