using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Core.StatusEffects;

namespace Task_U.Core
{
    public class Police : PersonagemBase
    {
        private static readonly Random random = new Random();
        private int Analise;
        private int coolDown;
        private bool stunou;

        public override void tomarDano(string inimigo, int dano)
        {
            Analise++;
            if (Analise > 8)
            {

                base.tomarDano(inimigo, dano / 2);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"> [PASSIVA] {Name} aprendeu a se defender.");
                Console.WriteLine($"> {Name}: Protocolo de defesa!");
                Console.ResetColor();
            }
            else
            {
                base.tomarDano(inimigo, dano);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"> [PASSIVA] {Name} analisou o golpe que recebeu.");
                Console.WriteLine($"> {Name}: Examinando ataque...");
                Console.ResetColor();
            }
        }
        public override int Damage()
        {
            if (inimigoAlvo != null && inimigoAlvo.status.OfType<Stun>().FirstOrDefault() == null)
            {
                Analise = Math.Min(10, Analise + 1);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"> {Name}: Não consigo analisar um alvo incapacitado.");
                Console.ResetColor();
            }
            if (Analise == 10 && inimigoAlvo != null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"> [CAMPO DE SUPRESSÃO SENTINELA!] {Name} cria um campo eletromagnético capaz de suprimir os inimigos!");
                Console.WriteLine($"> {Name}: VOCÊ TEM O DIREITO DE PERMANECER CALADO!");
                if (aliado != null && aliado.Name == "Kael")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {aliado.Name}: DEU CERTO! AHHHH MINHA OBRA PRIMA!");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> {Name}: Não sabia que gostava tanto desse campo.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {aliado.Name}: Eu... Não tava falando dele...");
                }
                Console.ResetColor();
                var stunExistente = inimigoAlvo.status.OfType<Stun>().FirstOrDefault();
                if (stunExistente == null && stunou == false)
                {
                    inimigoAlvo.status.Add(new Stun("Stun", 1));
                    stunou = true;
                }
                else if(stunExistente != null)
                {
                    stunExistente.Duration = 1;
                    stunou = true;
                }
                var silenceEsistente = inimigoAlvo.status.OfType<Silence>().FirstOrDefault();
                if (silenceEsistente == null)
                {
                    inimigoAlvo.status.Add(new Silence("Silence", 2));
                }
                else
                {
                    silenceEsistente.Duration = 2;
                }
                Console.WriteLine($"> {inimigoAlvo.Name} está atordoado por 1 turnos.");
                Console.WriteLine($"> {inimigoAlvo.Name} está silenciado por 2 turnos.");
                int danoFinal = AtkTotal() + Analise * 2 + inimigoAlvo.HpMax / 10;
                Analise = 0;
                Console.WriteLine($"> {Name} precisará recomçar sua análise.");
                return danoFinal;
            }
            return AtkTotal() + Analise * 2;
        }

        public override void Habilidade()
        {
            int chance = random.Next(0, 100);
            if (chance < Analise * 10)
            {
                if (Analise <= 5 && inimigoAlvo != null)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> [PULSO DE INIBIÇÃO] {Name} atira um laser de energia neurotóxico!");
                    Console.WriteLine($"> {Name}: Um resultado satisfatório.");
                    Console.ResetColor();
                    Console.WriteLine($"> {inimigoAlvo.Name} está silênciado por 2 turnos.");
                    var silenceEsistente = inimigoAlvo.status.OfType<Silence>().FirstOrDefault();
                    if (silenceEsistente == null)
                    {
                        inimigoAlvo.status.Add(new Silence("Silence", 2));
                    }
                    else
                    {
                        silenceEsistente.Duration = 2;
                    }
                }
                else if (Analise > 5 && inimigoAlvo != null && aliado != null)
                {
                    var stunExistente = inimigoAlvo.status.OfType<Stun>().FirstOrDefault();
                    if (stunExistente == null && stunou == false)
                    {
                        Console.WriteLine($"> [ALGEMAS DE SUPRESSÃO] {Name} atira algemas feitas de uma energia eletromagnética que imobiliza o alvo!");
                        Console.WriteLine($"> {Name}: Considere-se preso!");
                        Console.ResetColor();
                        inimigoAlvo.status.Add(new Stun("Stun", 2));
                        Console.WriteLine($"> {inimigoAlvo.Name} está atordoado por 2 turnos.");
                        stunou = true;
                    }
                    else if (stunExistente != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"O alvo já está imóvel!");
                        Console.WriteLine($"> {Name}: Não posso prender o que já está preso...");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"As algemas ainda estão recarregando");
                        Console.WriteLine($"> {Name}: Preciso de energia para prender novamente...");
                        Console.ResetColor();
                    }

                    aliado.BuffAtk += 2 * Mod;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> [ENCORAJAMENTO] {Name} explica os pontos fracos do inimigo.");
                    Console.WriteLine($"> {Name}: Ataque bem ali.");
                    if (aliado.Name == "Kael")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"> {aliado.Name}: A-ali? Como eu vou atacar ali?????");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"> {Name}: Você vai conseguir Kael.");
                    }
                    Console.ResetColor();
                    Console.WriteLine($"> {aliado.Name} recebeu um bônus de {2 * Mod} de ataque.");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"> [HABILIDADE] {Name} falhou em impedir o inimigo.");
                if (aliado != null && aliado.Name == "Kael")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {aliado.Name}: Não se preocupa... Na próxima você consegue, amor...");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> {Name}: Eu- Você é realmente relaxante sabia?");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {aliado.Name}: Eu... Eh... A gente tenta né?");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"> {Name}: Preciso de mais dados!");
                    Console.ResetColor();
                }
            }
        }

        public override void Passiva()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"> [PASSIVA] Análise do Inimigo = {Analise * 10}%");
            Console.ResetColor();
            if (Analise > 5 && inimigoAlvo != null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"> [PASSIVA] {Name} entendeu o estilo de combate do inimigo.");
                Console.WriteLine($"> {Name}: Me aproximando do alvo... Não preciso de reforços.");
                if (aliado != null && aliado.Name == "Kael")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {aliado.Name}: Que? E eu?!");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> {Name}: Você não conta como reforços.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {aliado.Name}: Poxa...");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> {Name}: Você é minha dupla fixa Kael, não um reforço.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {aliado.Name}: AH TÁ!");
                }
                Console.ResetColor();
                Console.WriteLine($"> {inimigoAlvo.Name} agora dá -{inimigoAlvo.Atk *  Analise / 10} de dano em ataques.");
                inimigoAlvo.BuffAtk -= inimigoAlvo.Atk/2 *  Analise / 10;
            }
            else if (inimigoAlvo != null && Analise < 5)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"> [PASSIVA] {Name} está estudando o inimigo.");
                Console.WriteLine($"> {Name}: Esses óculos holográficos não deixam escapar nada. Nem eu.");
                Console.ResetColor();
                Analise++;
            }
            if (inimigoAlvo != null)
            {
                var stunExistente = inimigoAlvo.status.OfType<Stun>().FirstOrDefault();
                var silenceExistente = inimigoAlvo.status.OfType<Silence>().FirstOrDefault();
                if (silenceExistente != null && silenceExistente.Duration > 0 || stunExistente != null && stunExistente.Duration > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> [PASSIVA] {Name} sabe onde atacar com o inimigo incapacitado.");
                    Console.WriteLine($"> {Name}: Uso de força autorizado.");
                    Console.ResetColor();
                    BuffAtk = inimigoAlvo.HpMax / 20;
                }
            }

            if (stunou == true && coolDown < 1)
            {
                coolDown += 1;
            } else if(stunou == true && coolDown >= 2)
            {
                stunou = false;
            }
        }

        public override void Resetar()
        {
            Analise = 0;
            coolDown = 0;
            stunou = false;
            base.Resetar();
        }
    }
}