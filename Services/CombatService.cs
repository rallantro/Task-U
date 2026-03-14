using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Data;
using Todo_Gacha.Core;
using Todo_Gacha.Services;
using System.Diagnostics;

namespace Todo_Gacha.Services
{
    public class CombatService
    {
        public void Combate(InimigoBase inimigo, PersonagemBase personagem)
        {
            using var context = new AppDbContext();  
            var user = context.Users.Find(1);
            var x = 1;

            Console.WriteLine("Atenção para o combate!");
            Console.WriteLine($"Seu inimigo será {inimigo.Name}");
            Console.WriteLine("--");
            Console.WriteLine(inimigo.Desc);
            Thread.Sleep(1500);

            while (personagem.Hp > 0 && inimigo.Hp > 0)
            {
                Console.Clear();
                Console.WriteLine($"---- TURNO {x} ----");
                personagem.Passiva(user);
                Console.WriteLine($"{personagem.Name}: {personagem.Hp}");
                Console.WriteLine($"{inimigo.Name}: {inimigo.Hp}");
                //Turno do Player
                bool Atacou = false;
                bool UsouHabilidade = false;
                bool MenuShow = true;
                Console.ForegroundColor = ConsoleColor.White;
                


                while (MenuShow)
                {
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
                    Console.WriteLine("4 - Encerrar Turno");
                    Console.WriteLine($"---- // ----");
                    switch (Console.ReadLine())
                    {
                        case "1" :
                            if (!Atacou)
                            {
                                int dano = personagem.Damage();
                                inimigo.Hp -= dano;
                                Atacou = true;  
                            }                    
                        break;
                        case "2":
                            if (!UsouHabilidade)
                            {
                                personagem.Habilidade();
                                UsouHabilidade = true;
                            }
                        break;
                        case "4":
                            Console.WriteLine("Seu turno acabou.");
                            MenuShow = false;
                        break;
                        
                    }
                }

                if(inimigo.Hp > 0)
                {
                    // Turno do Inimigo 
                    inimigo.Passiva(user); 
                    int danoInimigo = inimigo.Damage();
                    personagem.Hp -= danoInimigo;
                    Console.WriteLine($"{inimigo.Name} atacou e causou {danoInimigo} de dano!");

                    Console.WriteLine("\nPressione qualquer tecla para o próximo turno...");
                    Console.ReadKey();
                    x++;
                }
                
            }

            if (inimigo.Hp <= 0)
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
    }
}