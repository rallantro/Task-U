using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Entities
{
    public class Fire : PersonagemBase
    {

        private double Carga;
        private int bonus;
        private double modificador;
        private int porcentagem;
        private bool chama;
        private Random rand = new Random();

        public override int Damage()
        {
            int chance = rand.Next(1, 101);
            if (chama)
            {
                chance += 15;
            }
            modificador = Nodes >= 2 ? 5.78 : 4.88;
            porcentagem = (int)Math.Ceiling(AtkTotal() * modificador);
            if (chance <= porcentagem && inimigoAlvo != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [SEGUNDA BRASA] desfere um segundo golpe contra {inimigoAlvo.Name}!");
                string[] conjuntoFalas =
                {
                    "E o povo pede bis!",
                    "Segundo ato!",
                    "Olha a reprise!",
                    "E a platéia pede bis!",
                    "Mais quente agora!"
                };
                string falaSorteada = conjuntoFalas[rand.Next(conjuntoFalas.Length)];
                Console.WriteLine($"> {Name}: {falaSorteada}");
                Console.ResetColor();
                modificador = Nodes >= 1 ? 0.72 : 0.54;
                int dano = (int)Math.Ceiling(AtkTotal() * modificador);
                inimigoAlvo.tomarDano(this, dano);
                AplicarChama();
            }
            AplicarChama();
            modificador = Nodes >= 3 ? 0.98 : 0.78;
            Carga = Math.Min(100, Carga + Math.Ceiling((AtkTotal() + ModTotal()) * 0.5 * modificador));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"> (Clímax Ígneo: {Carga}/100)");
            if (Carga >= 65)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                string[] conjuntoFalas =
                {
                    "CHEGOU A HORA DO GRAN FINALE!",
                    "ESTOU PRONTO!",
                    "AGRADEÇAM PELO SHOW A SEGUIR!",
                    "JÁ ESTÁ NA HORA!",
                    "A ATRAÇÃO PRINCIPAL ESTÁ PRONTA!",
                    "O CLÍMAX JÁ ESTÁ PRONTO"
                };
                string falaSorteada = conjuntoFalas[rand.Next(conjuntoFalas.Length)];
                Console.WriteLine($"> {Name}: {falaSorteada} ({Name} poderá usar [Labareda Catártica] na sua próxima habilidade)");
            }
            else if (Carga >= 48)
            {
                chance = rand.Next(1, 101);
                string[] conjuntoFalas =
                {
                    "A temperatura tá subindo!",
                    "Quase no ponto de ebulição!",
                    "O palco tá fervendo!",
                    "Sintam a pressão acumulando!",
                    "Preparando o grande número!",
                    "Segurem-se, vai estourar!",
                    "Quase pronto para o show!"
                };
                Console.ForegroundColor = ConsoleColor.Red;
                string falaSorteada = conjuntoFalas[rand.Next(conjuntoFalas.Length)];
                Console.WriteLine($"> {Name}: {falaSorteada}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                string[] conjuntoFalas =
                {
                    "A energia está subindo!",
                    "Mais aplausos!",
                    "O público está vibrando!",
                    "Mais animação!",
                    "Estou só me aquecendo!",
                    "O melhor ainda está por vir!"
                };
                string falaSorteada = conjuntoFalas[rand.Next(conjuntoFalas.Length)];
                Console.WriteLine($"> {Name}: {falaSorteada}");
            }
            Console.ResetColor();
            return base.Damage() + bonus;
        }

        public override void Habilidade()
        {
            if (Carga >= 65 && inimigoAlvo != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [LABAREDA CATÁRTICA] {Name} bate ergue sua espada e conjura uma labareda enorme!");
                string[] conjuntoFalas =
                {
                    "E PARA O GRAN FINALE - QUEIME!",
                    "PALMAS PARA MINHA CHAMA ABSOLUTA!",
                    "ISSO É PELO MEU REINO!",
                    "HORA DO CLÍMAX!",
                    "QUEIME SOB MINHA ORDEM!",
                    "ESTE SERÁ SEU FIM!",
                    "PELO PODER DA CHAMA!"
                };
                string falaSorteada = conjuntoFalas[rand.Next(conjuntoFalas.Length)];
                Console.WriteLine($"> {Name}: {falaSorteada}");
                Console.WriteLine($"> (Clímax Ígneo foi consumido.)");
                Console.ResetColor();
                modificador = Nodes >= 5 ? 0.025 : 0.018;
                int dano = (int)Math.Ceiling(((AtkTotal() + bonus) * 3.76 + (ModTotal() + bonus) * 5.76) * Carga * modificador);
                inimigoAlvo.tomarDano(this, dano);
                Carga = 0;
                if (Nodes >= 6)
                {
                    int vezes = rand.Next(3, 8);
                    int x = 0;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> [INCINERAR] A chama de {Name} é absoluta! (Aplicará {vezes} queimaduras)");
                    Console.ResetColor();
                    while (x < vezes)
                    {
                        AplicarChama();
                        x++;
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [FORMA FLAMEJANTE] {Name} revela sua forma em brasa! Seus cabelos queimam e brilham como o sol! (+15% de chance de aplicar queimadura e de dar um segundo)");
                string[] conjuntoFalas =
                {
                    "Fogo no palco!",
                    "Holofotes em mim!",
                    "Hora de brilhar!",
                    "A pirotecnia é minha!",
                    "A cortina sobe, eu brilho!",
                    "Eu sou o show!",
                    "Pirotecnia!"
                };
                string falaSorteada = conjuntoFalas[rand.Next(conjuntoFalas.Length)];
                chama = true;
                double perda = Math.Ceiling(Carga * 0.05);
                Carga -= perda;
                Console.WriteLine($"> {Name}: {falaSorteada}");
                Console.WriteLine($"> {Name} perdeu {perda} de Clímax Ígneo!");
                Console.WriteLine($"> (Clímax Ígneo restante = {Carga})");
                Console.ResetColor();
                var buff = new BonusDMG("Euforia Ígnia", 1, bonus / 2);
                if (aliado != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> [FERVOR CONTAGIANTE] {Name} é absoluta espalha uma aura calorosa que fortalece seus aliados! (+{bonus / 2} de dano de ataque por 1 turno)");
                    Console.ResetColor();
                    aliado.status.Add(buff);
                }
            }
        }

        private void AplicarChama()
        {
            int chance = rand.Next(1, 101);
            if (chama)
            {
                chance += 15;
            }
            modificador = Nodes >= 2 ? 11.52 : 10.48;
            porcentagem = (int)Math.Ceiling(ModTotal() * modificador);
            if (chance <= porcentagem && inimigoAlvo != null)
            {
                int duracao = rand.Next(1, 3);
                modificador = Nodes >= 4 ? 0.25 : 0.16;
                int porcentagem = (int)Math.Ceiling(ModTotal() * modificador);
                var poison = new PoisonMaxStack("Queimadura", duracao, porcentagem);
                inimigoAlvo.status.Add(poison);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> {inimigoAlvo.Name} está em chamas! ({porcentagem}% da vida máxima como dano por {duracao} turno(s))");
                Console.ResetColor();
            }
        }

        public override void Passiva()
        {
            bonus = (int)Math.Ceiling((AtkTotal() + ModTotal()) * Carga * 0.03);
            if (bonus > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [AQUECIMENTO] A energia de {Name} o fortalece. (+{bonus} de bônus de Ataque)");
                Console.ResetColor();
            }
            if (chama)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> A forma flamejante de {Name} cessou...");
                Console.ResetColor();
                chama = !chama;
            }
        }
        public override void Resetar()
        {
            Carga = 0;
            bonus = 0;
            chama = false;
            base.Resetar();
        }
    }
}