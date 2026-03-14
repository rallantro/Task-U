using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Services;
using System.ComponentModel.DataAnnotations.Schema;
using Todo_Gacha.Models;
using Todo_Gacha.Data;

namespace Todo_Gacha.Core
{
    public class InimigoBase
    {
        public int Id { get; set; }
        public string Name {get; set;}
        public string Desc {get; set;}
        public int Atk {get; set;}
        public int HpMax {get; set;}
        public int Mod {get; set;}
        public int HabilidadeChance { get; set; }
        public int CrystalDrop { get; set; }
        public int? ItemDropId { get; set; }
        public string? DeathQuote { get; set; }
        public int Rarity {get; set;}

        [NotMapped]
         public int HpAtual {get; set;}

        public virtual int Damage()
        {
            return Atk;
        }

        public virtual void Habilidade()
        {
                
        }

        public virtual void Passiva(User user)
        {
            
        }
    }
}