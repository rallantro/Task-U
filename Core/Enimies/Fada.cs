using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Models;

namespace Todo_Gacha.Core
{
    public class Fada : InimigoBase
    {
        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            int vezes = rand.Next(2,Mod);
            if (useSkill < HabilidadeChance)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"> [ATAQUE NANO-BÔ!] {Name} se dissolve em um enxame de nano-bôs e ataca várias vezes!");
                Console.ResetColor();
                for (int i = 0; i < vezes; i++)
                {
                    int chance = rand.Next(0, alvos.Count());
                    PersonagemBase alvo = alvos[chance];
                    alvo.tomarDano(Name, Atk);
                }
                
            }
        }

        public override void Passiva(User user)
        {
            int useSkill = rand.Next(1, 11);
            if (useSkill >= 3)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"> [NANO RECONSTRUÇÃO] {Name} começa a se resonstruir.");
                Console.ResetColor();
                HpAtual += Mod;
            }
        }
    }
}