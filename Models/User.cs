using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Core;

namespace Todo_Gacha.Models
{
    public class User
    {
        public int Id { get; set; }
        public int Crystals { get; set; }
        public int PityLeg{ get; set; }
        public int PityEpic { get; set; }

        public DateTime lastLogin {get; set;}
        public DateTime LastBannerUpdate {get; set;}
        public int? Slot1_PersonagemAtivoId { get; set; }
        public int? Slot2_PersonagemAtivoId { get; set; }

        public PersonagemBase? Slot1_PersonagemAtivo {get; set;}
        public PersonagemBase? Slot2_PersonagemAtivo {get; set;}
        public int? ItemAtivoId { get; set; }
        public Item? ItemAtivo {get; set;}

        public int? InimigoId {get; set;}
        public bool? DerrotouInimigo {get; set;}
    }
}