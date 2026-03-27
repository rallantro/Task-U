using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Core;

namespace Task_U.Models
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
        public int? Slot1_ItemAtivoId { get; set; }
        public Item? Slot1_ItemAtivo {get; set;}
        public int? Slot2_ItemAtivoId { get; set; }
        public Item? Slot2_ItemAtivo {get; set;}

        public int? InimigoId {get; set;}
        public bool? DerrotouInimigo {get; set;}
    }
}