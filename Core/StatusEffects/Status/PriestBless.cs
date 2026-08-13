using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Task_U.Core.Entities;


namespace Task_U.Core.StatusEffects
{
    public class PriestBless : StatusEffect
    {
        private int oldHp;
        private double modificador;
        public override void Aplicar(PersonagemBase personagem)
        {
            PersonagemBase? priest = null;
            if (personagem.GetType() == typeof(Priest))
            {
                priest = personagem;
            }
            else if (personagem.aliado != null && personagem.aliado.GetType() == typeof(Priest))
            {
                priest = personagem.aliado;
            }

            if (priest != null && priest.HpAtual > 0)
            {
                if (oldHp > personagem.HpAtual)
                {
                    Random rand = new Random();
                    string[] falasCura =
{
                    "Beba da paz do oásis. Deixe que o cansaço flua para longe de você.",
                    "Apenas uma gota da nascente é o suficiente para renovar seu fôlego.",
                    "Limpando as impurezas... sinta o frescor em sua alma.",
                    "O rio nunca para de correr, e você também não deve parar.",
                    "Respire fundo. A água lava a dor e traz a vida de volta ao curso.",
                    "Permita-me te curar.",
                    "O ciclo se renova.",
                    "Flua e recupere-se."
                };

                    int dano = Math.Abs(personagem.HpAtual - oldHp);
                    int cura = (int)Math.Ceiling(dano * modificador);
                    personagem.curar(Name, cura);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{Name.ToUpper()}] A água lava a dor de {personagem.Name}");
                    Console.WriteLine($"> {priest.Name} {falasCura[rand.Next(falasCura.Count())]}");
                    Console.ResetColor();
                    Console.WriteLine($"{personagem.Name} recuperou {cura} de vida!");
                }

            }
            else
            {
                Duration = 0;
            }

            oldHp = personagem.HpAtual;
        }




        [SetsRequiredMembers]
        public PriestBless(string name, int duration, double cura) : base(name, duration, null)
        {
            modificador = cura;
            isBeneficial = true;
        }
    }
}