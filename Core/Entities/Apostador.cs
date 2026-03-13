using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Services;
using Todo_Gacha.Models;
using Todo_Gacha.Data;

namespace Todo_Gacha.Core.Entities
{
    public class Apostador : PersonagemBase
    {
        private static readonly Random random = new Random();
        public int BaseAtk {get; private set;}

        public override int Damage()
        {
            return Atk + BaseAtk;
        }

        public override void Habilidade()
        {
            int cura = random.Next(0, Mod);
            if (random.Next(0, 100) < 10) 
            {
                this.Hp -= cura; 
            }
            else
            {
                this.Hp += cura;
            }   
        }

        public override void Passiva(User user)
        {
            
            if (user.PityEpic > 5)
            {
                BaseAtk = Atk;
            } else
            {
                BaseAtk = 0;
            }
        }
    }
}