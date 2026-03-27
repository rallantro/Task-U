using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Data;
using Task_U.Core;
using Task_U.Services;
using Task_U.Core.Entities;

namespace Task_U.Services
{
    public class CreateService
    {
        public void CreateCharacter(AppDbContext context)
        {
            var Banshee = new Banshee
            {
                Name = "Banshee Cibernética",
                Desc = $"Uma mulher com um esgui corpo branco e translúcido, usando um longo vestido branco que parece se dissolver em glitches no ar. Seu cabelo flutua como se estivesse embaixo d'água, com um leve chiado de televisão de fundo. Seu olhar é vazio e vermelho, com longas garras e dentes afiados. Essa mulher desesperada está pronta para gritar",
                HpMax = 200,
                Rarity = 2,
                Atk = 5,
                Mod = 3,
                CrystalDrop = 2,
                DeathQuote = "KYYYYYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA!"
            };
            context.Inimigos.Add(Banshee);
            context.SaveChanges();
            
        }
    }
}