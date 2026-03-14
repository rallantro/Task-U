using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Todo_Gacha.Services;
using Todo_Gacha.Models;
using Todo_Gacha.Data;

namespace Todo_Gacha.Core
{
    public class PersonagemBase
    {
        public int Id { get; set; }
        public string Name {get; set;}
        public string Desc {get; set;}
        public int Rarity {get; set;}
        public int Atk {get; set;}
        public int HpMax {get; set;}

        [NotMapped]
        public int HpAtual {get; set;}
        public int Mod {get; set;}

        [NotMapped]
        public User user {get; set;}

        public string SummonQuote { get; set; }

        public int ModTotal() => (user?.ItemAtivo?.Atr == 3) ? Mod + user.ItemAtivo.Mod : Mod;

        public int AtkTotal() => (user?.ItemAtivo?.Atr == 2) ? Atk + user.ItemAtivo.Mod : Atk;

        public virtual int Damage()
        {
            return AtkTotal();
        }

        public virtual void Habilidade()
        {
                
        }

        public virtual void Passiva()
        {
            
        }
    }
}