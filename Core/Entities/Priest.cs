using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Update;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Entities
{
    public class Priest : PersonagemBase
    {

        private bool aplicarPassiva = false;
        private bool ressussitou = false;
        private int contador;

        public override int Damage()
        {
            if (inimigoAlvo != null)
            {
                double modificador = Nodes >= 2 ? 0.25 : 0.2;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> [RITO DA NASCENTE] canaliza a água pesada contra {inimigoAlvo.Name}. (Recebe +{modificador * 100}% de dano)");
                Console.WriteLine($"> {Name}: O que é impuro se dissolve na transparência...");
                Console.ResetColor();
                var debuff = new DebuffRes("Rito da Nascente", 1, modificador);
                inimigoAlvo.status.Add(debuff);
                debuff.Aplicar(inimigoAlvo);
            }
            return base.Damage();
        }
        public override void Passiva()
        {
            double modificador = Nodes >= 5 ? 0.15 : 0.1;
            if (!aplicarPassiva)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> [OÁSIS ETERNO] {Name} manifesta sua divindade em campo.");
                Console.WriteLine($"> {Name}: Onde havia areia e esquecimento, agora flui a vida. Bem-vindos ao meu Oásis; aqui, a dor não tem permissão para florescer.");
                Console.ResetColor();
                AplicarAura(this, modificador);
                if (aliado != null) AplicarAura(aliado, modificador);
                aplicarPassiva = true;
            }
            if (!ressussitou && aliado != null && aliado.HpAtual <= 0)
            {

                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> [MILAGRE DO NILO] {Name} resgata a alma de {aliado.Name}");
                Console.WriteLine($"> {Name}: Sob minha guarda, a morte é apenas uma miragem. \u001b[1mLevante-se\u001b[0m e contemple a eternidade do meu oásis.");
                Console.ResetColor();
                modificador = Nodes >= 4 ? 0.40 : 0.20;
                aliado.HpAtual = (int)Math.Ceiling(aliado.HpMax * modificador);
                if (Nodes >= 6 && contador < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> [MILAGRE DO NILO] As águas ainda guardam um último sopro de vida.");
                    Console.ResetColor();
                }
                else
                {
                    ressussitou = true;
                }
                contador++;
            }

        }

        public override void Habilidade()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"> [MANTO DAS ÁGUAS] {Name} cria um escudo feito da mais pura e mágica água de seu oásis.");
            Console.WriteLine($"> {Name}: Sinta o frescor do Nilo... deixe que a água absorva sua dor.");
            double modificador = Nodes >= 3 ? 0.15 : 0.1;
            var shieldEfect = new PriestShield("Cúpula de Orvalho", 1, modificador);
            modificador = Nodes >= 1 ? 5 : 4;

            int ganho = (int)Math.Round(ModTotal() * modificador * (1 - (Shield / (ModTotal() * modificador))));
            Shield += Math.Max(0, ganho);
            if (!status.Any(x => x.Name == "Cúpula de Orvalho"))
            {
                status.Add(shieldEfect);
            }
            if (aliado != null)
            {
                ganho = (int)Math.Round(ModTotal() * modificador * (1 - (aliado.Shield / (ModTotal() * modificador))));
                aliado.Shield += Math.Max(0, ganho);
                if (!aliado.status.Any(x => x.Name == "Cúpula de Orvalho"))
                {
                    aliado.status.Add(shieldEfect);
                }
                Console.WriteLine($"> {Name} protegeu {aliado.Name} com as águas do Nilo!");
                Console.WriteLine($"> {Name}: A correnteza te envolve");
            }
            Console.ResetColor();
        }

        private void AplicarAura(PersonagemBase alvo, double mod)
        {
            if (!alvo.status.Any(x => x.Name == "Benção do Oásis"))
            {
                alvo.status.Add(new PriestBless("Benção do Oásis", 1, mod));
            }
        }

        public override void Resetar()
        {
            aplicarPassiva = false;
            ressussitou = false;
            contador = 0;
            base.Resetar();
        }
    }

}
