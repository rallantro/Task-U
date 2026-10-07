using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Entities
{
    public class Star : PersonagemBase
    {
        private int orarCD;
        private int curaBonus;

        private int sacriTotal;

        private double modificador;

        private bool constState = false;
        private int constCd;

        public override void tomarDano(string inimigo, int dano)
        {
            if ((dano * debuffRes) - Shield > HpAtual && constState)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> {Name} está em plenitude e não pode ser morta.");
                Console.WriteLine($"> {Name}: As estrelas... ainda não terminaram de me usar.");
                Console.ResetColor();
                HpAtual = 1;
                return;
            }
            base.tomarDano(inimigo, dano);
        }

        public override int Damage()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"> {Name}: Que sua dor me ilumine!");
            Console.ResetColor();
            modificador = Nodes >= 2 ? 0.2 : 0.15;
            int heal = (int)Math.Ceiling(AtkTotal() * ModTotal() / 2 * modificador);
            if (aliado != null && ((double)aliado.HpAtual / aliado.HpMax) < ((double)HpAtual / HpMax) && aliado.HpAtual > 0)
            {
                aliado.curar(Name, heal);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> {Name} curou {aliado.Name} em {heal} pontos de vida!");
                Console.ResetColor();
            }
            else
            {
                curar(Name, heal);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> {Name} se curou em {heal} pontos de vida!");
                Console.ResetColor();
            }
            return AtkTotal();
        }
        public override void Habilidade()
        {
            modificador = Nodes >= 3 ? 0.48 : 0.35;
            int heal = (int)Math.Ceiling(HpMax * 0.08 * ModTotal() * modificador) + curaBonus;
            modificador = Nodes >= 1 ? 2.43 : 4;
            heal = (int)Math.Ceiling(heal * (1 + (1 - ((double)HpAtual / HpMax)) / modificador));

            if (aliado != null && aliado.HpAtual < aliado.HpMax * 0.8 && HpAtual > HpMax * 0.3 && aliado.HpAtual > 0)
            {
                int cost = (int)Math.Ceiling(HpAtual * 0.08);
                HpAtual -= cost;
                if (orarCD <= 0)
                {
                    sacriTotal += cost;
                    double threshold = HpMax * 0.28;
                    double pct = sacriTotal / threshold;

                    Console.ForegroundColor = pct > 0.75 ? ConsoleColor.Yellow : ConsoleColor.DarkYellow;
                    if (pct < 0.4)
                    {
                        Console.WriteLine($"> {Name}: Estrelas... ouçam-me.");
                        Console.WriteLine($"> {Name} está acumulando suas preces. {sacriTotal}/{HpMax * 0.28:F0} para alcançar [GRAÇA ESTELAR]");
                    }
                    else if (pct < 0.75)
                    {
                        Console.WriteLine($"> {Name}: As estrelas... ainda não responderam.");
                        Console.WriteLine($"> {Name} está acumulando suas preces. {sacriTotal}/{HpMax * 0.28:F0} para alcançar [GRAÇA ESTELAR]");
                    }
                    else if (pct >= 1)
                    {
                        Console.WriteLine($"> {Name}: Fui ouvida...");
                        Console.WriteLine($"> {Name} alcançou a graça. No próximo turno ela estará mais forte.");
                    }
                    else
                    {
                        Console.WriteLine($"> {Name}: Está perto... eu sinto o clamor voltando.");
                        Console.WriteLine($"> {Name} está acumulando suas preces. {sacriTotal}/{HpMax * 0.28:F0} para alcançar [GRAÇA ESTELAR]");
                    }
                    Console.ResetColor();
                }

                int faltando = aliado.HpMax - aliado.HpAtual;
                int curaEfetiva = Math.Min(heal, faltando);
                int overflow = heal - curaEfetiva;

                aliado.curar(Name, curaEfetiva);
                if (overflow > 0)
                {
                    Console.WriteLine($"> O excesso de luz transbordou: +{overflow} de escudo para {aliado.Name}.");
                    aliado.Shield += overflow;
                }

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [HABILIDADE] {Name} roga as estrelas, curando {heal} pontos de vida de {aliado.Name}, sacrificando {cost} pontos!");
                Console.WriteLine($"> {Name}: Rogo por {aliado.Name}, estrelas, que sua luz brilhe por mim!");
                Console.ResetColor();
            }
            else
            {
                int healAlly = (int)Math.Ceiling(heal * 0.6);
                int healSelf = heal - healAlly;
                if (aliado != null && (aliado.HpAtual + healAlly) <= aliado.HpMax && aliado.HpAtual > 0 && HpAtual > HpMax * 0.2)
                {
                    Console.WriteLine($"> [HABILIDADE] {Name} roga as estrelas, curando {aliado.Name} em {healAlly}!");
                    aliado.curar(Name, healAlly);
                }
                else
                {
                    healSelf += healAlly;
                }
                modificador = Nodes >= 4 ? 1.22 : 1;
                healSelf = (int)Math.Ceiling(healSelf * modificador);
                curar(Name, healSelf);

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [HABILIDADE] {Name} roga as estrelas, curando a si mesma em {healSelf}...");
                Console.WriteLine($"> {Name}: Que o brilho de cada estrela brilhe através de meu corpo!");
                Console.ResetColor();
            }

            curaBonus = 0;

        }
        public override void Passiva()
        {
            if (sacriTotal > HpMax * 0.28 && orarCD <= 0)
            {
                curaBonus = sacriTotal / 2;
                orarCD = 4;
                sacriTotal = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [GRAÇA ESTELAR] Uma coroa de estrelas surge ao redor de {Name}. ({Name} possui {curaBonus} de bônus de cura!)");
                Console.WriteLine($"> [GRAÇA ESTELAR] O bônus de cura será consumido no próximo uso da habilidade.");
                Console.WriteLine($"> {Name}: As estrelas ouvem a mim...");
                Console.ResetColor();
                if (Nodes >= 6)
                {
                    constState = true;
                    constCd = 2;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> [PLENITUDE] Uma luz fina começa a pulsar sob a pele de {Name}.");
                    Console.WriteLine($"> {Name}: Elas ouviram o meu clamor...");
                    Console.ResetColor();
                }
            }
            else if (constState && constCd > 0)
            {
                BuffMod += (int)Math.Ceiling(ModTotal() * 0.35);
                constCd--;

                if (constCd > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> [PLENITUDE] A luz continua pulsando sob a pele de {Name}.");
                    Console.WriteLine($"> {Name}: ... eu ainda estou aqui.");
                    Console.ResetColor();
                }
                else
                {
                    constState = false;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"> A luz se apaga. {Name} volta a si — exausta.");
                    Console.WriteLine($"> {Name}: ... obrigada.");
                    Console.ResetColor();
                }
            }

            if (aliado != null)
            {

                modificador = Nodes >= 5 ? 0.12 : 0.08;
                int bonus = (int)Math.Ceiling(aliado.AtkTotal() * ModTotal() * modificador);
                var buff = new BonusDMG("Fortalecimento Estelar", 1, bonus);
                aliado.status.Add(buff);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [FORTALECIMENTO ESTELAR] {Name} canaliza uma bênção em {aliado.Name}. (+{bonus} de dano)");
                Console.ResetColor();
            }

            if (orarCD > 0)
            {
                orarCD--;
            }
        }

        public override void Resetar()
        {
            orarCD = 0;
            curaBonus = 0;
            sacriTotal = 0;
            constState = false;
            constCd = 0;
            base.Resetar();
        }
    }
}