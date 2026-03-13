using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Data;
using Todo_Gacha.Core;

namespace Todo_Gacha.Services
{
    public class BannerService
    {
        public PersonagemBase rateUpEpic;
        public PersonagemBase rateUpLeg;
        private static readonly Random random = new Random();

        public void AtualizarBanner(AppDbContext context)
        {
            rateUpEpic = context.Personagens.Find(2);
        }
        public Item commumPull(AppDbContext context)
        {
            Item reward;
            reward = context.Itens.Where(x => x.Rarity == 1).OrderBy(x => Guid.NewGuid()).FirstOrDefault() ?? new Item { Name = "Papel Amassado", Desc = "Não serve para nada, mas é seu.", Rarity = 1 };
            return reward;
        }
        public PersonagemBase EpicPull(AppDbContext context)
        {
            var chance = random.Next(1, 101);
            PersonagemBase reward;

            if (chance <= 70)
            {
               reward = rateUpEpic; 
            }
            else
            {
                reward = context.Personagens.Where(x => x.Rarity == 3).OrderBy(x => Guid.NewGuid()).FirstOrDefault() ?? rateUpEpic;
            }
            
            return reward;
        }
    }
}