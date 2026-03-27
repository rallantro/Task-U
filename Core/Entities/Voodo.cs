using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Services;
using Task_U.Models;
using Task_U.Data;

namespace Task_U.Core
{
    public class Voodo : PersonagemBase
    {
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
                int dano = Atk + ((ModTotal() + VoodoDmg) * 2);
                VoodoLife = 0;
                VoodoDmg = 0;
                VoodoDone = false;
                VoodoReady = false;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> É hora da retrubição... {Name}: 'Toda dor volta para o dono.' ({dano} de dano causado)");
                Console.ResetColor();
                return dano;
            }
            else
            {
                Console.WriteLine($"{Name} causou {Atk} de dano!");
            }
            return Atk;
        }

        public override void Habilidade()
        {
            if (VoodoLife > HpAtual)
            {
                int interHp = HpAtual;
                HpAtual = VoodoLife;
                VoodoLife = interHp;  
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [HABILIDADE] {Name}: 'O boneco está mais vivo que eu agora.' (Vínculos alterados)");
                Console.WriteLine($"> [HABILIDADE] Agora {Name} está com {HpAtual} pontos de vida.");
                Console.ResetColor(); 
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [HABILIDADE] {Name}: 'Não... eu ainda não sofri o bastante.'");
                Console.WriteLine($"{Name} não pode substituir sua vida por uma inferior em seu boneco.");
                Console.ResetColor();
            } 
        }

        public override void Passiva()
        {
            if (!calculou)
            {
                Chances = 3;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [PASSIVA] {Name} possui {Chances} chances de vincular sua alma com o seu boneco voodo.");
                Console.ResetColor();
                calculou = true;
            }
            if (!VoodoDone)
            {
                VoodoLife = HpAtual;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [PASSIVA] {Name} sussurrou sua vida para o boneco. {VoodoLife} pontos de vida armazenados.");
                Console.ResetColor();
                VoodoDone = true;
            }
            if (HpAtual <= VoodoLife/5 && Chances > 0)
            {
                VoodoDmg = VoodoLife - HpAtual + ModTotal();
                HpAtual = VoodoLife * 2/3;
                VoodoReady = true;
                Chances--;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [PASSIVA] {Name}: 'O boneco ainda tem fôlego... e eu também.' (Vínculo consumido. Restam {Chances})");
                Console.WriteLine($"> [PASSIVA] {Name} agora está com {HpAtual} de pontos de vida.");
                Console.ResetColor();
            }
            if (Chances == 0)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [PASSIVA] O boneco de {Name} apodreceu. O [Espasmo Cadavérico] não pode mais ser invocado");
                Console.ResetColor();
            }
        }
    }
}