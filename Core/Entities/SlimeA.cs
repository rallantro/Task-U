using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Task_U.Services;
using Task_U.Models;
using Task_U.Data;

namespace Task_U.Core.Entities
{
    public class SlimeA : PersonagemBase
    {
        [NotMapped]
        public override int HpAtual { 
            get 
            {
                return base.HpAtual;
            } 
            set 
            {
                base.HpAtual = value; 
                if(this.HpAtual >= HpMax && !escudoBolha)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> [PASSIVA] {Name} criou um escudo de bolha em volta de si.");
                        Console.WriteLine($"> {Name}: Estou transbordando agora!");
                        Console.ResetColor();
                        Shield += HpMax/10;
                        escudoBolha = true;
                        Console.ReadKey();
                    }

                
                
            }
        }
        private int Fluxo;
        private bool escudoBolha;

        public override void curar(string aliado, int cura)
        {
            HpAtual += cura;
            Fluxo += 5;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"> [PASSIVA] {Name} recebeu 5% de fluxo através da cura!");
            Console.WriteLine($"> {Name}: Ahhh... que refrescante!");
            Console.ResetColor();
        }

        public override void tomarDano(string inimigo, int dano)
        {
            int danoTotal = Math.Max(0, dano - Shield);
            int danoShield = Math.Min(Shield, dano);
            int newFluxo = danoTotal * 3;
            Fluxo += Math.Min(100, newFluxo);
            Shield -= danoShield;
            if (danoShield > 0 && danoTotal == 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> {Name} bloqueou completamente o ataque de {inimigo} com seu escudo!");
                Console.WriteLine($"> {Name}: Isso nem fez cócegas!");
                Console.ResetColor();
            }
            else if(Fluxo < 60)
            {
                HpAtual -= danoTotal;
                Console.WriteLine($"{inimigo} atacou {Name} e causou {danoTotal} de dano!");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> [PASSIVA] {Name} recebeu {newFluxo}% de fluxo pelo dano recebido!");
                Console.WriteLine($"> {Name}: Ei isso doeu!");
                Console.ResetColor();
                Console.WriteLine($"{inimigo} atacou {Name} e causou {danoTotal} de dano!");
            }
            else
            {
                if (aliado != null && aliado.Name == "Jax")
                {
                    HpAtual -= danoTotal/3;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> [PASSIVA] O fluxo de {Name} está cheio! Ele vai estourar!");
                    Console.WriteLine($"> {Name}: Explosão de bolhas! Ploc, ploc, POW!!");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> {aliado.Name}: E EXPLOSÃO DE TINTA TAMBÉM!");
                    Console.ResetColor();
                    if(inimigoAlvo != null)
                    inimigoAlvo.tomarDano(this, (danoTotal * 2/3) + aliado.Mod);
                    Console.WriteLine($"{inimigo} atacou {Name} e causou {danoTotal/3} de dano!");
                    Fluxo = Math.Max(0, Fluxo - danoTotal); 
                }
                else
                {
                    HpAtual -= danoTotal/3;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> [PASSIVA] O fluxo de {Name} está cheio! Ele vai estourar!");
                    Console.WriteLine($"> {Name}: Explosão de bolhas! Ploc, ploc, POW!!");
                    Console.ResetColor();
                    Console.WriteLine($"{inimigo} atacou {Name} e causou {danoTotal/3} de dano!");
                    if(inimigoAlvo != null)
                    inimigoAlvo.tomarDano(this, danoTotal * 2/3);
                    Fluxo = Math.Max(0, Fluxo - danoTotal);   
                }
            }
        }

        public override void Habilidade()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"> [HABILIDADE] {Name} atira bolhas com suas mãos!");
            Console.WriteLine($"> {Name}: Vai um banho gelado aí? Hahaha!");
            Console.ResetColor();
            int dano = Fluxo/10 * Mod;    
            if(inimigoAlvo != null)
            {
                inimigoAlvo.tomarDano(this,dano); 
                inimigoAlvo.BuffAtk -= Mod; 
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> [HABILIDADE] {Name} reduz o ataque de {inimigoAlvo.Name} em {Mod}!");
                Console.WriteLine($"> {Name}: Sinta a pressão da maré!");
                Console.ResetColor();
            }
            
        }

        public override void Passiva()
        {
            if (Shield == 0 && escudoBolha == true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"> [PASSIVA] O escudo bolha vai estourar!");
                Console.WriteLine($"> {Name}: Cuidado com o respingo! Hahaha!");
                Console.ResetColor();
                if(inimigoAlvo != null)
                inimigoAlvo.tomarDano(this, HpMax/10);
                escudoBolha = false;
            }
        }
        
    }
}