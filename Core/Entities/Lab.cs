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
    public class Lab : PersonagemBase
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
                if(HpAtual >= HpMax*2/3)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"> [PASSIVA] {Name} criou uma distração utilizando uma bugiganga.");
                        Console.WriteLine($"> {Name}: AHHH! Eu espero que o transmutador de fluxo não esto- Droga! Eiiii Por aqui!");
                        Console.ResetColor();
                        this.chanceAlvo = 90;
                        Console.ReadKey();
                        Console.WriteLine($" {Name} agora tem mais chance de receber ataques.");
                    }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> [PASSIVA] {Name} ativou um dispositivo de camuflagem.");
                    Console.WriteLine($"> {Name}: Uma licensinha!! Eu acho que- POR QUE ESSE DISPOSITIVO NÃO- ah funcionou agora! Fui!");
                    Console.ResetColor();
                    Console.WriteLine($" {Name} agora tem menos chance de receber ataques.");
                    this.chanceAlvo = 20;
                    Console.ReadKey();
                }
            }
        }

        public override void Habilidade()
        {
            if (aliado != null && aliado.HpAtual >= aliado.HpMax/2)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [UPGRADE REMOTO] {Name} jogou um dispositivo estranho que aumenta a energia dos aliados.");
                if (aliado.Name == "Agente Clara")
                {
                    Console.WriteLine($"> {Name}: E-eu não sei se isso é muito seguro e-");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> {aliado.Name}: Está funcionando, estou melhorando. Então está perfeito.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {Name}: Se você diz, amor...");
                }
                Console.ResetColor();
                Console.WriteLine($" {aliado.Name} deu {Mod} de bônus de modificador para {aliado.Name}");
                aliado.BuffMod = Mod;
                Console.ReadKey();
            } else if (aliado != null && aliado.HpAtual <= aliado.HpMax/2 && HpAtual > HpMax/3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [MANUTENÇÃO PREVENTIVA] {Name} faz um reparo de primeiros socorros em {aliado.Name}");
                if (aliado.Name == "Agente Clara")
                {
                    Console.WriteLine($"> {Name}: Querida, tenha mais cuidado...");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> {aliado.Name}: Eu sei que você consegue consertar de tudo.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {Name}: Ah- Eu- Eh... Claro, amor!");
                }
                Console.ResetColor();
                aliado.curar(Name, Mod * 3);
                Console.ReadKey();
            } else if (aliado != null && aliado.HpAtual <= aliado.HpMax/2 && HpAtual < HpMax/3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [PROTOCOLO DE SOCORRO] {Name} faz um reparo de primeiros socorros em todos");
                if (aliado.Name == "Agente Clara")
                {
                    Console.WriteLine($"> {Name}: Ahhh! Eu vou morrer! Amor, por favor fica viva eu-");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"> {aliado.Name}: Eu não vou deixar você morrer.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {Name}: Ah- Eu- Eh... Eu também não vou deixar te machucarem!");
                }
                Console.ResetColor();
                this.curar(Name, HpMax/20 * 5);
                aliado.curar(Name, Mod * 3);
                Console.ReadKey();
            }
        }

        public override void Passiva()
        {
            if (aliado != null && aliado.HpAtual <= aliado.HpMax/5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [PROTÓTIPO DE FORÇA DE ÚLTIMA HORA] {Name} criou uma barreira em volta de {aliado.Name}");
                if (aliado.Name == "Agente Clara")
                {
                    Console.WriteLine($"> {Name}: Eu vou te proteger sempre!");
                    Console.ResetColor();
                    Console.WriteLine($" {Name} deu {Mod * 3} de escudo para {aliado.Name}");
                    aliado.Shield += Mod * 3;
                }
                else
                {
                    Console.WriteLine($"> {Name}: Espero que isso ajude! E funcione também!");    
                    Console.ResetColor();
                    Console.WriteLine($" {Name} deu {Mod * 2} de escudo para {aliado.Name}");
                    aliado.Shield += Mod * 2;
                }
                Console.ReadKey();
            }
 
        }
        
    }
}