using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;

namespace Task_U.Core
{
    public class Gargula : InimigoBase
    {
        private int reducao;
        public override void tomarDano(PersonagemBase inimigo, int dano)
        {
            
            int danoTotal = Math.Max(0, dano - Shield - reducao);
            int danoShield = Math.Min(Shield, dano - reducao);
            Shield -= danoShield;
            HpAtual -= danoTotal;
            if (danoShield > 0 && danoTotal == 0)
            {
                Console.WriteLine($"{Name} bloqueou completamente o ataque de {inimigo.Name} com seu escudo!");
            }
            else if(danoTotal <= reducao)
            {
                Console.WriteLine($"{Name} bloqueou completamente o ataque de {inimigo.Name} com sua pele dura!");
            }else
            {
                Console.WriteLine($"{inimigo.Name} atacou {Name} e causou {danoTotal} de dano!");
            }
        }

        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            if (useSkill <= HabilidadeChance)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"> [PLASMA DE PROTEÇÃO] {Name} Se proteje recebendo {Mod} de escudo!");
                Console.ResetColor();
                Shield = Mod;
            }
            else if(useSkill <= HabilidadeChance*2)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"> [SARAIVADA DE PLASMA!] {Name} Libera uma rajada de plasma atingindo toda a equipe!");
                Console.ResetColor();
                foreach (var personagem in alvos)
                {
                    personagem.tomarDano(Name, Mod/2);
                }
            }
        }

        public override void Passiva(User user)
        {
            if (HpAtual >= HpMax/4)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"> [CASCA GROSSA] {Name} é imune a ataques menores que {Mod}!");
                Console.ResetColor();
                reducao = Mod;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"> [RESISTÊNCIA APRIMORADA] {Name} se aprimora para se tornar imune a ataques menores que {Mod * 2}!");
                Console.ResetColor();
                reducao = Mod * 2;
            }
        }
    }
}