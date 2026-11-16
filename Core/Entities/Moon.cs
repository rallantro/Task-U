using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;
using Task_U.Data;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Entities
{
    public class Moon : PersonagemBase
    {
        private Random rand = new Random();
        private int FaseAtual = 1;
        private int FaseEscolhida;
        private int Harmonia;
        private double modificador;


        public override int Damage()
        {
            if (inimigoAlvo != null)
            {
                var OldMark = inimigoAlvo.status.OfType<MoonMark>().FirstOrDefault();
                if (OldMark != null && FaseAtual != 3)
                {
                    int Stacks = OldMark.Stacks;

                    if (FaseAtual == 1)
                    {
                        modificador = Nodes >= 4 ? 1.45 : 0.84;
                        int dano = (int)Math.Ceiling(AtkTotal() * (modificador * Stacks * Stacks) + inimigoAlvo.HpMax * (AtkTotal() * 0.00065) * Stacks);
                        modificador = Nodes >= 5 ? 0.028 : 0.046;
                        inimigoAlvo.status.Remove(OldMark);
                        int autoDano = (int)Math.Ceiling(HpMax * modificador * Stacks);
                        string[] dialogos =
                        {
                                $"> Shun: EXPLOSÃO CRESCENTE!",
                                $"> Shun: EXPLOSÃO CRESCENTE! \r\n> Shion: P-por que eu sou nerd mas isso não?",
                                $"> Shun: ASSIM COMO A LUA, MEU PODER CRESCE!",
                                $"> Shun: ASSIM COMO A LUA, MEU PODER CRESCE! \r\n> Shion: Você tá se machucando!",
                                $"> Shun: EXPLODAAAAAA!",
                                $"> Shun: EXPLODAAAAAA! \r\n> Shion: I-isso não foi demais?!",
                            };
                        string dialogoEscolhido = dialogos[rand.Next(0, 6)];
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"> [CONSUMIR CRESCENTE] {Name} aperta o punho, estourando {Stacks} Marcas Lunares em {inimigoAlvo.Name} causando dano adicional e consumindo {autoDano} pontos de vida!");
                        foreach (var linha in dialogoEscolhido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            Falar(linha.Trim());
                        }
                        HpAtual = Math.Max(1, HpAtual - autoDano);
                        Harmonia += OldMark.Stacks;

                        return dano;

                    }
                    else if (FaseAtual == 2)
                    {
                        modificador = Nodes >= 2 ? 0.25 : 0.15;
                        int cura = (int)Math.Ceiling(AtkTotal() * (modificador * Stacks * Stacks));
                        Harmonia += Stacks;
                        inimigoAlvo.status.Remove(OldMark);
                        string[] dialogos =
                        {
                                $"> Shion: E-esse é o poder da lua Minguante!",
                                $"> Shion: E-esse é o poder da lua Minguante! \r\n> Shun: Top 10 frases mais nerds que você já disse.",
                                $"> Shion: P-preciso proteger meu irmão!",
                                $"> Shion: P-preciso proteger meu irmão! \r\n> Shun: NÃO PRECISA NÃO!",
                                $"> Shion: Brilho minguante!",
                                $"> Shion: Brilho minguante! \r\n> Shun: Isso nem faz sentido!",
                            };
                        string dialogoEscolhido = dialogos[rand.Next(0, 6)];
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> [CONSUMIR MINGUANTE] {Name} faz um movimento suave e puxa {Stacks} Marcas Lunares para a equipe!");
                        foreach (var linha in dialogoEscolhido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            Falar(linha.Trim());
                        }
                        Console.WriteLine($"> {Name} se curou em {cura} pontos!");
                        curar(Name, cura);
                        if (aliado != null)
                        {
                            aliado.curar(Name, cura);
                            Console.WriteLine($"> {Name} curou {aliado.Name} em {cura} pontos!");
                        }
                        return base.Damage();
                    }
                    else
                    {
                        return base.Damage();
                    }
                }
                else if (FaseAtual == 3)
                {
                    int bonusDano = (int)Math.Ceiling(AtkTotal() * 14.25 * (1 - (double)inimigoAlvo.HpAtual / Math.Max(1, inimigoAlvo.HpMax)));
                    int dano = (int)Math.Ceiling(AtkTotal() * 14.25);
                    int cura = (int)Math.Ceiling(dano * 0.21);
                    int shield = HpAtual + cura - HpMax;
                    string[] dialogos =
                        {
                                $"> Shion: É CHEGADA \r\n> Shun: A HORA!",
                                $"> Shun: O BRILHO \r\n> Shion: É ETERNO!",
                                $"> Shun: SINTA A FORÇA \r\n> Shion: DO BRILHO LUNAR!",
                                $"> Shion: A LUA CLAMA \r\n> Shun: E NÓS AGIMOS!"
                            };
                    string dialogoEscolhido = dialogos[rand.Next(0, 4)];
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"> [RAJADA LUNAR] {Name} estende a mão e uma enorme rajada de pura energia prateada explode contra {inimigoAlvo.Name}!");
                    Console.WriteLine(dialogoEscolhido);
                    Console.WriteLine($"> [RAJADA LUNAR] O brilho traz energia e cura {Name} em {cura} pontos!");
                    Console.ResetColor();
                    if (shield > 0)
                    {
                        Console.WriteLine($"> [RAJADA LUNAR] Um escudo de energia prateada se forma em volta de {Name} e o protegerá de {shield} de dano!");
                        Shield += shield;
                    }
                    curar(Name, cura);
                    if (aliado != null)
                    {
                        int shieldAliado = aliado.HpAtual + cura - aliado.HpMax;
                        Console.WriteLine($"> [RAJADA LUNAR] O brilho traz energia e cura {aliado.Name} em {cura} pontos!");
                        if (shieldAliado > 0)
                        {
                            Console.WriteLine($"> [RAJADA LUNAR] Um escudo de energia prateada se forma em volta de {aliado.Name} e o protegerá de {shieldAliado} de dano!");
                            aliado.Shield += shieldAliado;
                        }
                        aliado.curar(Name, cura);
                    }
                    return dano + bonusDano;
                }
            }
            return base.Damage();
        }

        public override void Habilidade()
        {
            if (FaseAtual != 4 && FaseAtual != 3)
            {

                modificador = Nodes >= 1 ? 1.88 : 1.64;
                int dano;
                if (FaseAtual == 1)
                {
                    dano = (int)Math.Ceiling(AtkTotal() * modificador);
                }
                else
                {
                    dano = (int)Math.Ceiling(ModTotal() * modificador * 1.65);
                }
                if (inimigoAlvo != null)
                {
                    var OldMark = inimigoAlvo.status.OfType<MoonMark>().FirstOrDefault();
                    if (OldMark != null)
                    {
                        modificador = Nodes >= 3 ? 4 : 3;
                        if (OldMark.Stacks < modificador)
                        {
                            OldMark.Stacks++;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"> O inimigo já está com Marcas Lunares o suficiente! ({(OldMark != null ? OldMark.Stacks : 1)}/{(Nodes >= 3 ? 4 : 3)} Marcas Lunares)");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        var mark = new MoonMark("Marca da Lua", 1, 1);
                        inimigoAlvo.status.Add(mark);
                    }
                    if (FaseAtual == 1)
                    {
                        string[] dialogos =
                        {
                                $"> Shun: EU VOU ADORAR TE CORTAR EM PEDAÇOS!",
                                $"> Shun: EU VOU ADORAR TE CORTAR EM PEDAÇOS! \r\n> Shion: I-isso não é um exagero?",
                                $"> Shun: AHH EU VOU GOSTAR DE FAZER ISSO!",
                                $"> Shun: AHH EU VOU GOSTAR DE FAZER ISSO!\r\n> Shion: T-toma cuidado!",
                                $"> Shun: CORTAR, CORTAR, CORTAAAAAR!",
                                $"> Shun: CORTAR, CORTAR, CORTAAAAAR! \r\n> Shion: Ei, calma aí-",
                            };
                        string dialogoEscolhido = dialogos[rand.Next(0, 6)];
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"> [BRILHO DA LUA] {Name} usa sua foice lunar para rasgar e marcar {inimigoAlvo.Name}! ({(OldMark != null ? OldMark.Stacks : 1)}/{(Nodes >= 3 ? 4 : 3)} Marcas Lunares)");
                        Console.WriteLine($"> [LUA CRESCENTE] A foice emana um brilho prateado que torna o golpe mais brutal!");
                        foreach (var linha in dialogoEscolhido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            Falar(linha.Trim());
                        }
                        inimigoAlvo.tomarDano(this, dano);
                    }
                    else
                    {
                        string[] dialogos =
                        {
                                $"> Shion: Q-que a luz nos ilumine!",
                                $"> Shion: Q-que a luz nos ilumine! \r\n> Shun: Papo de nerd.",
                                $"> Shion: E-eu vou te proteger, Shun!",
                                $"> Shion: E-eu vou te proteger, Shun! \r\n> Shun: EU NÃO PRECISO SER PROTEGIDO, QUEM PRECISA É VOCÊ!",
                                $"> Shion: A luz prateada, retorna ao dono!",
                                $"> Shion: A luz prateada, retorna ao dono! \r\n> Shun: Precisa mesmo falar assim?",
                            };
                        string dialogoEscolhido = dialogos[rand.Next(0, 6)];
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> [BRILHO DA LUA] {Name} ergue a mão e o brilho prateado marca {inimigoAlvo.Name}! ({(OldMark != null ? OldMark.Stacks : 1)}/{(Nodes >= 3 ? 4 : 3)} Marcas Lunares)");
                        Console.WriteLine($"> [LUA MINGUANTE] O brilho reflete e cura {Name} em {(int)Math.Ceiling(dano * 1.25)}!");
                        foreach (var linha in dialogoEscolhido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            Falar(linha.Trim());
                        }
                        inimigoAlvo.tomarDano(this, (int)Math.Ceiling(dano * 0.35));
                        curar("Shion", (int)Math.Ceiling(dano * 1.25));
                    }

                }
            }
            else
            {
                if (FaseAtual == 4)
                {
                    if (Nodes >= 6)
                    {
                        Console.WriteLine("[MUDANÇA DE FASE] A Lua Nova está prestes a mudar... Qual será a próxima fase da lua?");
                        Console.WriteLine($"1 - 「Crescente」 | 2 - 「Minguante」");
                        bool escolheu = false;
                        while (!escolheu)
                        {
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    FaseEscolhida = 1;
                                    string[] dialogos =
                                    {
                                            $"> Shion: P-perai eu vou curar- \r\n> Shun: ESPERA NADA, EU VOU NA FRENTE!",
                                            $"> Shion: Shun, eu aguento... \r\n> Shun: Até parece. Descansa!",
                                            $"> Shun: DEIXA COMIGO! \r\n> Shion: Perai, ei, Shun!",
                                            $"> Shun: Mais velhos na frente! \r\n> Shion: P-perai, nós somos gêmeos!",
                                        };
                                    string dialogoEscolhido = dialogos[rand.Next(0, 4)];
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.WriteLine("[MUDANÇA DE FASE] A Lua Nova irá se tornar uma Lua Crescente!");
                                    Console.WriteLine(dialogoEscolhido);
                                    escolheu = true;
                                    break;
                                case "2":
                                    FaseEscolhida = 2;
                                    string[] dialogos2 =
                                    {
                                            $"> Shion: Eu vou nos proteger! \r\n> Shun: ARGH! TÁ VAI FAZ AÍ O QUE VOCÊ QUER!",
                                            $"> Shion: Shun, me deixa te ajudar! \r\n> Shun: ... Tá vai!",
                                            $"> Shun: ESTOU PRONTO PRA MATAR! \r\n> Shion: Não está não... Me deixa ir na sua frente!",
                                            $"> Shun: Por que você insiste tanto em ir primeiro? \r\n> Shion: Porque eu sou a proteção!",
                                        };
                                    dialogoEscolhido = dialogos2[rand.Next(0, 4)];
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.WriteLine("[MUDANÇA DE FASE] A Lua Nova irá se tornar uma Lua Minguante!");
                                    Console.WriteLine(dialogoEscolhido);
                                    escolheu = true;
                                    break;
                                default:
                                    string[] dialogos3 =
                                    {
                                            $"> Shion: Eu vou primeiro... \r\n> Shun: NÃO! EU VOU!",
                                            $"> Shion: Shun, defesa é tão importante quando ataque! \r\n> Shun: Mas o ataque ataca, dã!",
                                            $"> Shun: Para de ser um nerd e me deixa ir na frente! \r\n> Shion: Eu não sou nerd e eu preciso ir na frente!",
                                            $"> Shun: EU VOU NA FRENTE SAI! \r\n> Shion: Não, eu que vou!",
                                            $"> Shun: EU TÔ MAIS DESCANSADO QUE VOCÊ! \r\n> Shion: Não tá não! Eu que estou!",
                                        };
                                    dialogoEscolhido = dialogos3[rand.Next(0, 5)];
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.WriteLine("[MUDANÇA DE FASE] A Lua Nova está indecisa! (Escolha uma opção válida)");
                                    Console.WriteLine($"1 - 「Crescente」 | 2 - 「Minguante」");
                                    Console.WriteLine(dialogoEscolhido);
                                    break;
                            }
                        }

                    }
                    else
                    {
                        string[] dialogos =
                        {
                                $"> Shion: P-perai eu vou curar- \r\n> Shun: Para de se forçar, irmãozinho. A gente aguenta um pouco.",
                                $"> Shion: Estou muito cansado... \r\n> Shun: Nós estamos... Relaxa um pouco!",
                                $"> Shion: Mal consigo me mexer... \r\n> Shun: Claro que não, eu tô acabado...",
                                $"> Shun: EU TÔ MUITO CANSADO! \r\n> Shion: Tudo bem meu irmão!",
                                $"> Shun: Deixa o irmão mais velho cuidar de você! \r\n> Shion: Mas nós somos gêmeos!",
                            };
                        string dialogoEscolhido = dialogos[rand.Next(0, 5)];
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"> [BRILHO DA LUA NOVA] {Name} está exausta e não consegue usar suas habilidades.");
                        foreach (var linha in dialogoEscolhido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            Falar(linha.Trim());
                        }
                    }

                }
                else
                {
                    string[] dialogos =
                        {
                                $"> Shion: A LUA \r\n> Shun: JÁ ESTÁ CHEIA",
                                $"> Shun: NÃO HÁ NECESSIDADE \r\n> Shion: DE MAIS MARCAS",
                                $"> Shun: O PODER \r\n> Shion: JÁ ESTÁ EM NOSSAS MÃOS",
                                $"> Shion: CONTEMPLE O \r\n> Shun: BRILHO DA LUA"
                            };
                    string dialogoEscolhido = dialogos[rand.Next(0, 4)];
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"> [BRILHO DA LUA CHEIA] {Name} já está brilhando, e alcançou o seu poder total.");
                    Console.WriteLine(dialogoEscolhido);
                }
            }
        }

        public override void Passiva()
        {
            if (Harmonia < 5)
            {
                if (FaseAtual == 1)
                {
                    string[] dialogos =
                    {
                            $"> Shun: O nerd do meu irmão tá com papo de proteção... EU JÁ VOLTO! \r\n> Shion: E-ei! Proteção não é bobagem, sabia?",
                            $"> Shion: Deixa eu assumir um pouquinho... \r\n> Shun: Que?! Agora que tava ficando legal... Tá vai.",
                            $"> Shun: EU AGUENTO, VEM ME BATE MAIS! \r\n> Shion: Calma, meu irmão, me deixe curar.",
                            $"> Shion: E a lua estará em diminuendo! \r\n> Shun: Você... Pratica essas falas chiques?"
                        };
                    string dialogoEscolhido = dialogos[rand.Next(0, 4)];
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> [PASSIVA: FASES DA LUA] {Name} muda para a lua Minguante (Harmonia: {Harmonia})");
                    foreach (var linha in dialogoEscolhido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Falar(linha.Trim());
                    }
                    FaseAtual = 2;
                }
                else if (FaseAtual == 2)
                {
                    string[] dialogos =
                    {
                            $"> Shun: É minha vez, ME DÁ O CONTROLE! \r\n> Shion: Tá bom, tá bom, não precisa empurrar!",
                            $"> Shion: Acho que precisamos de mais ofensiva... \r\n> Shun: DEIXA COMIGO MANINHO!",
                            $"> Shun: EI! QUEM DEIXOU VOCÊ ENCOSTAR NO MEU IRMÃOZINHO? \r\n> Shion: E-eu tô bem!",
                            $"> Shun: A LUA ESTÁ CRESCENDO! \r\n> Shion: Estarei ao seu lado, nas sombras, meu irmão..."
                        };
                    string dialogoEscolhido = dialogos[rand.Next(0, 4)];
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"> [PASSIVA: FASES DA LUA] {Name} muda para a lua Crescente (Harmonia: {Harmonia})");
                    foreach (var linha in dialogoEscolhido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Falar(linha.Trim());
                    }
                    FaseAtual = 1;
                }
                else if (FaseAtual == 3)
                {
                    string[] dialogos =
                    {
                            $"> Shun: Preciso descansar... \r\n> Shion: E-eu também...",
                            $"> Shion: A lua cheia... \r\n> Shun: Descansa agora...",
                            $"> Shun: A lua nova, \r\n> Shion: vem para descansar...",
                            $"> Shun: Estou cansado... \r\n> Shion: Logo voltaremos a brilhar, meu irmão."
                        };
                    string dialogoEscolhido = dialogos[rand.Next(0, 4)];
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"> [PASSIVA: FASES DA LUA] {Name} muda para a lua Nova (Harmonia: {Harmonia})");
                    foreach (var linha in dialogoEscolhido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Falar(linha.Trim());
                    }
                    FaseAtual = 4;
                }
                else if (FaseAtual == 4)
                {
                    if (Nodes >= 6 && FaseEscolhida != 0)
                    {
                        FaseAtual = FaseEscolhida;
                        if (FaseEscolhida == 2)
                        {
                            string[] dialogos =
                            {
                                $"> Shun: O nerd do meu irmão tá com papo de proteção... EU JÁ VOLTO! \r\n> Shion: E-ei! Proteção não é bobagem, sabia?",
                                $"> Shion: Deixa eu assumir um pouquinho... \r\n> Shun: Que?! Agora que tava ficando legal... Tá vai.",
                                $"> Shun: EU AGUENTO, VEM ME BATE MAIS! \r\n> Shion: Calma, meu irmão, me deixe curar.",
                                $"> Shion: E a lua estará em diminuendo! \r\n> Shun: Você... Pratica essas falas chiques?"
                                };
                            string dialogoEscolhido = dialogos[rand.Next(0, 4)];
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"> [PASSIVA: FASES DA LUA] {Name} muda para a lua Minguante (Harmonia: {Harmonia})");
                            foreach (var linha in dialogoEscolhido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                Falar(linha.Trim());
                            }
                            FaseEscolhida = 0;
                        }
                        else
                        {
                            string[] dialogos =
                            {
                                $"> Shun: É minha vez, ME DÁ O CONTROLE! \r\n> Shion: Tá bom, tá bom, não precisa empurrar!",
                                $"> Shion: Acho que precisamos de mais ofensiva... \r\n> Shun: DEIXA COMIGO MANINHO!",
                                $"> Shun: EI! QUEM DEIXOU VOCÊ ENCOSTAR NO MEU IRMÃOZINHO? \r\n> Shion: E-eu tô bem!",
                                $"> Shun: A LUA ESTÁ CRESCENDO! \r\n> Shion: Estarei ao seu lado, nas sombras, meu irmão..."
                                };
                            string dialogoEscolhido = dialogos[rand.Next(0, 4)];
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine($"> [PASSIVA: FASES DA LUA] {Name} muda para a lua Crescente (Harmonia: {Harmonia})");
                            foreach (var linha in dialogoEscolhido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                Falar(linha.Trim());
                            }
                        }
                    }
                    else
                    {
                        string[] dialogos =
                        {
                            $"> Shun: Como você se recupera tão rápido? \r\n> Shion: A-acho que é um talento natural?",
                            $"> Shion: Descanse um pouco meu irmão... \r\n> Shun: EU TÔ ÓTIMO... Mas beleza.",
                            $"> Shun: SHION, ASSUME AÍ... Eu preciso... De um tempo \r\n> Shion: A lua descansa, mas eu não...",
                            $"> Shion: A lua minguante trará a calma e a proteção... \r\n> Shun: Nerd, nerd, nerd."
                            };
                        string dialogoEscolhido = dialogos[rand.Next(0, 4)];
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> [PASSIVA: FASES DA LUA] {Name} muda para a lua Minguante (Harmonia: {Harmonia})");
                        foreach (var linha in dialogoEscolhido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            Falar(linha.Trim());
                        }
                        FaseAtual = 2;
                    }
                }
            }
            else
            {
                string[] dialogos =
                {
                            $"> Shun: A LUA ESTÁ \r\n> Shion: SEMPRE COMPLETA!",
                            $"> Shion: AS DUAS FACES \r\n> Shun: SE UNEM!",
                            $"> Shun: MINGUANTE OU CRESCENTE \r\n> Shion: ELA SEMPRE ESTÁ COMPLETA!",
                            $"> Shion: Eu e você, \r\n> Shun: Sempre contra o mundo!",
                            $"> Shion: ENQUANTO HOUVER TREVAS \r\n> Shun: HAVERÁ LUZ!",
                            $"> Shun: FINALMENTE CHEGOU \r\n> Shion: A LUA CHEIA!"
                    };
                Console.ForegroundColor = ConsoleColor.White;
                string dialogoEscolhido = dialogos[rand.Next(0, 6)];
                Console.WriteLine($"> [PASSIVA: FASES DA LUA] Os olhos da {Name} brilham, seus cabelos se esvoaçam, enquanto emanam um brilho prateado... A LUA ESTÁ CHEIA!");
                Console.WriteLine(dialogoEscolhido);
                Console.ResetColor();
                FaseAtual = 3;
                Harmonia = 0;
            }
        }


        void Falar(string linha)
        {
            if (linha.StartsWith("> Shun:"))
                Console.ForegroundColor = ConsoleColor.Magenta;
            else if (linha.StartsWith("> Shion:"))
                Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(linha);
            Console.ResetColor();
        }

        public override void Resetar()
        {
            FaseEscolhida = 0;
            FaseAtual = 1;
            Harmonia = 0;
            base.Resetar();
        }

    }
}