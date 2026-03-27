using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Services;
using System.ComponentModel.DataAnnotations.Schema;
using Task_U.Models;
using Task_U.Data;
using Task_U.Core;

namespace Task_U.Core
{
    public class InimigoBase
    {
        public int Id { get; set; }
        public required string Name {get; set;}
        public required string Desc {get; set;}
        public int Atk {get; set;}
        public int HpMax {get; set;}
        public int Mod {get; set;}
        public int HabilidadeChance { get; set; }
        public int CrystalDrop { get; set; }
        public int? ItemDropId { get; set; }
        public string? DeathQuote { get; set; }
        public int Rarity {get; set;}
        private int _HpAtual;

        [NotMapped]
        public Random rand = new Random();

        [NotMapped]
        public int HpAtual {get { return _HpAtual; } set { _HpAtual = Math.Max(0, Math.Min(value, HpMax));}}
        [NotMapped]
        public int Shield {get; set;}

        [NotMapped]
        public int BuffAtk{get; set;}

        [NotMapped]
        public virtual int TurnoStun {get; set;} = 0;

        [NotMapped]
        public int TurnoSilence {get; set;} = 0;
        
        [NotMapped]
        public List<PersonagemBase> ?alvos {get; set;}


        public virtual void tomarDano(PersonagemBase inimigo, int dano)
        {
            int danoTotal = Math.Max(0, dano - Shield);
            int danoShield = Math.Min(Shield, dano);
            Shield -= danoShield;
            HpAtual -= danoTotal;
            if (danoShield > 0 && danoTotal == 0)
            {
                Console.WriteLine($"{Name} bloqueou completamente o ataque de {inimigo.Name} com seu escudo!");
            }
            else
            {
                Console.WriteLine($"{inimigo.Name} atacou {Name} e causou {danoTotal} de dano!");
            }
        }

        public virtual int Damage()
        {
            return Atk + BuffAtk;
        }

        public virtual void Habilidade()
        {
                
        }

        public virtual void Passiva(User user)
        {
            
        }
    }
}