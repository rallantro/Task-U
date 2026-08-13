using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Task_U.Models;
using Task_U.Data;
using Task_U.Core.StatusEffects;

namespace Task_U.Core
{
    public class Soul : PersonagemBase
    {

        private static readonly Random random = new Random();
        private int acumulo;


        public override void Habilidade()
        {

            if (inimigoAlvo != null && inimigoAlvo.status.Any(x => x.Name == "Azar do Rude") == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[CASTIGO DA DESCORTESIA]: {Name} pune aqueles que são mal educados com o azar.");
                Console.ResetColor();
                var debuff = new DebuffDMG("Azar do Rude", 2, inimigoAlvo.Atk * acumulo / 10);
                Console.WriteLine($"{inimigoAlvo.Name} foi amaldiçoado com {debuff.Name}");
                inimigoAlvo.status.Add(debuff);
            }
            else if (inimigoAlvo != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[CASTIGO DA DESCORTESIA]: {Name} pune aqueles que são mal educados com a RUÍNA.");
                Console.ResetColor();
                var debuff = new PoisonMaxStack("Ruína ao Malcriado", 2, 5);
                Console.WriteLine($"{inimigoAlvo.Name} foi amaldiçoado com {debuff.Name}");
                inimigoAlvo.status.Add(debuff);
            }
        }
        public override void Passiva()
        {
            acumulo = Math.Min(5, acumulo + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            if (acumulo < 10)
            {
                Console.WriteLine($"[PASSIVA: PRESENÇA TRANQUILA]: {Name} acalma o ambiente ao redor em {acumulo*10}%");    
            }
            else
            {
                Console.WriteLine($"[PASSIVA: PRESENÇA TRANQUILA]: {Name} está no ápice da hospitalidade.");
            }
            Console.ResetColor();
            if (aliado != null)
            {
                int bonus = (int)Math.Ceiling(aliado.AtkTotal() * acumulo / 10.0);
                var buff = new BonusDMG("Sorte do Educado", 1, bonus);
                if (aliado.status.Any(x => x.Name == "Sorte do Educado"))
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[PASSIVA: AURA DA HOSPIDALIDADE ETERNA]: {Name} continua fortalecendo os educados ao seu redor.");
                    Console.ResetColor();
                    Console.WriteLine($"{aliado.Name} teve a duração de {buff.Name} estendida em + 1 turno. (+ {bonus} de dano bônus)");

                    int index = aliado.status.IndexOf(buff);
                    aliado.status[index].Duration = 1;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[PASSIVA: AURA DA HOSPIDALIDADE ETERNA]: {Name} criou uma aura de fortalecimento aos seus aliados.");
                    Console.ResetColor();
                    Console.WriteLine($"{aliado.Name} recebeu {buff.Name}. (+ {bonus} de dano bônus)");
                    aliado.status.Add(buff);
                }

            }
            else
            {
                int bonus = (int)Math.Ceiling(AtkTotal() * acumulo / 10.0);
                var buff = new BonusDMG("Sorte do Educado", 1, (int)Math.Ceiling(AtkTotal() * acumulo / 10.0));
                if (status.Any(x => x.Name == "Sorte do Educado"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[PASSIVA: AURA DA HOSPIDALIDADE ETERNA]: {Name} continua fortalecendo ao único educado... Ele mesmo.");
                    Console.ResetColor();
                    Console.WriteLine($"{Name} teve a duração de {buff.Name} estendida em + 1 turno. (+ {bonus} de dano bônus)");

                    int index = status.IndexOf(buff);
                    status[index].Duration = 1;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[PASSIVA: AURA DA HOSPIDALIDADE ETERNA]: {Name} cria uma aura de fortalecimento aos educados... Ele.");
                    Console.ResetColor();
                    Console.WriteLine($"{Name} teve a duração de {buff.Name} estendida em + 1 turno. (+ {bonus} de dano bônus)");
                    status.Add(buff);
                }

            }
        }

        public override void Resetar()
        {
            acumulo = 0;
            base.Resetar();
        }


    }
}