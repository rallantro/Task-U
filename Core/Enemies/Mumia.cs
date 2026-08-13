using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;
using Task_U.Core.StatusEffects;

namespace Task_U.Core
{
    public class Mumia : InimigoBase
    {

        private bool explodiu;
        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            if (useSkill <= HabilidadeChance)
            {
                var alvo = EscolherAlvo();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"> [SUCÇÃO DE ESPARADRAPOS] {Name} projeta seus esparadrapos contra {alvo.Name}!");
                Console.ResetColor();
                int vezes = rand.Next(1, 5);
                for (int i = 0; i < vezes; i++)
                {
                    alvo.tomarDano(Name, Atk / 2);
                    HpAtual += Atk / 2 / 2;
                    Console.WriteLine($"> {Name} absorveu {Atk / 2 / 2} de vida!");
                }
                if (alvo.HpAtual < alvo.HpMax * 0.5)
                {
                    var debuff = new PoisonMaxStack("Decaimento", 10, 5);
                    var oldDebuff = alvo.status.FirstOrDefault(x => x.Name == "Decaimento");
                    if (oldDebuff != null)
                    {
                        oldDebuff.Duration = 10;    
                    }
                    else
                    {
                        alvo.status.Add(debuff);    
                    }
                    
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"> [MALDIÇÃO DA MORTE] {Name} amaldiçoa {alvo.Name}, para que ele morra em 10 turnos!");
                    Console.ResetColor();
                }
                else
                {
                    var debuff = new Stun("Aprisionamento", 1);
                    alvo.status.Add(debuff);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"> [MALDIÇÃO DA PARALISIA] {Name} amaldiçoa {alvo.Name}, prendendo-o com seus esparadrapos!");
                    Console.ResetColor();
                }
            }
        }

        public override void Passiva(User user)
        {
            if (HpAtual <= HpMax * 0.30 && alvos != null && explodiu == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"> [MALDIÇÃO FINAL] {Name} conjura tudo que é mais profano em desespero!");
                Console.ResetColor();
                foreach (var personagem in alvos)
                {
                    personagem.tomarDano(Name, Atk*2);
                    HpAtual -= HpAtual/2;
                }
                explodiu = true;
            }
        }

        public override void Resetar()
        {
            explodiu = false;
            base.Resetar();
        }
    }
}