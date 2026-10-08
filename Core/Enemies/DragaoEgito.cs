using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Enemies
{
    public class DragaoEgito : InimigoBase
    {

        private int fase;
        private PersonagemBase? alvoAnti;
        private int oldHp;
        private bool dormencia = false;
        private int contador;
        private string display_name = "Anomalia Sombria";

        public override int Damage()
        {
            if (fase == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"> {display_name}: ... Logo... Isso chegará ao fim..");
                Console.ResetColor();
            }

            else
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"> {display_name}: *GRRRR-HAAA!* SOFRAAAAA!");
                Console.ResetColor();
            }

            return base.Damage();
        }

        public override void tomarDano(PersonagemBase inimigo, int dano)
        {
            base.tomarDano(inimigo, dano);
            if (fase == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"> {display_name}: ...Inútil...");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"> {display_name}: *HREEE-EE-EHHH!* MAIS... MAIS DOR! VOU USAR SEUS ÓRGÃOS PARA CONSTRUIR UM PALÁCIOOOOOO!");
                Console.ResetColor();
            }

        }

        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            if (useSkill < HabilidadeChance && !dormencia)
            {
                if (fase == 0 && alvos != null)
                {
                    useSkill = rand.Next(1, 101);
                    if (useSkill < 20)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"> [HEMORAAGIA DAS TREVAS] A sombra de {display_name} se expande, corroendo a vida de todos ao redor!");
                        Console.WriteLine($"> {display_name}: ... Tudo... Perecerá...");
                        Console.ResetColor();
                        int hp = 0;
                        foreach (var alvo in alvos)
                        {
                            hp += alvo.HpAtual;
                        }
                        int dano = (int)Math.Ceiling(hp * 0.1);
                        foreach (var alvo in alvos)
                        {
                            alvo.tomarDano(display_name, dano);
                        }
                    }
                    else if (useSkill < 35)
                    {
                        var alvo = EscolherAlvo();
                        var debuff = new DebuffRes("Atrofia Espiritual", 1, 1.2);
                        alvo.status.Add(debuff);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"> [TOQUE DA PODRIDÃO] Um membro da sombra atravessa o peito de {alvo.Name}, infectando sua força!");
                        Console.WriteLine($"> {display_name}: ... Tua força... É meramente... Efêmera...");
                        Console.ResetColor();
                    }
                    else
                    {
                        var alvo = alvos.OrderByDescending(x => x.HpAtual).FirstOrDefault();
                        if (alvo != null)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine($"> [FOCO OBSESSIVO] A silhueta de {display_name} se alonga sobre {alvo.Name} como uma mortalha!");
                            Console.WriteLine($"> {display_name}: ... Tanta vitalidade... Tanta dor para causar...");
                            Console.ResetColor();
                            alvo.tomarDano(display_name, base.Damage() + Mod);
                        }
                    }


                }
                else if (fase == 1)
                {
                    useSkill = rand.Next(1, 101);
                    if (useSkill < 20 && alvos != null)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"> [O FIM ESTÁ PRÓXIMO] {display_name} se enrola em si mesmo, gerando uma crosta de obsidiana e ossos!");
                        Console.WriteLine($"> {display_name}: HAHAHAHAHAHAHAHAHAHAHAHAAAAAAAAA!");
                        Console.ResetColor();
                        Console.WriteLine($"> Destrua o Shield de {display_name} para impedir o fim!");
                        Shield += (int)Math.Ceiling(HpMax * 0.2);
                        dormencia = true;
                        contador = 4;
                    }
                    else if (useSkill < 45)
                    {
                        var alvo = EscolherAlvo();
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"> [EXPURGO DA ORDEM] {display_name} solta um rugido que desintegra as magias de proteção de {alvo.Name}!");
                        Console.WriteLine($"> {display_name}: SUAS BENÇÃOS SÃO MENTIRAS! DEMONSTRE SUA FRAQUEZAAAAAA!");
                        Console.ResetColor();
                        foreach (var efeito in alvo.status.Where(x => x.isBeneficial == true).ToList())
                        {
                            alvo.status.Remove(efeito);
                            Console.WriteLine($"> {display_name} removeu {efeito.Name} de {alvo.Name}");
                        }
                    }
                    else
                    {
                        if (alvos != null)
                        {
                            var maisForte = alvos.OrderByDescending(x => (double)x.HpAtual / x.HpMax).First();
                            var maisFraco = alvos.OrderBy(x => (double)x.HpAtual / x.HpMax).First();

                            double pctForte = (double)maisForte.HpAtual / maisForte.HpMax;
                            double pctFraco = (double)maisFraco.HpAtual / maisFraco.HpMax;

                            maisForte.HpAtual = (int)Math.Ceiling(pctFraco * maisForte.HpMax);
                            maisFraco.HpAtual = (int)Math.Ceiling(pctForte * maisFraco.HpMax);
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.WriteLine($"> [BALANÇA PROFANADA] {display_name} racha o espaço entre {maisForte.Name} e {maisFraco.Name}!");
                            Console.WriteLine($"> {display_name}: *KH-KH-KH...* MORRAM POR DENTRO! TROQUEM DE LUGAR NO MEU ABISMOOOOOOOOOOOO!");
                            Console.ResetColor();
                        }
                    }
                }
            }

        }
        public override void Passiva(User user)
        {
            display_name = (fase == 1) ? "APEP" : Name;
            if (alvos == null || alvos.Count == 0)
                return;
            if (HpAtual <= HpMax * 0.3 && fase != 1)
            {
                fase = 1;
                ConsoleColor[] coresGlitch = { ConsoleColor.DarkGray, ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.DarkMagenta };
                for (int i = 0; i < 6; i++)
                {
                    Console.BackgroundColor = coresGlitch[i % coresGlitch.Length];
                    Console.Clear();
                    Thread.Sleep(100);
                }
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("> A silhueta da criatura de sombras começa a tremer violentamente...");
                Thread.Sleep(1500);

                Console.WriteLine("> O som de estática torna-se ensurdecedor. O ar cheira a ozônio e carne queimada.");
                Thread.Sleep(2000);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("> *CRACK!*");
                Thread.Sleep(800);
                Console.WriteLine("> *SQUELCH!*");
                Thread.Sleep(800);

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("\n> A sombra se rasga de dentro para fora.");
                Console.WriteLine("> Vértebras colossais de obsidiana perfuram o tecido da realidade.");
                Thread.Sleep(2500);

                Console.WriteLine("> O que emerge não é um deus, nem um animal...");
                Thread.Sleep(2000);

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("> Apep emerge: uma serpente de ossos expostos e escamas de estática, com dezenas de olhos desordenados que vazam um lodo púrpura e negro.");
                Console.ResetColor();

                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n***************************************************");
                Console.WriteLine("       APEP: O CAOS PRIMORDIAL DESPERTOU           ");
                Console.WriteLine("***************************************************");
                Console.ResetColor();
                Thread.Sleep(3000);

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"\n> {display_name}: *HREEE-EE-EHHH!* FINALMENTE LIVRE DA ORDEM!");
                Console.WriteLine($"> {display_name}: VOU REESCREVER SUAS ALMAS EM PUREZA CAÓTICAAAAA!");
                Console.ResetColor();
                Thread.Sleep(2000);

                Console.WriteLine("\n> A batalha atinge um novo nível de horror...");
                Thread.Sleep(2000);
            }
            if (dormencia && contador > 0)
            {
                contador -= 1;
            }
            else if (dormencia && contador == 0 && alvos != null && Shield > 0)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [SENTENÇA DE APAGAMENTO] {display_name} desperta de sua crisálida e colapsa a realidade ao redor dos fracos!");
                Console.WriteLine($"> {display_name}: *KH-KH-KH...* DEIXEM DE EXISTIIIIIR!");
                Console.ResetColor();
                foreach (var alvo in alvos)
                {
                    if (alvo.HpAtual < alvo.HpMax * 0.2)
                    {
                        alvo.HpAtual = 0;
                    }
                    else
                    {
                        alvo.tomarDano(display_name, (int)Math.Ceiling(alvo.HpAtual * 0.25));
                    }
                }
                dormencia = false;
            }
            else if (dormencia && contador == 0 && Shield <= 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"> [SONO INTERROMPIDO] A crosta de obsidiana racha! {display_name} urra em agonia e frustração!");
                Console.WriteLine($"> {display_name}: *HREEE-EE-EHHH!* VERMES! VOCÊS APENAS ADIARAM O INEVITÁVEEEEEEL!");
                Console.ResetColor();
                dormencia = false;
            }
            if (fase == 0)
            {
                if (alvoAnti != null && alvoAnti.HpAtual > oldHp)
                {
                    int dano = alvoAnti.HpAtual - oldHp;
                    alvoAnti.tomarDano(display_name, dano);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"> [CAOS PERMANENTE] A cura de {alvoAnti.Name} apodrece instantaneamente!");
                    Console.WriteLine($"> {display_name}: ... Sangue novo... é apenas... mais lodo para queimar...");
                    Console.ResetColor();
                }
                alvoAnti = EscolherAlvo();
                oldHp = alvoAnti.HpAtual;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"> [VÍNCULO SOMBRIO] {display_name} observará a vida de {alvoAnti.Name} a partir de agora.");
                Console.ResetColor();
            }
            else if (fase == 1)
            {
                if (alvoAnti != null && alvoAnti.HpAtual > oldHp)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"> [VAMPIRISMO CAÓTICO] {display_name} abre a boca em seu peito e devora a cura de {alvoAnti.Name}!");
                    Console.WriteLine($"> {display_name}: *GURRR-HAAA!* OBRIGADO PELO BANQUETE! SUA ESPERANÇA ME TORNA ETERNOOOOO!");
                    Console.ResetColor();
                    int cura = alvoAnti.HpAtual - oldHp;
                    HpAtual += cura;
                }
                alvoAnti = EscolherAlvo();
                oldHp = alvoAnti.HpAtual;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"> [CONSUMO CAÓTICO] {display_name} observará a vida de {alvoAnti.Name} a partir de agora.");
                Console.ResetColor();
            }

        }
        public override void aoUsarSkill(PersonagemBase personagem)
        {
            if (fase == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"> [ECO DO VAZIO] A sombra de {display_name} reagem às habilidades de {personagem.Name}!");
                Console.WriteLine($"> {display_name}: ...Tolo... Suas ações são apenas ruído...");
                Console.ResetColor();
                int useSkill = rand.Next(1, 101);
                if (useSkill < 40)
                {
                    personagem.tomarDano(display_name, base.Damage() / 2);
                }
            }
            else if (fase == 1)
            {
                int useSkill = rand.Next(1, 101);
                if (useSkill < 55)
                {
                    personagem.tomarDano(display_name, (int)(base.Damage() * 0.9));

                    int chance = rand.Next(1, 101);
                    if (chance > 40 && chance < 80)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"> [CATARATA ABISSAL] {display_name} vomita uma bile negra que derrete a luz nos olhos de {personagem.Name}!");
                        Console.WriteLine($"> {display_name}: BEBA A ESCURIDÃO! ARRANQUE SEUS OLHOS E ADORE O VAZIOOOOO!");
                        Console.ResetColor();
                        var blind = new Blind("Velo de Estática", 2);
                        personagem.status.Add(blind);
                    }
                    else if (chance < 40)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"> [SUTURA ABISSAL] Tentáculos de sombra invadem a garganta de {personagem.Name}, costurando seus lábios e selando suas habilidades!");
                        Console.WriteLine($"> {display_name}: TEU PODER NÃO É NADA PERANTE MIIIIIIM!");
                        Console.ResetColor();
                        var silence = new Silence("Sutura Abissal", 2);
                        personagem.status.Add(silence);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"> [CAUDA CHICOTE] O corpo de {display_name} chicoteia o espaço, criando uma rajada de ar!");
                        Console.WriteLine($"> {display_name}: *GURRRGH-HAAA!* CALE-SE! SUA EXISTÊNCIA É UM RUÍDO QUE EU VOU ESMAGAAAAAAR!");
                        Console.ResetColor();
                    }
                }
            }
        }

        public override void Resetar()
        {
            fase = 0;
            dormencia = false;
            contador = 0;
            alvoAnti = null;
            oldHp = 0;
            base.Resetar();
        }
    }
}