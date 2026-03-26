#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Todo_Gacha.Data;
using Todo_Gacha.Core;
using Todo_Gacha.Services;

namespace Todo_Gacha.Models
{
    public class Gacha
    {
        public Gacha()
        {
        }
        
        Random random = new Random();

        public int legChance = 10;

        public int maxPityLeg = 100;
        public int maxPityEpic = 10;

        public bool luckEvent = false;

        public void luckyEvent(){
            luckEvent = true;
        }

        public void Pull(BannerService banner){
            using var context = new AppDbContext();  
            int number = random.Next(1, 1001);
            var user = context.Users.Find(1);

            user.Crystals -=10;

            int pityLeg = user.PityLeg;
            int pityEpic = user.PityEpic;

            pityLeg++;
            pityEpic++;

            int currentChance = (pityLeg >= 75) ? legChance + (5 * (pityLeg - 74)) : 10;

            if (luckEvent)
            {
                currentChance*=2;
                luckEvent = false;
            }


            Console.WriteLine("...");
            Thread.Sleep(1500);


            if(number <= currentChance || pityLeg == maxPityLeg)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("...");
                Thread.Sleep(500);
                Console.WriteLine("PULL LEGENDÁRIO! SSR"); 
                Thread.Sleep(500);
                Console.WriteLine("...");
                var ganhou = new PersonagemInventario();
                var reward = banner.LegendPull(context);
                ganhou.PersonagemId = reward.Id;
                ganhou.UserId = 1;
                context.InventarioPersonagens.Add(ganhou);
                Console.WriteLine($"-- {reward.Name.ToUpper()} --");
                Console.WriteLine("-");
                Console.WriteLine(reward.Desc);
                context.SaveChanges(); 
                pityLeg = 0;
                pityEpic = 0;
            }
            else if(number <= 60 || pityEpic == maxPityEpic)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("...");
                Thread.Sleep(500);
                Console.WriteLine("PULL ÉPICO! SR");  
                Thread.Sleep(500);
                Console.WriteLine("...");
                var ganhou = new PersonagemInventario();
                var reward = banner.EpicPull(context);
                ganhou.PersonagemId = reward.Id;
                ganhou.UserId = 1;
                context.InventarioPersonagens.Add(ganhou);
                Console.WriteLine(reward.SummonQuote);
                Console.WriteLine("...");
                Thread.Sleep(1500);
                Console.WriteLine($"-- {reward.Name.ToUpper()} --");
                Console.WriteLine(reward.Desc);
                context.SaveChanges();
                pityEpic = 0;
            }
            else if(number <= 250)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("...");
                Thread.Sleep(500);
                Console.WriteLine("PULL RARO! R");  
                Thread.Sleep(500);
                Console.WriteLine("...");
                var ganhou = new ItemInventario();
                var reward = banner.raroPull(context);
                ganhou.ItemId = reward.Id;
                ganhou.UserId = 1;
                context.InventarioItens.Add(ganhou);
                Console.WriteLine($"Parabéns você ganhou {reward.Name}!!");
                Console.WriteLine(reward.Desc);
                context.SaveChanges();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("...");                
                Thread.Sleep(500);
                Console.WriteLine("COMUM! C");
                Thread.Sleep(500);
                Console.WriteLine("...");
                var ganhou = new ItemInventario();
                var reward = banner.commumPull(context);
                ganhou.ItemId = reward.Id;
                ganhou.UserId = 1;
                context.InventarioItens.Add(ganhou);
                Console.WriteLine($"Parabéns você ganhou {reward.Name}!!");
                Console.WriteLine(reward.Desc);
                context.SaveChanges();
            }  
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine($"Faltam só mais {10 - pityEpic} giros para um épico garantido! Faltam só mais {100-pityLeg} para um Lendário Garantido!");     
        
            user.PityEpic = pityEpic;
            user.PityLeg = pityLeg;
            context.Users.Update(user);
            context.SaveChanges();
        }
    }
    
}