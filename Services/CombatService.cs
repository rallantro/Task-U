#nullable disable
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
        private readonly Random rand = new Random();
        public void Combate(InimigoBase inimigo, List<PersonagemBase> equipe, AppDbContext context, AdventureService adventure)
        {
            var user = context.Users.Find(1);
            var x = 1;
            var inventario = new InventarioServices();

            Console.WriteLine("Atenção para o combate!");
            Console.WriteLine($"Seu inimigo será {inimigo.Name}");
            Console.WriteLine("--");
            Console.WriteLine(inimigo.Desc);
            Console.ReadKey();
            inimigo.HpAtual = inimigo.HpMax;
            foreach (var personagem in equipe)
            {
                personagem.user = user;
                personagem.HpAtual = personagem.HpMax;
                personagem.chanceAlvo = 50;
                var aliado = equipe.FirstOrDefault(x => x.Id != personagem.Id);
                personagem.aliado = aliado;

            }
            

            while (equipe.Any(x => x.HpAtual > 0) && inimigo.HpAtual > 0)
            {
                inimigo.BuffAtk = 0;
                foreach (var personagem in equipe)
                {
                    personagem.BuffAtk = 0;
                    personagem.inimigoAlvo = inimigo;
                }
                Console.Clear();
                foreach (var personagem in equipe)
                {
                    
                    if (inimigo.HpAtual <= 0)
                    {
                        break;
                    }
                    if (personagem.HpAtual <= 0)
                    {
                        continue;
                    }
                    Console.Clear();
                    Cabecalho(equipe, inimigo, x);
                    personagem.Passiva();
                    Console.ReadKey();
                    //Turno do Player
                    bool Atacou = false;
                    bool UsouHabilidade = false;
                    bool UsouItem = false;
                    bool MenuShow = true;
                    Console.ForegroundColor = ConsoleColor.White;
                    


                    while (MenuShow)
                    {
                        Console.Clear();
                        Cabecalho(equipe, inimigo, x);
                        Console.WriteLine($"\n --- TURNO DE {personagem.Name} ---");
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
                                    inimigo.tomarDano(personagem.Name, dano);
                                    Atacou = true;  
                                }
                                Console.ReadKey();                   
                            break;
                            case "2":
                                if (!UsouHabilidade)
                                {
                                    personagem.Habilidade();
                                    UsouHabilidade = true;
                                }
                                Console.ReadKey();
                            break;
                            case "3":
                                Console.Clear();
                                Cabecalho(equipe, inimigo, x);
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
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.WriteLine("Opção Inválida!");
                                }
                                                                                        
                            break;
                            case "4":
                                Console.WriteLine("Seu turno acabou.");
                                Console.ReadKey();
                                MenuShow = false;
                            break;
                            
                        }
                    }
                
                    }

                if(inimigo.HpAtual > 0 && equipe.Any(x => x.HpAtual > 0))
                {
                    Console.Clear();
                    Cabecalho(equipe, inimigo, x);
                    inimigo.Passiva(user); 
                    var vivos = equipe.Where(p => p.HpAtual > 0).ToList();
                    if(vivos.Count() == 0)
                    {
                        continue;
                    }
                    inimigo.alvos = vivos;
                    inimigo.Habilidade();
                    int danoInimigo = inimigo.Damage();
                    
                    int chanceTotal = 0;
                    foreach (var personagem in vivos)
                    {
                        chanceTotal += personagem.chanceAlvo;
                    }
                    int chance = rand.Next(0, chanceTotal);
                    PersonagemBase alvo = vivos.FirstOrDefault();
                    foreach (var personagem in vivos)
                    {
                        if (chance < personagem.chanceAlvo)
                        {
                            alvo = personagem;
                            break;
                            
                        }
                        chance -= personagem.chanceAlvo;
                    }
                    alvo.tomarDano(inimigo.Name, danoInimigo);
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
                user.DerrotouInimigo = true;
                adventure.AtualizarInimigo(context);
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.White;
                context.SaveChanges();
            }
        }
        private void Cabecalho(List<PersonagemBase> equipe, InimigoBase inimigo, int turno)
        {
            Console.WriteLine($"================== TURNO {turno} ==================");
            foreach (var personagem in equipe)
            {
                if (personagem.HpAtual <= 0) Console.ForegroundColor = ConsoleColor.DarkRed;
                personagem.HpAtual = Math.Min(personagem.HpAtual, personagem.HpMax);
                Console.WriteLine($" {personagem.Name} | HP: {personagem.HpAtual}");   
                Console.ResetColor();
            }
            inimigo.HpAtual = Math.Min(inimigo.HpAtual, inimigo.HpMax);
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine($" INIMIGO: {inimigo.Name.ToUpper()} | HP: {inimigo.HpAtual}/{inimigo.HpMax}");
            Console.WriteLine("===================================================");
        }
    } 
}