using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Entities
{
    public class Bennu : PersonagemBase
    {
        private int CooldownAtual;
        private bool ressucitou = false;
        private Random random = new Random();

        public override void tomarDano(string inimigo, int dano)
        {
            if ((Shield > 0 || status.Any(x => x.Name == "Proteção das Chamas")) && inimigoAlvo != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [CALOR DE {Name.ToUpper()}] {inimigoAlvo.Name} se queimou ao tocar em {Name}!");
                Console.ResetColor();
                var debuff = new PoisonMaxStack("Queimadura", 2, Mod);
                inimigoAlvo.status.Add(debuff);
                Console.WriteLine($"{inimigoAlvo.Name} recebeu {debuff.Name}");
                int chance = Nodes >= 4 ? 50 : 40;
                if (random.Next(1, 101) < chance)
                {
                    var cego = new Blind("Cegueira", 2);
                    inimigoAlvo.status.Add(cego);
                }
            }
            if (Nodes >= 6 && (dano * debuffRes) - Shield > HpAtual && !ressucitou)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [RESSURREIÇÃO DA FENIX] {Name} renasce antes que possa morrer!");
                Console.ResetColor();
                CooldownAtual += 2;
                HpAtual = (int)Math.Ceiling(HpMax * 0.3);
                ressucitou = true;
            }
            base.tomarDano(inimigo, dano);
        }
        public override void Habilidade()
        {
            if (CooldownAtual == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [ASAS DO RENASCIMENTO] {Name} Ascende em chamas de proteção!");
                Console.ResetColor();

                double porcentagem = Nodes >= 2 ? 0.4 : 0.3;
                var Res = new BuffRes("Proteção em Chamas", 2, porcentagem);
                status.Add(Res);
                
                Console.WriteLine($"> {Name} Recebe -{(1 - porcentagem) * 100}% de dano.");

                porcentagem = Nodes >= 5 ? 0.35 : 0.25;
                int valorShield = (int)Math.Ceiling(HpMax * porcentagem);
                Shield += valorShield;

                CooldownAtual = Nodes >= 3 ? 2 : 3;

                Console.WriteLine($"> {Name} está radiante! Escudo de {valorShield} gerado.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [FALHA] {Name} está recarregando suas energias e não pode usar sua habilidade novamente por {CooldownAtual} turnos!");
                Console.ResetColor();
            }
        }

        public override void Passiva()
        {
            if (CooldownAtual > 0)
            {
                CooldownAtual -= 1;
            }
            if (HpAtual > HpMax * 0.3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [BRILHO DE {Name.ToUpper()}] {Name} está atraindo a atenção dos inimigos!");
                Console.ResetColor();
                chanceAlvo = Nodes >= 1 ? 300 : 200;
            }
            base.Passiva();
        }

        public override void Resetar()
        {
            CooldownAtual = 0;
            ressucitou = false;
            base.Resetar();
        }
    }
}
