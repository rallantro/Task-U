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
    public class Grafiteiro : PersonagemBase
    {
        private int BonusDMG;
        private int QuantDmg;
        private bool Paint;
        private double modificador;

        public override int Damage()
        {
            int dano = AtkTotal();
            if (!Paint && QuantDmg > 0)
            {
                dano = (AtkTotal() + BonusDMG) * (QuantDmg + 1) / 2;
                QuantDmg = 0;
                BonusDMG = 0;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> KABOOOOOOM! {Name} deu TONELADAS de dano! ({dano})");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"{Name} causou {dano} de dano!");
            }
            return dano;
        }

        public override void Habilidade()
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"Qual cor {Name} deve usar?");
            Console.WriteLine("[0] Alternar modo de pintura (preparar/parar)");
            if (QuantDmg == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            Console.WriteLine("[1] Vermelha (Explosão)");
            Console.WriteLine("[2] Azul (Debuff no inimigo)");
            Console.WriteLine("[3] Bufagem (buff na equipe)");
            Console.ForegroundColor = ConsoleColor.Red;

            int opcao = int.Parse(Console.ReadLine() ?? "0");
            int tintaUsada = QuantDmg;
            bool obraPrima = Nodes >= 6 && tintaUsada >= 10;
            bool escolheu = false;
            int tintaPerdida = 0;
            while (!escolheu)
            {
                switch (opcao)
                {
                    case 0:
                        Paint = !Paint;
                        Console.WriteLine($"> {Name} {(Paint ? "começa a" : "para de")} pintar!");
                        escolheu = true;
                        tintaPerdida = 0;
                        break;
                    case 1:
                        if (QuantDmg > 0)
                        {
                            int danoBase = AtkTotal() + ModTotal();
                            int danoFinal = (int)(danoBase * (1 + tintaUsada * 0.15));
                            if (obraPrima) danoFinal *= 2;
                            if (inimigoAlvo != null) inimigoAlvo.tomarDano(this, danoFinal);
                            Console.WriteLine($"> {Name} usa {tintaUsada} camadas de tinta, causando {danoFinal} de dano!");
                            escolheu = true;
                            modificador = Nodes >= 5 ? 0.40 : 0.53;
                            tintaPerdida = (int)Math.Ceiling(QuantDmg * modificador);
                        }
                        else
                        {
                            Console.WriteLine($"{Name} não tem tinta o suficiente...");
                        }
                        break;
                    case 2:
                        if (QuantDmg > 0)
                        {
                            if (inimigoAlvo != null)
                            {
                                inimigoAlvo.status.Add(new Blind("Cegueira", 2 + (obraPrima ? 2 : 0)));
                                Console.WriteLine($"> {Name} espicha tinta azul! Inimigo cego por 2 turnos!");
                                escolheu = true;
                                modificador = Nodes >= 5 ? 0.35 : 0.45;
                                tintaPerdida = (int)Math.Ceiling(QuantDmg * modificador);
                            }
                            else
                            {
                                Console.WriteLine($"{Name} não tem tinta o suficiente...");
                            }
                        }
                        break;
                    case 3:
                        if (QuantDmg > 0)
                        {
                            modificador = Nodes >= 3 ? 0.68 : 0.5;
                            int bonusAtk = (int)Math.Ceiling(AtkTotal() * modificador * (obraPrima ? 2 : 1));
                            modificador = Nodes >= 3 ? 0.35 : 0.2;
                            int bonusSpeed = (int)Math.Ceiling(Speed * modificador * (obraPrima ? 2 : 1));
                            modificador = Nodes >= 3 ? 2 : 1;
                            var buffA = new BonusDMG("Euforia Amarela", (int)modificador, bonusAtk);
                            var buffS = new BonusSPD("Ânimo Amarelo", (int)modificador, bonusSpeed);
                            status.Add(buffA);
                            status.Add(buffS);
                            Console.WriteLine($"> {Name} se pinta com +{bonusAtk} de Atk e +{bonusSpeed} de Speed por 2 turnos!");
                            if (aliado != null)
                            {
                                aliado.status.Add(buffA);
                                aliado.status.Add(buffS);
                                Console.WriteLine($"> {Name} também pinta {aliado.Name} com +{bonusAtk} de Atk e +{bonusSpeed} de Speed por 2 turnos!");
                            }
                            modificador = Nodes >= 5 ? 0.25 : 0.35;
                            tintaPerdida = (int)Math.Ceiling(QuantDmg * modificador);
                            escolheu = true;
                        }
                        else
                        {
                            Console.WriteLine($"{Name} não tem tinta o suficiente...");
                        }
                        break;
                }
            }
            QuantDmg -= tintaPerdida;
            if (obraPrima && Nodes >= 6)
            {
                Console.WriteLine("> OBRA-PRIMA! O efeito foi dobrado!");
            }
            Console.ResetColor();
        }

        public override void Passiva()
        {
            if (Paint)
            {
                modificador = Nodes >= 4 ? 12 : 10;
                if (QuantDmg > modificador)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> [PASSIVA] A TINTA NÃO VAI AGUENTAR MAIS... EXPLOSÃO NO PRÓXIMO ATAQUE!");
                    Console.ResetColor();
                    Paint = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    int vezes = Nodes >= 1 ? 2 : 1;
                    modificador = Nodes >= 2 ? 1.98 : 1.82;
                    int danoBonus = (int)Math.Ceiling(BonusDMG + (ModTotal() * modificador));
                    Console.WriteLine($"> [PASSIVA] COLOCANDO TINTA! Camada {QuantDmg + vezes} (+{danoBonus} de dano na explosão)!");
                    Console.ResetColor();
                    BonusDMG += (int)Math.Ceiling(ModTotal() * modificador);
                    QuantDmg += vezes;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [PASSIVA] Sem tinta!");
                Console.ResetColor();
            }
        }

        public override void Resetar()
        {
            BonusDMG = 0;
            QuantDmg = 0;
            Paint = false;
            base.Resetar();
        }
    }
}
