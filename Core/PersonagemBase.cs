#nullable enable
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
        public required string Name {get; set;}
        public required string Desc {get; set;}
        public int Rarity {get; set;}
        public int Atk {get; set;}
        public int HpMax {get; set;}
        
        [NotMapped]
        public int chanceAlvo {get; set;}

        [NotMapped]
        public int HpAtual {get; set;}
        [NotMapped]
        public int Shield {get; set;}
        [NotMapped]
        public int BuffAtk {get; set;}

        [NotMapped]
        public int BuffMod {get; set;}
        public int Mod {get; set;}

        [NotMapped]
        public required User user {get; set;}

        [NotMapped]
        public PersonagemBase? aliado {get; set;}

        public required string SummonQuote { get; set; }

        public int ModTotal() => (user?.ItemAtivo?.Atr == 3) ? Mod + BuffMod + user.ItemAtivo.Mod : Mod + BuffMod;

        public int AtkTotal() => (user?.ItemAtivo?.Atr == 2) ? Atk + BuffAtk + user.ItemAtivo.Mod : Atk + BuffAtk;

        public virtual int Damage()
        {
            return AtkTotal();
        }

        public virtual void tomarDano(string inimigo, int dano)
        {
            int danoTotal = Math.Max(0, dano - Shield);
            int danoShield = Math.Min(Shield, dano);
            Shield -= danoShield;
            HpAtual -= danoTotal;
            if (danoShield > 0 && danoTotal == 0)
            {
                Console.WriteLine($"{Name} bloqueou completamente o ataque de {inimigo} com seu escudo!");
            }
            else
            {
                Console.WriteLine($"{inimigo} atacou {Name} e causou {danoTotal} de dano!");
            }
        }

        public virtual void Habilidade()
        {
                
        }

        public virtual void Passiva()
        {
            
        }
    }
}