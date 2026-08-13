using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core
{
    public class PersonagemInventario
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PersonagemId { get; set; }
        public int Quantidade {get; set;}
        public int NodesNivel {get; set;}
    }
}