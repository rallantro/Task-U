using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;

namespace Task_U.Core
{
    public class DronEscaravelho : InimigoBase
    {
        private int analiseTecnica = 0;
        private PersonagemBase? alvoAnalisado;

        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            // Ele ataca mais vezes que a fada, mas com dano menor por hit
            int vezes = rand.Next(3, 6);

            if (useSkill <= HabilidadeChance)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"> [SOBRECARGA DE SISTEMA] {Name} libera um enxame de micro-drones!");
                Console.ResetColor();

                for (int i = 0; i < vezes; i++)
                {
                    PersonagemBase alvo = EscolherAlvo();
                    int danoFinal = Atk / 2;
                    if (alvo == alvoAnalisado)
                    {
                        danoFinal = (Atk + analiseTecnica)/2;
                    }
                    alvo.tomarDano(Name, danoFinal);
                }
                analiseTecnica = 0;
            }
        }

        public override void Passiva(User user)
        {
            if (rand.Next(1, 11) <= 4)
            {
                alvoAnalisado = EscolherAlvo();
                analiseTecnica = Math.Min(Mod * 3, analiseTecnica += Mod);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"> [SCANNER DE VULNERABILIDADE] {Name} está analisando as brechas na defesa de {alvoAnalisado.Name}!");
                Console.WriteLine($"> O próximo ataque de enxame contra {alvoAnalisado.Name} terá +{analiseTecnica} de dano por hit.");
                Console.ResetColor();
            }
        }

        public override void Resetar()
        {
            alvoAnalisado = null;
            analiseTecnica = 0;
            base.Resetar();
        }

    }
}
