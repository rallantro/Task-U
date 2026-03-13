using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo_Gacha.Models
{
    public class SideQuest
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Dif { get; set; } 
        public bool IsDone { get; set; }
    }
}