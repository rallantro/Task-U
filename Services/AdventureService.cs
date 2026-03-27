#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task_U.Data;
using Task_U.Core;
using Task_U.Services;
using System.Diagnostics;

namespace Task_U.Services
{
    public class AdventureService
    {
        private readonly Random rand = new Random();
        public void AtualizarInimigo(AppDbContext context)
        {
            var hoje = DateTime.Now.Date;
            var user = context.Users.Find(1);

            if (user.lastLogin.Date < hoje.Date)
            {
                var Sorte = rand.Next(0,100);
                int Rarity = 1;
                if (Sorte <= 4)
                {
                    Rarity = 4;
                } else if (Sorte <= 20)
                {
                    Rarity = 3;
                }else if (Sorte <= 50)
                {
                    Rarity = 2;
                }else
                {
                    Rarity = 1;
                }
                var inimigoSorteado = context.Inimigos.Where(x => x.Rarity == Rarity).OrderBy(x => EF.Functions.Random()).FirstOrDefault() ?? context.Inimigos.Find(1);
                user.InimigoId = inimigoSorteado.Id;
                context.SaveChanges();
            } else if (user.DerrotouInimigo == true)
            {
                var InimigoOld = context.Inimigos.Find(user.InimigoId);
                int rarityNew;

                if (InimigoOld.Rarity <= 2)
                {
                    var Sorte = rand.Next(0,2);
                    rarityNew =  InimigoOld.Rarity + Sorte; 
                } else if (InimigoOld.Rarity == 3)
                {
                    var Sorte = rand.Next(0,100);
                    if (Sorte <= 10)
                    {
                        rarityNew =  4; 
                    }
                    else
                    {
                        rarityNew = 3;
                    }                   
                }
                else
                {
                    rarityNew = 1;
                }

                var inimigoSorteado = context.Inimigos.Where(x => x.Rarity == rarityNew).OrderBy(x => EF.Functions.Random()).FirstOrDefault() ?? context.Inimigos.Find(1);
                user.InimigoId = inimigoSorteado.Id;
                user.DerrotouInimigo = false;
                context.SaveChanges();              
            }  
        } 
        
    }
}