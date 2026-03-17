using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Data;
using Todo_Gacha.Core;
using Todo_Gacha.Services;
using Todo_Gacha.Core.Entities;

namespace Todo_Gacha.Services
{
    public class CreateService
    {
        public void CreateCharacter(AppDbContext context)
        {
            var fada = new Fada
            {
                Name = "Fada de Nanobots",
                Desc = "Um enchame de nano robôs que se moldam na forma de uma fada, uma esfera brilhante e azul, como vírus de aprimoramento que consome tecnologia e magia.",
                HpMax = 100,
                Rarity = 1,
                Atk = 4,
                CrystalDrop = 1,
                Mod = 10,
                HabilidadeChance = 70,
                DeathQuote = "Prriiii..."
            };
            context.Inimigos.Add(fada);
            context.SaveChanges();

            var QFada = new FadaRa
            {
                Name = "Rainha dos Nanobots",
                Desc = "Protegida no subterrâneo, um aglomerado de nanobots que assumiu uma forma feminina majestosa, capaz de controlar outros enxames. Brilha com luz própria e emite um zumbido harmônico.",
                HpMax = 400,
                Rarity = 4,
                Atk = 10,
                CrystalDrop = 8,
                Mod = 8,
                HabilidadeChance = 70,
                DeathQuote = "Isso... É impossível... Priiiii"
            };
            context.Inimigos.Add(QFada);
            context.SaveChanges();
            
        }
    }
}