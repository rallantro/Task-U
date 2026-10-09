using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;
using Task_U.Core.StatusEffects;

namespace Task_U.Core
{
    public class FenrirF : InimigoBase
    {
        private int stackSpeed;
        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            if (useSkill < HabilidadeChance)
            {
                PersonagemBase alvo = EscolherAlvo();
                if (alvo.HpAtual >= alvo.HpMax * 0.3)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"> [MORDIDA SANGUINÁRIA] {Name} dá uma mordida poderosa em {alvo.Name}");
                    Console.ResetColor();
                    double porcentagem = SpeedTotal() * 0.0375;
                    var poison = new PoisonMaxStack("Sangramento", 2, porcentagem);
                    alvo.status.Add(poison);
                    alvo.tomarDano(Name, Damage());
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"> [CAÇADA SANGUENOLENTA] {Name} sente o cheiro de sangue em {alvo.Name}, e dá uma mordida feroz!");
                    Console.ResetColor();
                    int danoBase = Damage();
                    int danoBonus = (int)Math.Ceiling((1 - (double)alvo.HpAtual / alvo.HpMax) * danoBase * 0.67);
                    alvo.tomarDano(Name, danoBase + danoBonus);
                }

            }
        }

        public override void Passiva(User user)
        {
            stackSpeed += 5;
            BuffSpeed = stackSpeed;
        }

        public override void Resetar()
        {
            stackSpeed = 0;
            base.Resetar();
        }

    }
}