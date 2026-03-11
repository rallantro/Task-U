using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Todo_Gacha.Models
{
    public class Tarefa
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Dif { get; set; } 
        public bool IsDone { get; set; }
        
    }
}