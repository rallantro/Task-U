using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Models
{
    public class Loja
    {
        public int id { get; set; }
        public int personagemId { get; set; }
        public int preco { get; set; }
        public bool comprado { get; set; }
        public bool personagem {get; set;}
    }
}