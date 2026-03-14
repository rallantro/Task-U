using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Data;
using Todo_Gacha.Core;
using Todo_Gacha.Services;

namespace Todo_Gacha.Services
{
    public class CreateService
    {
        public void CreateCharacter(AppDbContext context)
        {
            var Wolf = new InimigoBase();
            Wolf.Name = "Lobo";
            Wolf.Desc = "Um lobo cinzento e feio";
            Wolf.DeathQuote = "Auuuuuuu...";
            Wolf.CrystalDrop = 1;
            Wolf.Atk = 2;
            Wolf.Hp = 100;
            Wolf.Mod = 5;
            Wolf.HabilidadeChance = 0;
            context.Inimigos.Add(Wolf);
            context.SaveChanges();
        }
    }
}