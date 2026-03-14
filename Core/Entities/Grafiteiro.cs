using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Services;
using Todo_Gacha.Models;
using Todo_Gacha.Data;

namespace Todo_Gacha.Core
{
    public class Grafiteiro : PersonagemBase
    {
        private int BonusDMG;
        private int QuantDmg;
        private bool Paint;

        public override int Damage()
        {
            int dano = Atk;
            if (!Paint)
            {
              dano = (Atk + BonusDMG) * QuantDmg;
            }
            return dano;
        }

        public override void Habilidade()
        {
            Paint = !Paint;  
        }

        public override void Passiva(User user)
        {
            if (Paint)
            {
                BonusDMG += Mod;
                QuantDmg +=1;
            }
            else
            {
                QuantDmg = 0;
            }
        }
    }
}
