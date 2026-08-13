using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Entities
{
    public class Scribe : PersonagemBase
    {
        private int Runes = 0;
        private Random rand = new Random();

        public override int Damage()
        {
            Runes = Math.Min(3, Runes + 1);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"[PASSIVA: PONTO SEM NÓ]: {Name} puxa sua linha congelante e tece uma runa com fios de geada.");
            Console.WriteLine($"Runas Atuais: {Runes}/3");
            Console.ResetColor();
            return base.Damage();
        }

        public override void Habilidade()
        {
            if (inimigoAlvo != null && Runes >= 1)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"[ARREMATE GLACIAL] {Name} consome suas runas para costurar {inimigoAlvo.Name} ao peso de um inverno eterno.");
                Console.ResetColor();
                double modificador = Nodes >= 3 ? 3.7 : 2.5;
                inimigoAlvo.tomarDano(this, (int)Math.Ceiling(AtkTotal() * modificador));
                int dmg = (int)Math.Ceiling(ModTotal() * 2.24 * Runes);
                modificador = Nodes >= 5 ? 0.68 : 0.58;
                bool temNode6 = Nodes >= 6;
                double speedMod = Nodes >= 4 ? 2.87 : 2.67;
                var Boom = new ScribeBoom("Arremate Glacial", Runes, (int)Math.Ceiling(ModTotal() * speedMod), modificador, dmg, temNode6);
                inimigoAlvo.status.RemoveAll(x => x.Name == "Arremate Glacial");
                inimigoAlvo.status.Add(Boom);
                Console.WriteLine($"{inimigoAlvo.Name} recebeu {Boom.Name}. (Diminui a velocidade por {Boom.Duration})");
                Runes = Nodes >= 3 ? 1 : 0;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Runas Atuais: {Runes}/3");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"A agulha treme vazia. {Name} precisa tecer mais fios antes de lançar o arremate");
                Console.ResetColor();
            }
        }

        public override void Passiva()
        {
            int chance = rand.Next(0, 101);
            if (Runes >= 2 && Nodes >= 2 && inimigoAlvo != null || Runes >= 1 && chance <= 80 && inimigoAlvo != null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"[PASSIVA: TORMENTO GÉLIDO]: A mera presença de {Name} evoca um frio congelante que enfraquece os inimigos.");
                Console.ResetColor();
                inimigoAlvo.tomarDano(this, (int)Math.Ceiling(ModTotal() * 0.75 * Runes));
                double modificador = Nodes >= 1 ? 0.28 : 0.13;
                var Res = new DebuffRes("Laçada Gélida Menor", 1, modificador);
                inimigoAlvo.status.Add(Res);
                Console.WriteLine($"{inimigoAlvo.Name} recebeu {Res.Name}. (Recebe +{Res.Mod * 100}% de dano por {Res.Duration} turno(s))");
            }
        }
        public override void Resetar()
        {
            Runes = 0;
            base.Resetar();
        }
    }
}