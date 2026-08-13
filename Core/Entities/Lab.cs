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
        private Random rand = new Random();

        [NotMapped]
        public override int HpAtual
        {
            get
            {
                return base.HpAtual;
            }
            set
            {
                if (value < base.HpAtual)
                {
                    if (HpAtual >= HpMax * 2 / 3)
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
                base.HpAtual = value;
            }
        }

        public override void Habilidade()
        {
            double modificador;
            if (aliado != null && aliado.HpAtual >= aliado.HpMax / 2)
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
                Console.WriteLine($" {aliado.Name} deu {ModTotal()} de bônus de modificador para {aliado.Name}");
                modificador = Nodes >= 4 ? 1.758 : 1.258;
                aliado.BuffMod = (int)Math.Ceiling(ModTotal() * modificador);
                Console.ReadKey();
            }
            else if (aliado != null && aliado.HpAtual <= aliado.HpMax / 2 && HpAtual > HpMax / 3)
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
                modificador = Nodes >= 3 ? 5.584 : 4.542;
                aliado.curar(Name, (int)Math.Ceiling(ModTotal() * modificador));
                Console.ReadKey();
            }
            else if (aliado != null && aliado.HpAtual <= aliado.HpMax / 2 && HpAtual < HpMax / 3)
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
                modificador = Nodes >= 1 ? 0.143 : 0.072;
                this.curar(Name, (int)Math.Ceiling(HpMax * modificador));
                modificador = Nodes >= 3 ? 5.584 : 4.542;
                aliado.curar(Name, (int)Math.Ceiling(ModTotal() * modificador));
                Console.ReadKey();
            }
        }

        public override void Passiva()
        {
            if (aliado != null && (aliado.HpAtual <= aliado.HpMax / 5 || aliado.HpAtual <= aliado.HpMax * 0.32 && Nodes >= 2))
            {
                double modificador;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [PROTÓTIPO DE FORÇA DE ÚLTIMA HORA] {Name} criou uma barreira em volta de {aliado.Name}");
                if (aliado.Name == "Agente Clara")
                {
                    Console.WriteLine($"> {Name}: Eu vou te proteger sempre!");
                    Console.ResetColor();
                    modificador = Nodes >= 5 ? 3.325 : 2.145;
                    int escudo = (int)Math.Ceiling(ModTotal() * modificador);
                    Console.WriteLine($" {Name} deu {escudo} de escudo para {aliado.Name}");
                    aliado.Shield += escudo;
                }
                else
                {
                    Console.WriteLine($"> {Name}: Espero que isso ajude! E funcione também!");
                    Console.ResetColor();
                    modificador = Nodes >= 5 ? 2.325 : 1.145;
                    int escudo = (int)Math.Ceiling(ModTotal() * modificador);
                    Console.WriteLine($" {Name} deu {escudo} de escudo para {aliado.Name}");
                    aliado.Shield += escudo;
                }
                Console.ReadKey();
            }
            if (Nodes == 6 && HpAtual <= HpMax * 0.28)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [BRAINSTORM DESESPERADO] {Name} está desesperado!");
                int chance = rand.Next(3, 6);
                int feitos = 0;
                while (feitos < chance)
                {
                    int escolhido = rand.Next(1, 3);
                    if (escolhido == 1 && inimigoAlvo != null)
                    {
                        Console.WriteLine($"> [MINI EXPLOSOFUCHO] {Name} atira um bichinho de pelúcia que explode em {inimigoAlvo.Name}!");
                        Console.ResetColor();
                        inimigoAlvo.tomarDano(this, (int)Math.Ceiling(AtkTotal() * 2.34));
                    }
                    else if (escolhido == 2)
                    {
                        Console.WriteLine($"> [RECUPERADOR SINTÉTICO DE CURATIVOS] {Name} cola um curativo diferente em toda a equipe!");
                        this.curar(Name, (int)Math.Ceiling(ModTotal() * 6.14));
                        if (aliado != null)
                        {
                            aliado.curar(Name, (int)Math.Ceiling(ModTotal() * 6.14));
                        }
                    }
                    else
                    {
                        if (aliado != null)
                        {
                            Console.WriteLine($"> [ANTENA BIÔNICA MEGA TECH] {Name} coloca uma antena na cabeça de {aliado.Name} para defendê-lo!");
                            Console.ResetColor();
                            int escudo = (int)Math.Ceiling(ModTotal() * 6.52);
                            Console.WriteLine($" {Name} deu {escudo} de escudo para {aliado.Name}");
                            aliado.Shield += escudo;
                        }
                        else
                        {
                            Console.WriteLine($"> [ANTENA BIÔNICA MEGA TECH] {Name} coloca uma antena na sua cabeça para defender-se!");
                            Console.ResetColor();
                            int escudo = (int)Math.Ceiling(ModTotal() * 6.52);
                            Console.WriteLine($" {Name} deu {escudo} de escudo para si mesmo!");
                            Shield += escudo;
                        }
                    }
                    feitos++;
                }
            }
        }
    }
}