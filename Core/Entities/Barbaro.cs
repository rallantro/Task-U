using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Task_U.Models;
using Task_U.Data;

namespace Task_U.Core
{
    public class Barbaro : PersonagemBase
    {
        private int furia { get; set; }
        private int defesa { get; set; }
        private static readonly Random random = new Random();
        public int BaseAtk { get; private set; }
        private double modificador;
        private bool berserk = false;

        public override int Damage()
        {
            modificador = Nodes >= 3 ? 0.07 : 0.05; 
            int furiaBonus = (int)Math.Ceiling(furia * modificador * ModTotal());
            return AtkTotal() + BaseAtk + furiaBonus;
        }

        public override void tomarDano(string inimigo, int dano)
        {

            int danoTotal = Math.Max(0, (int)(dano * debuffRes) - Shield);
            int danoShield = Math.Min(Shield, (int)(dano * debuffRes));
            Shield -= danoShield;
            int danoReal = Math.Max(0, danoTotal - defesa);
            modificador = Nodes >= 1 ? 0.65 : 0.5;
            furia = Math.Min(55, (int)Math.Ceiling(danoReal * modificador));
            HpAtual = Math.Max(0, HpAtual -= danoReal);
            if (danoShield > 0 && danoTotal == 0)
            {
                Console.WriteLine($"{Name} bloqueou completamente o ataque de {inimigo} com seu escudo!");
            }
            else
            {
                if (danoReal == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> [PASSIVA] {Name} resiste ao ataque, anulando o dano inimigo!");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"{inimigo} atacou {Name} e causou {danoTotal} de dano!");  
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (danoReal < danoTotal)
                    {
                        Console.WriteLine($"> [PASSIVA] {Name} resiste ao ataque, recebendo apenas {danoReal} de dano!");   
                    }
                    if (Nodes == 6 && HpAtual <= 0 && !berserk)
                    {
                        HpAtual = 1;
                        berserk = true;
                        furia = 55;
                    }
                    
                    Console.ResetColor(); 
                }
            }
        }

        public override void Habilidade()
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"> [HABILIDADE] {Name} entra na frente dos inimigos enquanto consome sua fúria!");
            Console.ResetColor();
            chanceAlvo = 450;
            Console.WriteLine($"> {Name} tem 90% de chance de ser alvo dos ataques inimigos.");
            int diferenca = furia - (int)Math.Ceiling(furia * 0.45);
            modificador = Nodes >= 2 ? 0.38 : 0.45;
            furia -= (int)Math.Ceiling(furia * modificador);
            curar("consumo de fúria", diferenca);
        }

        public override void Passiva()
        {
            modificador = Nodes >= 5 ? 0.4 : 0.25;
            BaseAtk = (int)Math.Ceiling((HpMax - HpAtual) * ModTotal() * modificador);
            if (BaseAtk > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [PASSIVA] A fúria de {Name} aumenta! (+{BaseAtk} de ATK)");
                Console.ResetColor();
            }
            modificador = Nodes >= 4 ? 45 : 35;
            defesa = Math.Min((int)modificador, furia * ModTotal());
            if (defesa > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [PASSIVA] A fúria de {Name} a deixa mais resistente! (Resiste à {defesa} do dano recebido)");
                Console.ResetColor();
            }
        }

        public override void Resetar()
        {
            berserk = false;
            furia = 0;
            defesa = 0;
            BaseAtk = 0;
            base.Resetar();
        }

    }
}