using System;
using System.Collections.Generic;
using System.Linq;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Entities
{
    public class Cleaner : PersonagemBase
    {
        private int pureza = 0;
        private double modificador;
        private Dictionary<PersonagemBase, int> shieldCount = new Dictionary<PersonagemBase, int>();

        public override void Habilidade()
        {
            PersonagemBase alvo;

            if (aliado != null && (aliado.Shield <= 0 || aliado.Shield < Shield))
            {
                alvo = aliado;
            }
            else
            {
                alvo = this;
            }

            if (alvo.Shield <= 0)
            {
                shieldCount.Remove(alvo);
            }
            double vezes = shieldCount.ContainsKey(alvo) ? shieldCount[alvo] : 0;
            modificador = Nodes >= 1 ? 7.64 : 6.24;
            int valorBase = (int)Math.Ceiling(Mod * modificador);
            modificador = Nodes >= 3 ? 1.43 : 1.58;
            int valorEscudo = Math.Max(1, (int)Math.Ceiling(valorBase / ((vezes + 1) * modificador)));



            var oldShield = alvo.status.OfType<escudoPurificador>().FirstOrDefault();
            if (oldShield != null)
                alvo.status.Remove(oldShield);

            alvo.Shield += valorEscudo;

            if (shieldCount.ContainsKey(alvo))
            {
                shieldCount[alvo]++;
            }
            else
            {
                shieldCount[alvo] = 1;
            }


            var purif = new escudoPurificador("Pele de Ânfora", 1);
            alvo.status.Add(purif);


            Console.ForegroundColor = ConsoleColor.DarkGreen;
            if (alvo == this)
                Console.WriteLine($"> [AUTOEMBALSAMAMENTO] {Name} aplica {valorEscudo} de escudo em si mesma e purifica suas próprias impurezas!");
            else
                Console.WriteLine($"> [PROTOCOLO DE EMBALSAMAMENTO] {Name} aplica {valorEscudo} de escudo e protege {alvo.Name} de más influências!");
            Console.ResetColor();
        }

        public override void Passiva()
        {
            PersonagemBase alvo = aliado ?? this;

            if (alvo != null && alvo.status.Any(s => s is escudoPurificador))
            {
                pureza++;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"> [HIGIENE ESPIRITUAL] Pureza do ambiente: {pureza}/3");
                Console.ResetColor();

                if (pureza >= 4 || pureza >= 3 && Nodes >= 4)
                {
                    modificador = Nodes >= 2 ? 7.76 : 5.45;
                    int cura = (int)Math.Ceiling(Mod * modificador);
                    if (alvo != this)
                    {
                        curar(Name, cura);
                    }
                    alvo.curar(Name, cura);
                    Console.WriteLine($"{Name} e {alvo.Name} recuperaram {cura} HP!");

                    var alvosParaCura = aliado != null ? new[] { this, aliado } : [this];
                    var alvoCura = alvosParaCura.OrderBy(p => p.HpAtual).First();

                    var debuff = alvoCura.status.FirstOrDefault(s => !s.isBeneficial);
                    if (debuff != null)
                    {
                        alvoCura.status.Remove(debuff);
                        Console.WriteLine($"{Name} removeu {debuff.Name} de {alvoCura.Name}!");
                    }
                    else
                    {
                        modificador = Nodes >= 5 ? 0.42 : 0.31;
                        var buff = new BonusDMG("Limpeza Revigorante", 1, (int)Math.Ceiling(alvoCura.AtkTotal() * modificador));
                        alvoCura.status.Add(buff);
                    }

                    if (Nodes >= 6)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine($"> [ASSEIO IMACULADO] {Name} impedirá que o impuro toque {alvoCura}!");
                        Console.ResetColor();
                        alvoCura.Shield += (int)Math.Ceiling(alvoCura.HpMax * 0.13);
                    }
                    pureza = 0;
                }
            }
            else
            {
                pureza = 0;
            }
        }

        public override void Resetar()
        {
            pureza = 0;
            shieldCount.Clear();
            base.Resetar();
        }
    }

}
