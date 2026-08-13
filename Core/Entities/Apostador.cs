using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Task_U.Services;
using Task_U.Models;
using Task_U.Data;
using Task_U.Core.Entities;
using System.ComponentModel.Design.Serialization;

namespace Task_U.Core
{
    public class Apostador : PersonagemBase
    {
        private static readonly Random random = new Random();
        public int BaseAtk { get; private set; }
        private int rolls;

        [NotMapped]
        public int BonusDMG { get; private set; }
        public bool Win { get; set; }

        public override int Damage()
        {
            int dano = AtkTotal() + BaseAtk + BonusDMG;
            return dano;
        }

        public override void Habilidade()
        {
            string NomeSkill = "LET IT RIDE!";
            int porcentagem = 40;
            if (aliado != null && typeof(SlimeA) == aliado.GetType())
            {
                porcentagem += 10;
                NomeSkill = "LET IT SPLASH!";
            }
            double modificador = Nodes >= 5 ? 1.47 : 1.15;
            porcentagem += (int)Math.Ceiling(ModTotal() * modificador);
            modificador = Nodes >= 3 ? 3.47 : 2.15;
            int cura = random.Next(ModTotal(), (int)Math.Ceiling(ModTotal() * modificador));
            int chance = random.Next(0, 100);
            if (chance > porcentagem && HpAtual > ModTotal())
            {
                this.HpAtual = Math.Max(1, HpAtual - cura);
                Win = false;
                rolls = 0;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [{NomeSkill}] {Name} Teve azar!");
                Console.WriteLine($"> {Name} perdeu {cura} pontos de vida!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                this.HpAtual += cura;
                Win = true;
                rolls++;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [{NomeSkill}] {Name} Teve sorte!");
                Console.WriteLine($"> {Name} se curou em {cura} pontos de vida!");
                Console.WriteLine($"> {Name} teve {rolls} vitórias seguidas!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (Win == true)
            {
                modificador = Nodes >= 2 ? 0.69 : 0.55;
                BonusDMG += (int)Math.Ceiling((AtkTotal() + ModTotal()) * modificador);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [{NomeSkill}] {Name} Tem um bônus de {BonusDMG} em cada ataque!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                int BeforeAtk;
                BeforeAtk = BonusDMG;
                modificador = Nodes >= 4 ? 0.32 : 0.20;
                BonusDMG = (int)Math.Ceiling(BonusDMG * modificador);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [{NomeSkill}] {Name} perdeu {BeforeAtk - BonusDMG} de bônus de seus ataques...");
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (chance <= 10 && Nodes >= 6)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [JACKPOT!] {Name} ATINGIU O PICO DA SORTE!");
                this.HpAtual += (int)Math.Ceiling(cura * 1.24);
                Console.WriteLine($"> {Name} se curou em {cura} pontos de vida!");
                BonusDMG += (int)Math.Ceiling((AtkTotal() + ModTotal()) * 1.32);
                Console.WriteLine($"> {Name} Recebeu um bônus de {BonusDMG} em cada ataque!");
                Console.ResetColor();
            }
        }

        public override void Passiva()
        {
            if (rolls >= 3)
            {
                double modificador = Nodes >= 1 ? 0.88 : 0.64;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"> [SORTE DE PRINCIPIANTE] {Name} Está em uma sequência de vitórias explosiva!");
                Console.ResetColor();
                BaseAtk = (int)Math.Ceiling((AtkTotal() + BonusDMG) * modificador);
                Console.WriteLine($"> {Name} recebeu um bônus de {BaseAtk} de dano!");
                if (user.PityEpic > 5)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> [ADRENALINA] O azar de hoje é o dano de amanhã! {Name} sente a sorte mudando.");
                    Console.WriteLine($"> [DOPAMINA] O ataque de {Name} foi dobrado!");
                    Console.ResetColor();
                    BaseAtk += (int)Math.Ceiling((AtkTotal() + BonusDMG) * 0.35);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Lurios precisa de mais vitórias consecutivas para ativar a Sorte de Principiante.");
                Console.ResetColor();
                BaseAtk = 0;
            }

        }

        public override void Resetar()
        {
            rolls = 0;
            BonusDMG = 0;
            Win = false;
            base.Resetar();
        }
    }
}