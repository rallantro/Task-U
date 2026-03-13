using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public int Hp {get; set;}
        public int Mod {get; set;}

        public string SummonQuote { get; set; }

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