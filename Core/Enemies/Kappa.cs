using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;

namespace Task_U.Core.Enemies
{
    public class Kappa : InimigoBase
    {
        private bool pratoCaiu = false;
        private bool recebeuShield;

        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            if (useSkill <= HabilidadeChance && alvos != null)
            {
                if (!pratoCaiu)
                {
                    if (Shield == HpMax / 15)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine($"> [PRATO CHEIO!] {Name} Não consegue colocar mais nano-fluídos em seu prato! Seu escudo está no máximo!");
                        Console.ResetColor();
                        PersonagemBase alvo = EscolherAlvo();
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine($"> [TRANSBORDAR NANO-FLUÍDO] {Name} transborda, causando dano a {alvo.Name}");
                        Console.ResetColor();
                        alvo.tomarDano(Name, Mod);
                    }
                    else
                    {
                        Shield = Math.Min(Shield + HpMax / 35, HpMax / 15);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine($"> [GLUB GLUB!] {Name} enche seu prato com nano-fluídos! Ele recebeu mais escudo!");
                        Console.ResetColor();
                    }
                }
                else
                {
                    PersonagemBase alvo = EscolherAlvo();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"> [RAJADA DE NANO-FLUÍDOS!] {Name} atira nano-fluídos eletrocutantes contra {alvo.Name}");
                    Console.ResetColor();
                    alvo.tomarDano(Name, Atk + Mod);
                }
            }
        }

        public override void Passiva(User user)
        {
            if (!recebeuShield)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"> [GLUB GLUB!] {Name} está com seu prato com nano-fluídos! Ele escudo!");
                Console.ResetColor();
                Shield = HpMax / 25;
                recebeuShield = true;
            }
            if (Shield == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [É O FIM!] O prato de {Name} foi quebrado! Ele entrou em desespero!");
                Console.ResetColor();
                pratoCaiu = true;
            }
            base.Passiva(user);
        }

        public override void Resetar()
        {
            pratoCaiu = false;
            recebeuShield = false;
            base.Resetar();
        }
    }
}