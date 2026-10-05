using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Services;
using Task_U.Models;
using Task_U.Data;
using Task_U.Core.StatusEffects;

namespace Task_U.Core
{
    public class Voodo : PersonagemBase
    {
        private int cd;

        public override int Damage()
        {
            double modificador = Nodes >= 5 ? 0.64 : 0.52;
            int cura = (int)Math.Ceiling(AtkTotal() * modificador);
            HpAtual = Math.Min(HpAtual + cura, HpMax);
            return base.Damage();
        }

        public override void Habilidade()
        {
            if (HpAtual > 1 && cd == 0)
            {
                double modificador = Nodes >= 4 ? 0.12 : 0.24;
                HpAtual = HpAtual - (int)Math.Ceiling(HpAtual * modificador);
                modificador = Nodes >= 3 ? ModTotal() / (ModTotal() + 3.54) : ModTotal() / (ModTotal() + 5.37);
                var buffRes = new BuffRes("Campo Putrefe", 3, modificador);
                if(aliado != null)
                {
                    aliado.status.Add(buffRes);
                }
                status.Add(buffRes);
                cd = Nodes >= 2 ? 4 : 5;
            }
        }

        public override void Passiva()
        {
            if (status.Any(x => x.Name == "Campo Putrefe"))
            {
                double modificador = Nodes >= 1 ? 0.05 : 0.02;
                int cura = (int)Math.Ceiling(HpMax * modificador);
                HpAtual = Math.Min(HpAtual + cura, HpMax);
            }
            if (cd > 0)
            {
                cd--;
            }
        }
        public override void Resetar()
        {
            base.Resetar();
        }
    }
}