using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core.Entities
{
    public class Princess : PersonagemBase
    {
        private bool form;
        private int contadorAnimais;

        public override int Damage()
        {
            if (form)
            {
                
            }
            return base.Damage();
        }

        public override void Habilidade()
        {
            form = !form;
            base.Habilidade();
        }

        public override void Passiva()
        {
            if (form)
            {
                BuffAtk += Mod;
                BuffSpeed += (int)Math.Ceiling(SpeedTotal() * 0.25);    
            }
            else
            {
                if (inimigoAlvo != null)
                {
                    BuffAtk += (int)Math.Ceiling(40.0 * (1 - inimigoAlvo.HpAtual / inimigoAlvo.HpMax));   
                }
                BuffSpeed -= (int)Math.Ceiling(SpeedTotal() * 0.25);
            }
            base.Passiva();
        }
        public override void Resetar()
        {
            base.Resetar();
        }
    }
}