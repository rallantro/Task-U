using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;
using Task_U.Core;
using Task_U.Core.StatusEffects;

namespace Task_U.Core
{
    public class Aranha : InimigoBase
    {
        private bool finalFase = false;
        public override int Damage()
        {
            return base.Damage();
        }

        public override void Habilidade()
        {
            PersonagemBase alvo = EscolherAlvo();
            int useSkill = rand.Next(1, 101);
            if (useSkill < HabilidadeChance || finalFase && useSkill < HabilidadeChance * 2)
            {
                if (!finalFase)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"> [INJEÇÃO DE VENENO] {Name} ataca {alvo.Name}, injetando veneno com suas garras afiadas.");
                    Console.WriteLine($"> {Name}: Apodreça...");
                    Console.ResetColor();
                    var poison = new PoisonMaxStack("Envenenamento", Mod, 10);
                    alvo.status.Add(poison);
                    int totalPercent = alvo.status.OfType<PoisonMaxStack>().Sum(p => p.Mod.Value);
                    Console.WriteLine($"{alvo.Name} agora está envenenado, com um envenenamento maior! ({totalPercent}% da vida máxima por turno)");
                }
                else
                {
                    int aranhas = rand.Next(3, 9);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"> [COMANDO DA PROLE] {Name} chama sua prole para atacar!");
                    Console.WriteLine($"> {Name}: Ataquem, minhas pequenas!");
                    Console.ResetColor();
                    for (int i = 0; i < aranhas; i++)
                    {
                        alvo = EscolherAlvo();
                        var smallPoison = new PoisonFixedStack("Envenenamento menor", 1, 1);
                        alvo.status.Add(smallPoison);
                        Console.WriteLine($"> {alvo.Name} foi mordido por uma aranha menor!");
                    }
                    foreach (var alvoVeneno in alvos)
                    {
                        if (alvoVeneno.status.Any(x => x is PoisonFixedStack))
                        {
                            Console.WriteLine($"{alvoVeneno.Name} agora está envenenado, com um envenenamento menor! ({alvoVeneno.status.Count(x => x is PoisonFixedStack)} de dano por turno)");
                        }
                    }
                    alvo = EscolherAlvo();
                    var poison = new PoisonMaxStack("Envenenamento", Mod, 15);
                    alvo.status.Add(poison);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"> [INJEÇÃO DE VENENO] {Name} ataca {alvo.Name}, injetando veneno com suas garras afiadas.");
                    Console.WriteLine($"> {Name}: Putrifique!");
                    Console.ResetColor();
                    int totalPercent = alvo.status.OfType<PoisonMaxStack>().Sum(p => p.Mod.Value);
                    Console.WriteLine($"{alvo.Name} agora está envenenado, com um envenenamento maior! ({totalPercent}% da vida máxima por turno)");

                    PersonagemBase alvoStun = alvos.Where(x => !x.status.Any(x => x is Stun)).OrderByDescending(x => x.status.Count(a => a.Name.Contains("Envenenamento"))).FirstOrDefault();
                    if (alvoStun != null && alvoStun.status.Count(x => x.Name.Contains("Envenenamento")) > 3)
                    {
                        var stun = new Stun("Stun", 2);
                        alvoStun.status.Add(stun);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"> [TEIA NEURO-TOXICA] {Name} enrola {alvoStun.Name} com uma teia neuro-tóxica, o paralizando por 2 turnos.");
                        Console.WriteLine($"> {Name}: Decomponha!");
                        Console.ResetColor();
                    }
                }
            }


        }

        public override void Passiva(User user)
        {
            if (HpAtual <= HpMax / 5 && !finalFase)
            {
                finalFase = true;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [PERDA DAS APARÊNCIAS] {Name} revela sua natureza monstruosa!");
                Console.WriteLine($"> {Name}: Eu... Tentei... Ser gentil!");
                Console.ResetColor();
            }
        }

        public override void Resetar()
        {
            finalFase = false;
            base.Resetar();
        }
    }
}