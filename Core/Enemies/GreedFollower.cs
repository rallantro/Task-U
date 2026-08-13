using System;
using System.Collections.Generic;
using System.Linq;
using Task_U.Core.StatusEffects;
using Task_U.Models;

namespace Task_U.Core.Enemies
{
    public class GreedFollower : InimigoBase
    {
        private int tesouroAcumulado = 0;
        private bool modoDesespero = false;
        private int bonusChance = 0;

        public override int Damage()
        {
            return Atk + tesouroAcumulado * Mod / 10 + BuffAtk;
        }

        public override void Habilidade()
        {
            int chance = rand.Next(1, 101);
            if (chance <= HabilidadeChance + bonusChance)
            {
                int acao = rand.Next(1, 101);
                PersonagemBase alvo = EscolherAlvo();

                if (acao <= 40)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"> [DÍZIMO DE SANGUE] {Name} avança sobre {alvo.Name} com ganchos famintos.");
                    Console.WriteLine($"> {Name}: Você tem tanto... me dê um pouco... SÓ UM POUCO!");
                    Console.ResetColor();
                    int danoExtra = (int)(alvo.HpAtual * (0.05 + Mod/100.0));
                    alvo.tomarDano(Name, Atk + danoExtra);
                    tesouroAcumulado += 5;
                }
                else if (acao <= 75)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> [GANÂNCIA CORROSIVA] {Name} odeia a proteção de {alvo.Name}.");
                    Console.WriteLine($"> {Name}: Por que você está seguro? SÓ EU POSSO ESTAR PROTEGIDO!");
                    Console.ResetColor();

                    if (alvo.Shield > 0)
                    {
                        int roubo = (alvo.Shield / 2) + Mod;
                        alvo.Shield -= roubo;
                        Shield += roubo;
                        Console.WriteLine($"> {Name} arrancou parte da proteção de {alvo.Name} para si!");
                    }
                    else
                    {
                        alvo.status.Add(new DebuffRes("Exposição", 2, 0.8));
                        Console.WriteLine($"> {Name} jogou uma praga sobre {alvo.Name}! (Recebe 80% a mais de dano por 2 turnos)");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"> [RECUO MESQUINHO] {Name} se esconde nas sombras para contar seus restos.");
                    int cura = (Mod * 2) + (tesouroAcumulado / 2);
                    HpAtual = Math.Min(HpMax, HpAtual + cura);
                    Console.WriteLine($"> {Name} recuperou {cura} de HP.");
                    Console.ResetColor();
                }
            }
        }

        public override void Passiva(User user)
        {
            if (HpAtual <= HpMax * 0.35 && !modoDesespero)
            {
                modoDesespero = true;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [POSSESSÃO DOENTIA] {Name} percebe que pode perder tudo!");
                Console.WriteLine($"> {Name}: MEU! É TUDO MEU! VOCÊS NÃO VÃO LEVAR!");
                Console.ResetColor();

                status.Add(new BonusDMG("Desespero", 10, tesouroAcumulado / 2 * Mod / 10));
                bonusChance = 20;
            }

            tesouroAcumulado += 2;
        }

        public override void Resetar()
        {
            tesouroAcumulado = 0;
            modoDesespero = false;
            bonusChance = 0;
            base.Resetar();
        }
    }
}