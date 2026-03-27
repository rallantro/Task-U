using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;

namespace Task_U.Core
{
    public class Aranha : InimigoBase
    {
        private static readonly Random random = new Random();
        public override int Damage()
        {
            return Atk;
        }

        public override void Habilidade()
        {
            //
        }

        public override void Passiva(User user)
        {
            
        }
    }
}