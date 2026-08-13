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

        public override int Damage()
        {
            var dano = AtkTotal() + BaseAtk;
            Console.WriteLine($"{Name} causou {dano} de dano!");
            return AtkTotal() + BaseAtk;
        }

        public override void tomarDano(string inimigo, int dano)
        {

            int danoTotal = Math.Max(0, (int)(dano * debuffRes) - Shield);
            int danoShield = Math.Min(Shield, (int)(dano * debuffRes));
            Shield -= danoShield;
            int danoReal = Math.Max(0, danoTotal - defesa);
            furia += 1;
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
                    Console.WriteLine($"> [PASSIVA] {Name} resiste ao ataque, recebendo apenas {danoReal} de dano!");
                    Console.ResetColor(); 
                }
            }
        }

        public override void Habilidade()
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"> [HABILIDADE] {Name} entra na frente dos inimigos!");
            Console.ResetColor();

            chanceAlvo = 450;
            Console.WriteLine($"> {Name} tem 90% de chance de ser alvo dos ataques inimigos.");

            if (HpAtual > 4)
            {
                HpAtual -= 4;
                BaseAtk += 5;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [HABILIDADE:] A fúria de {Name} aumenta! +{5} de ATK neste turno pelo custo de {4} pontos de vida");
                Console.ResetColor();
            }
        }

        public override void Passiva()
        {
            BaseAtk = (HpMax - HpAtual) * ModTotal() / 4;
            if (BaseAtk > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [PASSIVA] A fúria de {Name} aumenta! (+{BaseAtk} de ATK)");
                Console.ResetColor();
            }
            defesa = Math.Min(15, furia * Mod / 2);
            if (defesa > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [PASSIVA] A fúria de {Name} a deixa mais resistente! (Resiste à {defesa} do dano recebido)");
                Console.ResetColor();
            }
        }

    }
}