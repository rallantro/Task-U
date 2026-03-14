using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Data;
using Todo_Gacha.Core;
using Todo_Gacha.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Todo_Gacha.Services
{
    public class BannerService
    {
        public PersonagemBase rateUpEpic;
        public PersonagemBase rateUpLeg;
        private static readonly Random random = new Random();

        public void AtualizarBanner(AppDbContext context)
        {
            var hoje = DateTime.Now.Date;
            var user = context.Users.Find(1);

            if(user.lastLogin.Date >= user.LastBannerUpdate.AddDays(7))
            {
                rateUpEpic = context.Personagens.Where(x => x.Rarity == 3).OrderBy(x => EF.Functions.Random()).FirstOrDefault();
                rateUpLeg = context.Personagens.Where(x => x.Rarity == 4).OrderBy(x => EF.Functions.Random()).FirstOrDefault();
                var atualBanner = new Banner();
                atualBanner.Id = 1;
                atualBanner.LegId = rateUpLeg.Id;
                atualBanner.EpicId = rateUpEpic.Id;
                context.banners.Update(atualBanner);
                user.LastBannerUpdate = DateTime.Now.Date; 
                context.SaveChanges();
            }
            else
            {
                var atualBanner = context.banners.Find(1);
                rateUpEpic = context.Personagens.Find(atualBanner.EpicId);
                rateUpLeg = context.Personagens.Find(atualBanner.LegId);
            }
            
        }
        public Item commumPull(AppDbContext context)
        {
            Item reward;
            reward = context.Itens.Where(x => x.Rarity == 1).OrderBy(x => EF.Functions.Random()).FirstOrDefault();
            return reward;
        }
        public Item raroPull(AppDbContext context)
        {
            Item reward;
            reward = context.Itens.Where(x => x.Rarity == 2).OrderBy(x => EF.Functions.Random()).FirstOrDefault();
            return reward;
        }
        public PersonagemBase EpicPull(AppDbContext context)
        {
            var chance = random.Next(1, 101);
            PersonagemBase reward;

            if (chance >= 50)
            {
               reward = rateUpEpic; 
            }
            else
            {
                reward = context.Personagens.Where(x => x.Rarity == 3).OrderBy(x => EF.Functions.Random()).FirstOrDefault() ?? rateUpEpic;
            }
            
            return reward;
        }

        public PersonagemBase LegendPull(AppDbContext context)
        {
            var chance = random.Next(1, 101);
            PersonagemBase reward;

            if (chance >= 50)
            {
               reward = rateUpLeg; 
            }
            else
            {
                reward = context.Personagens.Where(x => x.Rarity == 4).OrderBy(x => EF.Functions.Random()).FirstOrDefault() ?? rateUpLeg;
            }
            
            return reward;
        }
    }
}