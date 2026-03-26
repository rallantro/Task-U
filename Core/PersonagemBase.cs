#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Todo_Gacha.Services;
using Todo_Gacha.Models;
using Todo_Gacha.Data;
using Todo_Gacha.Migrations;

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

        private int _HpAtual;
        
        [NotMapped]
        public int chanceAlvo {get; set;}

        [NotMapped]
        public InimigoBase? inimigoAlvo {get; set;}

        [NotMapped]
        public virtual int HpAtual {get { return _HpAtual; } set { _HpAtual = Math.Max(0, Math.Min(value, HpMax));}}
        [NotMapped]
        public int Shield {get; set;}
        [NotMapped]
        public int BuffAtk {get; set;}

        [NotMapped]
        public int BuffMod {get; set;}

        [NotMapped]
        public int TurnoStun {get; set;} =0;

        [NotMapped]
        public int TurnoSilence {get; set;} =0;
        public int Mod {get; set;}

        [NotMapped]
        public User? user {get; set;}

        [NotMapped]
        public PersonagemBase? aliado {get; set;}

        public required string SummonQuote { get; set; }

        public int ModTotal() {
            Item? itemEquipado = null;

            if(this == user?.Slot1_PersonagemAtivo) itemEquipado = user.Slot1_ItemAtivo;
            else if(this == user?.Slot2_PersonagemAtivo) itemEquipado = user.Slot2_ItemAtivo;

            if (itemEquipado != null && itemEquipado.Atr == 3)
            {
                return Math.Max(0,Mod + BuffMod + itemEquipado.Mod);
            }
            else
            {
                return Math.Max(0,Mod + BuffMod);   
            }
            
        }

        public int AtkTotal(){
            Item? itemEquipado = null;

            if(this == user?.Slot1_PersonagemAtivo) itemEquipado = user.Slot1_ItemAtivo;
            else if(this == user?.Slot2_PersonagemAtivo) itemEquipado = user.Slot2_ItemAtivo;

            if (itemEquipado != null && itemEquipado.Atr == 2)
            {
                return Math.Max(0,Mod + BuffAtk + itemEquipado.Mod);
            }
            else
            {
                return Math.Max(0,Mod + BuffAtk);   
            }
        }

        public virtual int Damage()
        {
            return AtkTotal();
        }

        public virtual void tomarDano(string inimigo, int dano)
        {
            int danoTotal = Math.Max(0, dano - Shield);
            int danoShield = Math.Min(Shield, dano);
            Shield -= danoShield;
            HpAtual = Math.Max(0, HpAtual -= danoTotal);
            if (danoShield > 0 && danoTotal == 0)
            {
                Console.WriteLine($"{Name} bloqueou completamente o ataque de {inimigo} com seu escudo!");
            }
            else
            {
                Console.WriteLine($"{inimigo} atacou {Name} e causou {danoTotal} de dano!");
            }
        }

        public virtual void curar(string aliado, int cura)
        {
            HpAtual = Math.Min(HpAtual + cura, HpMax);;
        }

        public virtual void Habilidade()
        {
                
        }

        public virtual void Passiva()
        {
            
        }
    }
}