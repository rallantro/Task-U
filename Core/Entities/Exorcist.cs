using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task_U.Core;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Entities
{
    public class Exorcist : PersonagemBase
    {
        private static readonly Random random = new Random();
        private int acumulo;
        private int pity;
        public override void aplicarEfeitos()
        {
            int chance = random.Next(1, 101);
            if (chance > 50 && pity < 3)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> {Name}: Ahhhh! Quanta energia negativa!");
                Console.ResetColor();
                pity += 1;
                base.aplicarEfeitos();
            }
            else
            {
                if (status.Any(x => x.isBeneficial == false))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> [PASSIVA: SELO DE PROTEÇÃO] O selo de proteção em {Name} se ativou e impediu que ele seja afetado por efeitos negativos nesse turno.");
                    Console.WriteLine($"> {Name}: Eu sou INTOCÁVEL!");
                    Console.ResetColor();
                    acumulo = Math.Min(3, acumulo + 1);
                }
                foreach (var effect in status.Where(x => x.isBeneficial == true).ToList())
                {
                    effect.Aplicar(this);
                }
                status.RemoveAll(e => e.Duration == 0);
                pity = 0;
            }

        }
        public override void Habilidade()
        {

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"> {Name}: Hummm... O que eu devo fazer?");
            Console.WriteLine($"{Name} deve usar o 「Selo da Luz」 ou o 「Selo das Sombras」?");
            Console.ResetColor();
            Console.WriteLine($"(Total: {acumulo}/3 de acúmulos)");
            Console.WriteLine($"1 - 「Selo da Luz」 | 2 - 「Selo das Sombras」");
            Console.WriteLine($"");
            bool escolheu = false;
            while (!escolheu)
            {
                switch (Console.ReadLine())
                {
                    case "1":

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> [SELO DA LUZ] {Name} conjura o「Selo da Luz」para fortalecer e curar!");
                        int easterEgg = random.Next(1, 101);
                        if (easterEgg < 10)
                        {
                            Console.WriteLine($"> {Name}: Ahhhh! Meu olho! Eu sempre esqueço como isso brilha!");
                        }
                        else
                        {
                            Console.WriteLine($"> {Name}: Eu conjuro a luz e a proteção! SOCORRO!");
                        }
                        Console.ResetColor();

                        var heal = new HealFixedStack("Cura Espiritual", 1, (int)Math.Ceiling(ModTotal() * 5.28));
                        if (status.Any(x => x.Name == "Cura Espiritual"))
                        {
                            int indice = status.FindIndex(x => x.Name == "Cura Espiritual");
                            status[indice].Mod = (int)Math.Ceiling(ModTotal() * 5.28);
                            status[indice].Duration = 1;
                        }
                        else
                        {
                            status.Add(heal);
                        }
                        Console.WriteLine($"> {Name} recebeu {heal.Name}. ({heal.Mod} de cura por {heal.Duration} turnos)");

                        var healAlly = new HealFixedStack("Cura Espiritual", 1, (int)Math.Ceiling(ModTotal() * 5.28));
                        if (aliado != null)
                        {
                            if (aliado.status.Any(x => x.Name == "Cura Espiritual"))
                            {
                                int indice = aliado.status.FindIndex(x => x.Name == "Cura Espiritual");
                                aliado.status[indice].Mod = (int)Math.Ceiling(ModTotal() * 5.28);
                                aliado.status[indice].Duration = 1;
                            }
                            else
                            {
                                aliado.status.Add(healAlly);
                            }
                            Console.WriteLine($"> {aliado.Name} recebeu {healAlly.Name}. ({healAlly.Mod} de cura por {healAlly.Duration} turnos)");
                        }


                        var buff = new BonusDMG("Poder Espiritual", 1, ModTotal() * 2);
                        if (status.Any(x => x.Name == "Poder Espiritual"))
                        {
                            int indice = status.FindIndex(x => x.Name == "Poder Espiritual");
                            status[indice].Mod = ModTotal() * 2;
                            status[indice].Duration = 1;
                        }
                        else
                        {
                            status.Add(buff);
                        }
                        Console.WriteLine($"> {Name} recebeu {buff.Name}. ({buff.Mod} de dano adicional por {buff.Duration} turnos)");

                        var buffAlly = new BonusDMG("Poder Espiritual", 1, ModTotal() * 2);
                        if (aliado != null && aliado.status.Any(x => x.Name == "Poder Espiritual"))
                        {
                            int indice = aliado.status.FindIndex(x => x.Name == "Poder Espiritual");
                            aliado.status[indice].Mod = ModTotal() * 2;
                            aliado.status[indice].Duration = 1;
                            Console.WriteLine($"> {aliado.Name} recebeu {buffAlly.Name}. ({buffAlly.Mod} de dano adicional por {buffAlly.Duration} turnos)");
                        }
                        else if (aliado != null)
                        {
                            aliado.status.Add(buffAlly);
                            Console.WriteLine($"> {aliado.Name} recebeu {buffAlly.Name}. ({buffAlly.Mod} de dano adicional por {buffAlly.Duration} turnos)");
                        }


                        acumulo = Math.Min(3, acumulo + 1);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> [EQUILÍBRIO] {Name} absorve poder, através do「Selo da Luz」, fortalecendo a força do「Selo das Sombras」. (Total: {acumulo}/3 de acúmulos)");
                        Console.ResetColor();
                        escolheu = true;
                        break;
                    case "2":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> [SELO DAS SOMBRAS] {Name} conjura o「Selo das Sombras」para enfraquecer o mal!");
                        easterEgg = random.Next(1, 101);
                        if (easterEgg < 40)
                        {
                            Console.WriteLine($"> {Name}: VAI DE RETO COISA RUIM! SAI! SAI!");
                        }
                        else
                        {
                            Console.WriteLine($"> {Name}: Tá sentindo esse peso? É o seu karma ruim te dando um abraço!");
                        }
                        Console.ResetColor();

                        if (inimigoAlvo != null)
                        {
                            var Res = new DebuffRes("Fraqueza Espiritual", Math.Max(acumulo, 2), 1);
                            if (inimigoAlvo.status.Any(x => x.Name == "Fraqueza Espiritual"))
                            {
                                int indice = inimigoAlvo.status.FindIndex(x => x.Name == "Fraqueza Espiritual");
                                inimigoAlvo.status[indice].Duration = Math.Max(acumulo, 1);
                            }
                            else
                            {
                                Res.Aplicar(inimigoAlvo);
                                inimigoAlvo.status.Add(Res);
                            }
                            Console.WriteLine($"> {inimigoAlvo.Name} recebeu {Res.Name}. (Recebe +{1 * 100}% de dano por {Res.Duration} turno(s))");

                            var debuff = new DebuffDMG("Pressão Espiritual", Math.Max(acumulo, 1), Math.Max((int)Math.Ceiling(acumulo * ModTotal() * 3.24), 1));
                            if (inimigoAlvo.status.Any(x => x.Name == "Pressão Espiritual"))
                            {
                                int indice = inimigoAlvo.status.FindIndex(x => x.Name == "Pressão Espiritual");
                                inimigoAlvo.status[indice].Mod = Math.Max((int)Math.Ceiling(acumulo * ModTotal() * 3.24), 1);
                                inimigoAlvo.status[indice].Duration = Math.Max(acumulo, 1);
                            }
                            else
                            {
                                inimigoAlvo.status.Add(debuff);
                            }
                            Console.WriteLine($"> {inimigoAlvo.Name} recebeu {debuff.Name}. (-{debuff.Mod} de dano no ataque por {debuff.Duration} turnos)");
                        }

                        acumulo = Math.Max(0, acumulo - 2);

                        escolheu = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> {Name}: ... CALMA AÍ... Eu to pensando...");
                        Console.ResetColor();
                        break;
                }
            }
        }

        public override void Passiva()
        {
            if (aliado != null && aliado.HpAtual < HpAtual)
            {
                if (aliado.status.Count > 0)
                {
                    var debuffsAlly = aliado.status.Where(p => p.isBeneficial == false).ToList();
                    if (debuffsAlly.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> [PASSIVA] {Name} purifica a energia negativa em seu aliado");
                        Console.WriteLine($"> {Name}: SAI DO {aliado.Name.ToUpper()}, ENERGIA RUIM!");
                        Console.ResetColor();
                        int indexAlly = random.Next(debuffsAlly.Count);
                        var efeito = debuffsAlly[indexAlly];
                        int realIndex = aliado.status.IndexOf(efeito);
                        Console.WriteLine($"> {Name} purificou {aliado.Name}, retirando {aliado.status[realIndex].Name}.");
                        aliado.status.RemoveAt(realIndex);

                        var buff = new BonusDMG("Fortalecimento Espiritual", 2, Atk);
                        aliado.status.Add(buff);
                        acumulo = Math.Min(3, acumulo + 1);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> [EQUILÍBRIO] {Name} absorve poder das energias negativas liberadas, fortalecendo a força do「Selo das Sombras」. (Total: {acumulo}/3 de acúmulos)");
                        Console.WriteLine($"> {Name}: Equilíbrio é importante: 50% de luz, 50% de sombra e 100% de chance de eu sair ganhando no final!");
                        Console.ResetColor();
                    }
                }
            }
            else
            {
                if (status.Count > 0)
                {
                    var debuffs = status.Where(p => p.isBeneficial == false).ToList();
                    if (debuffs.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> [PASSIVA] {Name} purifica a energia negativa em si mesmo");
                        Console.WriteLine($"> {Name}: AH! VAI DE RETO! SAI DE MIM COISA RUIM!");
                        Console.ResetColor();
                        int index = random.Next(debuffs.Count);
                        var efeito = debuffs[index];
                        int realIndex = status.IndexOf(efeito);
                        Console.WriteLine($"> {Name} se purificou, retirando {status[realIndex].Name}.");
                        status.RemoveAt(realIndex);

                        var buff = new BonusDMG("Fortalecimento Espiritual Menor", 2, Atk * 3 / 4);
                        if (status.Any(x => x.Name == "Fortalecimento Espiritual Menor"))
                        {
                            int indice = status.FindIndex(x => x.Name == "Fortalecimento Espiritual Menor");
                            status[indice].Duration = 2;
                        }
                        else
                        {
                            status.Add(buff);
                        }
                        acumulo = Math.Min(3, acumulo + 1);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> [EQUILÍBRIO] {Name} absorve poder das energias negativas liberadas, fortalecendo a força do「Selo das Sombras」. (Total: {acumulo}/3 de acúmulos)");
                        Console.WriteLine($"> {Name}: Dizem que o equilíbrio exige sacrifício. Você sacrifica sua dignidade e eu fico mais forte. É a lei da oferta e da procura!");
                        Console.ResetColor();
                    }


                }
            }
            if (inimigoAlvo != null && inimigoAlvo.status.Count > 0)
            {

                if (aliado != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> [RESSONÂNCIA DE CARMA] {Name} absorve a energia perdida pelo inimigo.");
                    Console.WriteLine($"> {Name}: Tudo o que vai, sempre vai- NÃO! Tudo o que vem, vai- PERA! EU SEI FALAR BONITO EU JURO!");
                    Console.ResetColor();
                    var buffAlly = new BonusDMG("Fortalecimento Espiritual Maior", 2, Atk);
                    if (aliado.status.Any(x => x.Name == "Fortalecimento Espiritual Maior"))
                    {
                        int indice = aliado.status.FindIndex(x => x.Name == "Fortalecimento Espiritual Maior");
                        aliado.status[indice].Duration = 2;
                    }
                    else
                    {
                        aliado.status.Add(buffAlly);
                    }
                    Console.WriteLine($"> {aliado.Name} recebeu {buffAlly.Name}. (+{buffAlly.Mod} de dano no ataque por {buffAlly.Duration} turnos)");
                }

                var buff = new BonusDMG("Fortalecimento Espiritual Maior", 2, Atk);
                if (status.Any(x => x.Name == "Fortalecimento Espiritual Maior"))
                {
                    int indice = status.FindIndex(x => x.Name == "Fortalecimento Espiritual Maior");
                    status[indice].Duration = 2;
                }
                else
                {

                    status.Add(buff);
                }
                Console.WriteLine($"> {Name} recebeu {buff.Name}. (+{buff.Mod} de dano no ataque por {buff.Duration} turnos)");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> {Name}: Luz pra mim, sombra pra você... Viu? O equilíbrio perfeito!");
                Console.ResetColor();
            }
        }

        public override void Resetar()
        {
            acumulo = 0;
            pity = 0;
            base.Resetar();
        }
    }
}