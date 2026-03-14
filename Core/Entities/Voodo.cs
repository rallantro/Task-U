using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Services;
using Todo_Gacha.Models;
using Todo_Gacha.Data;

namespace Todo_Gacha.Core
{
    public class Voodo : PersonagemBase
    {
        private static readonly Random random = new Random();
        private int VoodoLife;
        private bool VoodoDone;
        private int VoodoDmg;
        private bool VoodoReady;
        private int Chances;
        private bool calculou;

        public override int Damage()
        {
            if (VoodoReady)
            {
                int dano = Atk + (Mod + VoodoDmg)/2;
                VoodoLife = 0;
                VoodoDmg = 0;
                VoodoDone = false;
                VoodoReady = false;
                return dano;
            }
            return Atk;
        }

        public override void Habilidade()
        {
            if (VoodoLife < Hp)
            {
                int interHp = Hp;
                Hp = VoodoLife;
                VoodoLife = interHp;   
            }
            else
            {
                
            } 
        }

        public override void Passiva(User user)
        {
            if (!calculou)
            {
                Chances = random.Next(1,5);
                calculou = true;
            }
            if (!VoodoDone)
            {
                VoodoLife = Hp;
                VoodoDone = true;
            }
            if (Hp <= Hp/5 && Chances > 0)
            {
                VoodoDmg = VoodoLife - Hp + Mod;
                Hp = VoodoLife/2;
                VoodoReady = true;
                Chances--;
            }
        }
    }
}