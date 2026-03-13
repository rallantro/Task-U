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
        public void AtualizarBanner()
        {
            
        }
        public PersonagemBase EpicPull(AppDbContext context)
        {
            var reward = context.Personagens.Find(2);
            return reward;
        }
    }
}