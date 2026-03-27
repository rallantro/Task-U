using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;

namespace Task_U.Core
{
    public class FadaRa : InimigoBase
    {
        private int Aprimoramentos;
        private bool shieldInicial;

        public override void tomarDano(PersonagemBase inimigo, int dano)
        {
            int danoTotal = Math.Max(0, dano - Shield);
            int danoShield = Math.Min(Shield, dano);
            Shield -= danoShield;
            HpAtual -= danoTotal;
            if (danoShield > 0 && danoTotal == 0)
            {
                Console.WriteLine($"{Name} bloqueou completamente o ataque de {inimigo.Name} com suas fadas!");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"> {Name}: Prrrrrriiii! Hahaha meu exército é infinito!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"{inimigo.Name} atacou {Name} e causou {danoTotal} de dano!");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"> {Name}: Prrrrrriiii!!!!!");
                Console.ResetColor();
            }
        }
        public override int Damage()
        {
            return base.Damage();
        }
        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            int vezes = rand.Next(2, 4 + Aprimoramentos);
            if (useSkill < HabilidadeChance/2)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"> [Ordens da Rainha!] {Name} ordena que seus nano-bôs e ataca várias vezes!");
                Console.WriteLine($"> {Name}: Prrrrrriiii! S-sofra imediatamente!");
                Console.ResetColor();
                for (int i = 0; i < vezes; i++)
                {
                    int chance = rand.Next(0, alvos.Count());
                    PersonagemBase alvo = alvos[chance];
                    alvo.tomarDano(Name, 4);
                }
                
            } else if (useSkill < HabilidadeChance)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"> [NANO ASSIMILAR!] {Name} ordena que seus nano-bôs roubem os dados de um aliado!");
                Console.WriteLine($"> {Name}: Prrrrrriiii! Resistir é inútil, vocês serão assimilados!");
                Console.ResetColor(); 
                int chance = rand.Next(0, alvos.Count());
                PersonagemBase alvo = alvos[chance];
                alvo.BuffAtk -= Mod;
                BuffAtk += Mod;
                Aprimoramentos++;
            }
        }

        public override void Passiva(User user)
        {
            int useSkill = 0;
            if (Shield > 0)
            {
                useSkill = rand.Next(1, 11);
                if (useSkill >= 7)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine($"> [NANO BARREIRA] {Name} junta fadas a sua frente como uma barreira.");
                    Console.WriteLine($"> {Name}: Prrrrrriiii! Me protejam!");
                    Console.ResetColor();
                    Shield = Mod;
                }
            } else if (!shieldInicial)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"> [NANO BARREIRA] {Name} junta fadas a sua frente como uma barreira.");
                Console.WriteLine($"> {Name}: Prrrrrriiii! Estarei sempre protegida!");
                Console.ResetColor();
                Shield = Mod;
            }
            if (Aprimoramentos >= 10)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"> [NANO EVOLUÇÃO] {Name} evolúi, criando um par de asas adicional.");
                Console.WriteLine($"> {Name}: Prrrrrriiii! HAHAHAHAHAHA!");
                Console.ResetColor();
                BuffAtk += Aprimoramentos;
            }
            useSkill = rand.Next(1, 11);
            if (useSkill >= 7)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"> [NANO RECONSTRUÇÃO] {Name} começa a se resonstruir.");
                Console.WriteLine($"> {Name}: Prrrrrriiii! Me tornem gloriosa!");
                Console.ResetColor();
                HpAtual += HpMax/20;
            }
        }
    }
}