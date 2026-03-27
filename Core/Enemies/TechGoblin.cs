using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Models;

namespace Todo_Gacha.Core
{
    public class TechGoblin : InimigoBase
    {
        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            if (useSkill < HabilidadeChance)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"> [CURTO-CIRCUITO!] {Name} causa um ataque elétrico com dano adicional!!");
                Console.ResetColor();
                int chance = rand.Next(0, alvos.Count());
                PersonagemBase alvo = alvos[chance];
                alvo.tomarDano(Name, Mod);
            }
        }

        public override void Passiva(User user)
        {
            int useSkill = rand.Next(1, 11);
            if (useSkill >= 5)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"> [NAHM NAHM!] {Name} comeu um circuito! O ataque dele aumenta em {Mod} por um turno!");
                BuffAtk += Mod;
                Console.ResetColor();
            }
        }
        
    }
}