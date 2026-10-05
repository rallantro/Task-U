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
        private bool Usou = false;

        public override int Damage()
        {
            double modificador = Nodes >= 5 ? 0.588 : 0.385;
            int cura = (int)Math.Ceiling(AtkTotal() * modificador);
            HpAtual = Math.Min(HpAtual + cura, HpMax);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"> [COSTURA DA ALMA] {Name} puxa um fio invisível de seu peito e o amarra ao boneco.");
            Console.WriteLine($"> {Name}: Só mais um pouquinho... a dor vai passar...");
            Console.WriteLine($"> O boneco absorve a agonia e devolve vida. (+{cura} HP)");
            Console.ResetColor();
            return base.Damage();
        }

        public override void Habilidade()
        {
            if (HpAtual > 1 && cd == 0)
            {
                double modificador = Nodes >= 4 ? 0.084 : 0.108;
                int custo = (int)Math.Ceiling(HpAtual * modificador);
                HpAtual = HpAtual - custo;
                modificador = Nodes >= 3 ? ModTotal() / (ModTotal() + 3.54) : ModTotal() / (ModTotal() + 5.37);
                var buffRes = new BuffRes("Campo Putrefe", 4, modificador);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [CAMPO PUTREFE] {Name} corta o próprio pano e deixa a alma escorrer como fumaça.");
                Console.WriteLine($"> {Name}: Vocês não precisam ver isso... eu já estou acostumado.");
                Console.WriteLine($"> A névoa da alma de {Name} envolve o campo. (Custo: {custo} HP)");
                Console.ResetColor();

                if (aliado != null)
                {
                    modificador = Nodes >= 3 ? ModTotal() / (ModTotal() + 3.54) : ModTotal() / (ModTotal() + 5.37);
                    BuffRes buffResAl = new BuffRes("Campo Putrefe", 4, modificador);
                    buffResAl.Aplicar(aliado);
                    aliado.status.Add(buffResAl);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"> [VÍNCULO DE ALMA] {Name} estende a mão e toca a sombra de {aliado.Name}.");
                    Console.WriteLine($"> {Name}: Eu seguro sua alma... prometo que não vou deixar ela cair.");
                    Console.WriteLine($"> {aliado.Name} recebeu {buffRes.Name}. (Recebe -{modificador * 100:F1}% de dano por 3 turnos)");
                    Console.ResetColor();
                }
                buffRes.Aplicar(this);
                status.Add(buffRes);
                cd = Nodes >= 2 ? 4 : 5;
            }

            if (Nodes >= 6 && aliado != null && aliado.HpAtual <= 0 && !Usou)
            {
                int diff = HpAtual - (int)Math.Ceiling(HpAtual * 0.50);
                HpAtual = diff;
                aliado.HpAtual = diff;
                Usou = true;
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"> [TROCA DE ALMAS] {Name} arranca metade de sua própria alma e a oferece ao boneco.");
                Console.WriteLine($"> {Name}: Eu já morri uma vez... você não precisa passar por isso agora.");
                Console.WriteLine($"> [VÍNCULO PARTILHADO] {Name} e {aliado.Name} agora compartilham {diff} HP cada. A alma de {Name} está dividida.");
                Console.ResetColor();
            }
        }

        public override void Passiva()
        {
            if (status.Any(x => x.Name == "Campo Putrefe"))
            {
                double modificador = Nodes >= 1 ? 0.0245 : 0.0157;
                int cura = (int)Math.Ceiling(HpMax * modificador);
                HpAtual = Math.Min(HpAtual + cura, HpMax);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [PUTREFAÇÃO ABSORVIDA] O boneco de {Name} absorve a podridão do campo e a devolve como vida.");
                Console.WriteLine($"> {Name}: O boneco está quentinho... é como se eu ainda estivesse vivo. (+{cura} HP)");
                Console.ResetColor();
            }
            if (cd > 0)
            {
                cd--;
            }
        }

        public override void Resetar()
        {
            cd = 0;
            Usou = false;
            base.Resetar();
        }
    }
}