using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Models;
using Todo_Gacha.Data;

namespace Todo_Gacha.Core.Entities
{
    public class Barbaro : PersonagemBase
    {
        private static readonly Random random = new Random();
        public int BaseAtk {get; private set;}

        public int HpMax;

        public override int Damage()
        {
            return Atk + BaseAtk;
        }

        public override void Habilidade()
        {
            Hp -= 4;
            BaseAtk += 5; 
        }

        public override void Passiva(User user)
        {

            BaseAtk = (HpMax - Hp) * Mod/2;
        }
        
    }
}