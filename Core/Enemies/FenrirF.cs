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
        private bool frenesi = false;
        private bool eraFrenesi = false;

        public override int SpeedTotal()
        {
            return Math.Max(1, Speed + BuffSpeed + stackSpeed);
        }
        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            if (useSkill < HabilidadeChance)
            {
                if (frenesi)
                {
                    int chance = rand.Next(1, 101);
                    if (chance > 50)
                    {
                        PersonagemBase alvo = EscolherAlvo();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"> [CAÇADA SANGUENOLENTA] {Name} sente o cheiro de sangue em {alvo.Name}, e dá uma mordida feroz!");
                        Console.WriteLine($"> {Name}: *GRRRRAAAWWW!*");
                        Console.ResetColor();
                        int danoBase = (int)Math.Ceiling(Damage() * 1.38);
                        int danoBonus = (int)Math.Ceiling((1 - (double)alvo.HpAtual / alvo.HpMax) * danoBase * 0.67);
                        alvo.tomarDano(Name, danoBase + danoBonus);
                        HpAtual += (int)Math.Ceiling((danoBase + danoBonus) * 0.12);
                    }
                    else if (alvos != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"> [UIVAR AMEAÇADOR] {Name} uiva e enfraquece todos ao seu redor! (-37% de Ataque em todos os Personagens)");
                        Console.WriteLine($"> {Name}: Auuuuuuuuuuuuu...");
                        Console.ResetColor();
                        foreach (var personagem in alvos)
                        {
                            int valor = (int)Math.Ceiling(personagem.AtkTotal() * 0.37);
                            var debuff = new DebuffDMG("Maldição do Fernir", 2, valor);
                            personagem.status.Add(debuff);

                        }

                    }

                }
                else
                {
                    PersonagemBase alvo = EscolherAlvo();
                    if (alvo.HpAtual >= alvo.HpMax * 0.3)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"> [MORDIDA SANGUINÁRIA] {Name} dá uma mordida poderosa em {alvo.Name}");
                        Console.WriteLine($"> {Name}: *grrr...*");
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
                        Console.WriteLine($"> {Name}: *sniff*");
                        Console.ResetColor();
                        int danoBase = Damage();
                        int danoBonus = (int)Math.Ceiling((1 - (double)alvo.HpAtual / alvo.HpMax) * danoBase * 0.67);
                        alvo.tomarDano(Name, danoBase + danoBonus);
                    }
                }
            }
        }

        public override void Passiva(User user)
        {
            stackSpeed = Math.Min(stackSpeed + 5, 50);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"> [EMOÇÃO DA CAÇADA] {Name} se fortalece com o decorrer da luta. (+5 de velocidade | Total: {stackSpeed})");
            Console.WriteLine($"> {Name}: *Grrr...* Au! Au!");
            Console.ResetColor();

            eraFrenesi = frenesi;
            if (HpAtual < 0.3 * HpMax)
            {
                frenesi = true;
            }
            else
            {
                frenesi = false;
            }

            if (frenesi && !eraFrenesi)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [FRENESI DA BESTA] {Name} cresce, seus olhos brilham em vermelho, e as runas douradas em seu corpo parecem emitir luz própria!");
                Console.WriteLine($"> {Name}: *GRRRRAAAWWW!* AU AU AUUUU!");
                Console.ResetColor();
            }
            else if (!frenesi && eraFrenesi)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"> [FRENESI DA BESTA] {Name} está calmo... Por enquanto...");
                Console.WriteLine($"> {Name}: Arf... arf...");
                Console.ResetColor();
            }
        }

        public override void Resetar()
        {
            stackSpeed = 0;
            frenesi = false;
            eraFrenesi = false;
            base.Resetar();
        }

    }
}