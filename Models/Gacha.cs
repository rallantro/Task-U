using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Todo_Gacha.Data;

namespace Todo_Gacha.Models
{
    public class Gacha
    {
        private AppDbContext context;

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

        public void Pull(){
            using var context = new AppDbContext();  
            int number = random.Next(1, 1001);
            var user = context.Users.Find(1);

            if (user.Crystals < 10) {
                Console.WriteLine("Crystals insuficientes! Vá estudar para ganhar mais.");
                return;
            }

            user.Crystals -=10;

            int pityLeg = user.PityLeg;
            int pityEpic = user.PityEpic;

            pityLeg++;
            pityEpic++;

            int curruentChance = (pityLeg >= 75) ? legChance + (5 * (pityLeg - 74)) : 10;

            if (luckEvent)
            {
                curruentChance*=2;
                luckEvent = false;
            }

            Console.WriteLine("...");
            Thread.Sleep(1500);


            if(number <= curruentChance || pityLeg == maxPityLeg)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("...");
                Thread.Sleep(500);
                Console.WriteLine("PULL LEGENDÁRIO! SSR");  
                pityLeg = 0;
                pityEpic = 0;
            }
            else if(number <= 60 || pityEpic == maxPityEpic)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("...");
                Thread.Sleep(500);
                Console.WriteLine("PULL ÉPICO! SR");  
                pityEpic = 0;
            }
            else if(number <= 250)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("...");
                Thread.Sleep(500);
                Console.WriteLine("PULL RARO! R");  
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("...");                
                Thread.Sleep(500);
                Console.WriteLine("COMUM! C");
            }  
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Faltam só mais {10 - pityEpic} giros para um épico garantido! Faltam só mais {100-pityLeg} para um Lendário Garantido!");     
        
            user.PityEpic = pityEpic;
            user.PityLeg = pityLeg;
            context.Users.Update(user);
            context.SaveChanges();
        }
    }
    
}