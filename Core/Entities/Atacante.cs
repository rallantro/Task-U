using System;
using System.Collections.Generic;
using System.Linq;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Entities
{
    public class Atacante : PersonagemBase
    {
        private int contadorGolpes = 0;
        private Random random = new Random();
        private int cooldownHabilidade = 0;

        public override int Damage()
        {
            int danoBase = base.Damage();
            int danoFinal = danoBase;

            if (contadorGolpes == 1)
            {
                double porcentagem = Nodes >= 1 ? 1.60 : 1.35;
                danoFinal = (int)(danoBase * porcentagem);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [NÓ DE TENSÃO] {Name} executa o segundo golpe da sequência! Dano +{(porcentagem - 1) * 10}%!");
                Console.ResetColor();
            }
            else if (contadorGolpes == 2 && inimigoAlvo != null)
            {
                double porcentagem = Nodes >= 4 ? 0.15 : 0.10;
                int danoExplosao = (int)(inimigoAlvo.HpMax * porcentagem) + (int)(ModTotal() * 3 * (1 - (double)inimigoAlvo.HpAtual / inimigoAlvo.HpMax));
                danoFinal += danoExplosao;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [CORTE DO VEREDITO] {Name} finaliza a sequência com um golpe explosivo!");
                Console.ResetColor();
                if (Nodes >= 5)
                {
                    int cura = (int)Math.Ceiling(danoExplosao * 0.05);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"> [SIFÃO DA EXISTÊNCIA] {Name} rouba um pouco da vida perdida de seu inimigo!");
                    Console.ResetColor();
                    curar(Name, cura);
                    if (Nodes >= 6)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"> [SUPRESSÃO DO DESTINO] {Name} Suprime o inimigo com silêncio!");
                        Console.ResetColor();
                        inimigoAlvo.status.Add(new Silence("Supressão", 2));
                    }
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"> [FIO DO PRINCÍPIO] {Name} Começa sua sequência de ataques.");
                Console.ResetColor();
            }

            contadorGolpes = (contadorGolpes + 1) % 3;

            return danoFinal;
        }

        public override void Habilidade()
        {
            if (cooldownHabilidade <= 0 && inimigoAlvo != null)
            {
                int multiplicador = Nodes >= 2 ? 10 : 5;
                int dano = base.Damage() + ModTotal() * multiplicador;
                inimigoAlvo?.tomarDano(this, dano);

                Console.ForegroundColor = ConsoleColor.Magenta;

                contadorGolpes = (contadorGolpes + 2) % 3;

                if (contadorGolpes == 0 && inimigoAlvo != null)
                {
                    double porcentagem = Nodes >= 4 ? 0.15 : 0.10;
                    int danoExplosao = (int)(inimigoAlvo.HpMax * porcentagem) + (int)(ModTotal() * 5 * (1 - (double)inimigoAlvo.HpAtual / inimigoAlvo.HpMax));
                    inimigoAlvo?.tomarDano(this, danoExplosao);
                    Console.WriteLine($"> [ACELERAÇÃO DE CICLOS] {Name} completa a sequência com um golpe explosivo imediato!");
                }
                else
                {
                    
                    Console.WriteLine($"> [ACELERAÇÃO DE CICLOS] {Name} usa Habilidade Rápida, avançando dois golpes na sequência!");
                    
                }
                Console.ResetColor();
                cooldownHabilidade = Nodes >= 3 ? 2 : 3;

            }
            else
            {
                Console.WriteLine($"> Habilidade em recarga. Ainda faltam {cooldownHabilidade} turnos.");
            }
        }

        public override void Passiva()
        {
            if (cooldownHabilidade > 0) cooldownHabilidade--;
        }

        public override void Resetar()
        {
            contadorGolpes = 0;
            cooldownHabilidade = 0;
            base.Resetar();
        }
    }
}