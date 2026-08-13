using System;
using System.Collections.Generic;
using System.Linq;
using SQLitePCL;
using Task_U.Core.StatusEffects;
using Task_U.Models;

namespace Task_U.Core
{
    public class Oni : InimigoBase
    {
        private int fase = 1;
        private int multiplicadorGolpes = 0;

        public override void aplicarEfeitos()
        {
            if (status.Any(x => x.isBeneficial == false))
            {
                if (fase < 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> {Name}: Isso está começando a me irritar....");
                    Console.ResetColor();
                }
                else if(fase == 3)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"> {Name}: ISSO É INÚTIL!");
                    Console.ResetColor();
                    status.RemoveAll(x => x.GetType() == typeof(Stun));
                    status.RemoveAll(x => x.GetType() == typeof(Silence));
                    status.RemoveAll(x => x.GetType() == typeof(Blind));
                }

            }

            base.aplicarEfeitos();
        }

        public override void tomarDano(PersonagemBase inimigo, int dano)
        {
            if (fase == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> {Name}: Isso não vai ficar assim...");
                Console.ResetColor();
            }
            else if (fase == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> {Name}: Arrrrrgh!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"> {Name}: EU VOU TE QUEBRAR EM PEDAÇOS!");
                Console.ResetColor();
            }
            base.tomarDano(inimigo, dano);
        }
        public override int Damage()
        {
            int danoBase = Math.Max(1, Atk + BuffAtk);
            int danoFinal = fase switch
            {
                1 => danoBase,
                2 => (int)Math.Ceiling((danoBase + Mod) * 1.15),
                3 => (int)Math.Ceiling((danoBase + Mod) * 1.5),
                _ => danoBase
            };
            if (fase == 2 && alvos != null)
            {
                int danoReal = Math.Max(0, danoFinal - alvos.First().Shield);
                int cura = (int)Math.Ceiling(danoReal * 1.25);
                HpAtual += cura;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [ROUBO DE SANGUE] {Name} drena vida e recupera {cura} HP!");
                Console.WriteLine($"> {Name}: O seu sangue... ME FORTACELE!");
                Console.ResetColor();
            }


            return danoFinal;
        }

        public override void Passiva(User user)
        {
            if (fase == 1 && HpAtual <= HpMax * 0.7)
            {
                fase = 2;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [FASE 2 – ATAQUE DE IRA] {Name} tem seus olhos e chifres incandescentes em vermelho! Sua espada agora rouba sua energia vital!");
                Console.WriteLine($"> {Name}: Como... Como você OUSA?!");
                Console.ResetColor();
                BuffAtk = 5;
                return;
            }
            else if (fase == 2 && HpAtual <= HpMax * 0.3)
            {
                fase = 3;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"> [FASE 3 – FÚRIA DE ONI] {Name} joga fora sua katana e empunha um enorme machado de guerra! Seus músculos incham com poder demoníaco, suas unhas crescem e viram garras! Ele se torna um monstro.");
                Console.WriteLine($"> {Name}: EU VOU DESPEDAÇAR QUALQUER UM QUE ENTRAR EM MEU CAMINHO!");
                Console.ResetColor();
                BuffAtk = 10;
                return;
            }

            switch (fase)
            {
                case 3:
                    multiplicadorGolpes = Math.Min(multiplicadorGolpes + 25, 80);
                    break;
            }
        }

        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);

            switch (fase)
            {
                case 1:
                    if (useSkill <= HabilidadeChance)
                    {
                        int golpes = rand.Next(2, 6);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"> [CORTE RÁPIDO] {Name} executa {golpes + 1} golpes consecutivos com sua katana!");
                        Console.WriteLine($"> {Name}: Nada escapa a minha lâmina.");
                        Console.ResetColor();
                        for (int i = 0; i < golpes; i++)
                        {
                            PersonagemBase alvo = EscolherAlvo();
                            int danoGolpe = Atk + BuffAtk;
                            alvo.tomarDano(Name, danoGolpe);
                        }
                    }
                    break;

                case 2:
                    if (useSkill <= HabilidadeChance)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"> [LÂMINA DE SANGUE] {Name} rasga o próprio peito para dar um golpe sangrento!");
                        Console.WriteLine($"> {Name}: Ahhhrg! Sofra!");
                        Console.ResetColor();
                        PersonagemBase alvo = EscolherAlvo();
                        int dano = (int)Math.Ceiling((Damage() * 2 + Mod) * alvo.HpAtual / alvo.HpMax * 0.35);
                        alvo.tomarDano(Name, dano);
                    }
                    break;

                case 3:
                    if (useSkill <= HabilidadeChance)
                    {
                        int golpes = 1;
                        if (rand.Next(1, 101) <= multiplicadorGolpes)
                        {
                            golpes = 4;
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"> [MACHADO FURIOSO] {Name} entra em frenesi e desfere QUATRO golpes devastadores!");
                            Console.WriteLine($"> {Name}: MORRA! MORRA! MORRA! MORRAAAAAA!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"> [GOLPE ESMAGADOR] {Name} ataca com toda sua força!");
                            Console.WriteLine($"> {Name}: EU VOU TE DESTRUIR!");
                            Console.ResetColor();
                        }

                        Dictionary<PersonagemBase, int> golpesPorAlvo = new Dictionary<PersonagemBase, int>();
                        for (int i = 0; i < golpes; i++)
                        {
                            PersonagemBase alvo = EscolherAlvo();
                            int danoEsmagador = (int)Math.Max(Math.Ceiling((Damage() + Mod) * 0.35 * (1 - alvo.HpAtual / alvo.HpMax)), 5);
                            if (!golpesPorAlvo.ContainsKey(alvo))
                            {
                                golpesPorAlvo[alvo] = 0;
                            }
                            golpesPorAlvo[alvo]++;
                            int contador = golpesPorAlvo[alvo];
                            danoEsmagador = (int)Math.Ceiling((decimal)danoEsmagador / contador);
                            int danoTotal = danoEsmagador;
                            alvo.tomarDano(Name, danoTotal);
                        }
                    }
                    break;
            }
        }

        public override void Resetar()
        {
            fase = 1;
            multiplicadorGolpes = 0;
            base.Resetar();
        }

        public override PersonagemBase EscolherAlvo()
        {
            if (rand.Next(1, 101) <= 50 && alvos != null && fase == 2 || rand.Next(1, 101) <= 60 && alvos != null && fase == 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [INSTINTO ASSASSINO] {Name} sente o alvo com menos vida!");
                Console.WriteLine($"> {Name}: UM POR UM!");
                Console.ResetColor();
                return alvos.OrderBy(x => x.HpAtual).FirstOrDefault() ?? base.EscolherAlvo();
            }
            return base.EscolherAlvo();
        }
    }
}