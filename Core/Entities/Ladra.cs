using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Entities
{
    public class Ladra : PersonagemBase
    {
        private int pilhagem;
        private bool usouMimica;
        private Random random = new Random();
        private int coolDown;
        private int pity;
        public override int Damage()
        {
            int danoTotal = 0;
            int ataquesExtras = 0;
            int chance = random.Next(1, 101);
            int modChance = Nodes >= 4 ? 70 : Nodes > 1 ? 50 : 40;
            int furtosAtk = 0;
            int furtosShield = 0;
            int danoBase = base.Damage();
            danoTotal += danoBase;

            if (chance < modChance || pity >= 3)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"> [MÃOS LEVES] {Name} se prepara para um assalto...");
                Console.WriteLine($"> {Name}: Não pisca... você pode acabar ficando sem nada.");
                Console.ResetColor();

                string[] falasSaque =
                {
                    "Isso aqui estava sobrando, não estava?",
                    "Ops, acho que você perdeu alguma coisa. Que descuido!",
                    "Achado não é roubado, quem perdeu foi relaxado.",
                    "Olha só, um upgrade de graça!",
                    "Você não está usando isso mesmo...",
                    "Vou levar isso só como recordação!"
                };


                while (inimigoAlvo != null && (inimigoAlvo.BuffAtk > 1 || inimigoAlvo.Shield > 0) && ataquesExtras < 6)
                {
                    string falaSorteada = falasSaque[random.Next(falasSaque.Length)];
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"> {Name} fez um furto! x{ataquesExtras + 1}");
                    Console.WriteLine($"> {Name}: {falaSorteada}");
                    Console.ResetColor();

                    if (inimigoAlvo.BuffAtk > 0)
                    {

                        int mod = inimigoAlvo.BuffAtk / 2;
                        inimigoAlvo.BuffAtk -= mod;
                        BuffAtk += mod;
                        furtosAtk += mod;
                        var buff = new BonusDMG("Vantagem Indevida", 1, mod);
                        var debuff = new DebuffDMG("Saqueado", 1, mod);
                        if (status.Any(x => x.Name == "Vantagem Indevida"))
                        {
                            var old = status.FirstOrDefault(x => x.Name == "Vantagem Indevida");
                            if (old != null)
                            {
                                old.Duration = 1;
                                old.Mod = mod;
                            }
                        }
                        else
                        {
                            status.Add(buff);
                        }
                        if (inimigoAlvo.status.Any(x => x.Name == "Saqueado"))
                        {
                            var old = inimigoAlvo.status.FirstOrDefault(x => x.Name == "Saqueado");
                            if (old != null)
                            {
                                old.Duration = 1;
                                old.Mod = mod;
                            }
                        }
                        else
                        {
                            inimigoAlvo.status.Add(debuff);
                        }

                    }
                    if (inimigoAlvo != null && inimigoAlvo.Shield > 0)
                    {
                        int mod = inimigoAlvo.Shield / 2;
                        Shield += mod;
                        inimigoAlvo.Shield -= mod;
                        furtosShield += mod;
                    }
                    int add = Nodes >= 2 ? 2 : 1;
                    pilhagem = Math.Min(5, pilhagem + add);

                    danoBase = base.Damage();
                    double reducao = Nodes >= 4 ? 0.12 : 0.15;
                    int danoExtra = (int)Math.Ceiling(danoBase * (1 - reducao * (ataquesExtras + 1)));
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"> {Name} deu um ataque furtivo, causando {danoExtra} de dano!");
                    Console.ResetColor();
                    danoTotal += danoExtra;
                    ataquesExtras++;

                    Thread.Sleep(200);
                }


                List<string> furtos = new List<string>();
                if (furtosAtk > 0)
                {
                    furtos.Add($"{furtosAtk} de Atk!");
                }
                if (furtosShield > 0)
                {
                    furtos.Add($"{furtosShield} de Shield!");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                string[] falasFinal =
                {
                    "Tão fácil que chega a ser sem graça.",
                    "Meu. Tudo meu.",
                    "Não faz essa cara, ficou melhor em mim.",
                    "Obrigada pela doação!",
                    "Roubar você é quase um insulto à minha profissão de tão fácil.",
                    "O quê? Isso aqui? Eu já tinha isso quando cheguei, juro!"
                };
                string falaFinal = falasSaque[random.Next(falasFinal.Length)];
                Console.WriteLine($"> {Name} fez uma sequência de {ataquesExtras} furtos! Furtou {string.Join(" e ", furtos)}!");
                Console.WriteLine($"> {Name}: {falaFinal}");
                Console.ResetColor();
                pity = 0;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"> {Name}: Sério? Já acabou?");
                Console.ResetColor();
                pity += 1;
            }
            return danoTotal;
        }

        public override void Habilidade()
        {
            if (coolDown == 0)
            {
                int dano;
                if (Nodes >= 5)
                {
                    int bonus = Mod * pilhagem;
                    if (Nodes >= 5 && pilhagem >= 5)
                    {
                        bonus = (int)Math.Ceiling(bonus * 1.5);
                    }

                    dano = base.Damage() + bonus;
                    if (Nodes >= 6 && !usouMimica && inimigoAlvo != null && pilhagem >= 5)
                    {
                        switch (inimigoAlvo.tipoBoss)
                        {
                            case InimigoBase.TipoBoss.Berserker:
                                dano += Math.Max(10, (int)(1 - (double)inimigoAlvo.HpAtual / inimigoAlvo.HpMax) * 60);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"> [USURPADORA] {Name} furtou a técnica inimiga! O dano desse ataque será baseado na vida perdida!");
                                Console.WriteLine($"> {Name}: Quanto mais o sangue sobe, mais o seu tempo desce!");
                                Console.ResetColor();
                                break;
                            case InimigoBase.TipoBoss.Buffer:
                                if (inimigoAlvo.BuffAtk > 0)
                                {
                                    int roubado = inimigoAlvo.BuffAtk;
                                    BuffAtk += roubado;
                                    inimigoAlvo.BuffAtk = 0;
                                    dano += roubado;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"> [USURPADORA] {Name} rouba o poder do {inimigoAlvo.Name}! ({roubado} de ataque)");
                                    Console.WriteLine($"> {Name}: Você se deu ao trabalho de carregar tudo isso? Deixa que eu levo daqui.");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    dano += Math.Max(10, (1 - BuffAtk / Math.Max(1, inimigoAlvo.BuffAtk)) * 60);
                                }
                                break;
                            case null:
                                break;
                        }
                        usouMimica = true;
                    }
                }
                else
                {
                    dano = base.Damage() + (Mod * pilhagem);
                }


                if (inimigoAlvo != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[RETORNO DE INVESTIMENTOS] {Name} faz um ataque poderoso, com base no que ela pilhou!");
                    Console.ResetColor();
                    inimigoAlvo.tomarDano(this, dano);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"> {Name}: Nada pessoal, são apenas negócios.");
                    Console.WriteLine($"[ACERTO DE CONTAS] {Name} Expõe um ponto fraco, deixando o inimigo vunerável!");
                    Console.ResetColor();
                    var debuffRes = new DebuffRes("Exposto", 2, 0.3);
                    debuffRes.Aplicar(inimigoAlvo);
                    inimigoAlvo.status.Add(debuffRes);
                }
                pilhagem -= Nodes >= 3 ? pilhagem / 2 : pilhagem;
                coolDown = 1;
            }
        }

        public override void Passiva()
        {
            if (coolDown > 0)
            {
                coolDown -= 1;
            }
            if (pilhagem > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"> [EVASÃO ESTRATÉGICA] {Name} se move mais rapidamente, se camuflando no próprio ar.");
                Console.WriteLine($"> {Name}: Continue tentando, quem sabe um dia você me acerta.");
                Console.ResetColor();
                chanceAlvo = 6;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"> [EVASÃO ESTRATÉGICA] {Name} volta a ficar visível claramente ao inimigo");
                Console.WriteLine($"> {Name}: Esforço nota dez, pontaria nota zero. Quer que eu fique parada para facilitar?");
                Console.ResetColor();
                chanceAlvo = 50;
            }
        }

        public override void Resetar()
        {
            usouMimica = false;
            coolDown = 0;
            pilhagem = 0;
            pity = 0;
            base.Resetar();
        }
    }

}
