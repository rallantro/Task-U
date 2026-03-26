using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Todo_Gacha.Models;

namespace Todo_Gacha.Core
{
    public class Banshee : InimigoBase
    {
        [NotMapped]
        public override int TurnoStun {
            get
            {
                return base.TurnoStun;
            }
            set
            {
                if (base.TurnoStun > 0 && base.TurnoStun < 3)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"> [INTANGIBILIDADE CIBERNÉTICA] {Name} se solta de suas amarras...");
                    Console.ResetColor();
                    base.TurnoStun = 0;
                }
                else
                {
                    base.TurnoStun = value;
                }
            }
        }
        private bool gritou = false;
        public override int Damage()
        {
            foreach (var alvo in alvos)
            {
                if (alvo.TurnoSilence > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{Name} foi fortalecida pela confusão!");
                    Console.ResetColor();
                    BuffAtk += Mod * 2;
                }
            }
            return base.Damage();   
            
        }
        public override void tomarDano(PersonagemBase inimigo, int dano)
        {
            int reducao = 0;
            if (inimigo.TurnoSilence > 0)
            {
                Console.WriteLine($"O ataque de {inimigo.Name} atravessa parcialmente {Name}!");
                reducao = dano / 2;
            }
            int danoTotal = Math.Max(0, dano - Shield - reducao);
            int danoShield = Math.Min(Shield, dano - reducao);
            Shield -= danoShield;
            HpAtual -= danoTotal;

            Console.WriteLine($"{inimigo.Name} atacou {Name} e causou {danoTotal} de dano!");
        }

        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            if (useSkill <= HabilidadeChance && alvos != null)
            {
                PersonagemBase alvo;
                int chance = rand.Next(0, alvos.Count());
                alvo = alvos[chance];
                if (alvo.TurnoSilence > 0)
                {
                    var novoAlvo = alvos.FirstOrDefault(x => x != alvo && x != null);
                    if (novoAlvo != null)
                    {
                        alvo = novoAlvo;
                    }
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"> [GRITO DE BANSHEE] {Name} grita causando agonia a {alvo.Name}! (1 turno de silence)");
                Console.ResetColor();
                alvo.TurnoSilence = 1;
            }
        }

        public override void Passiva(User user)
        {
            if (HpAtual <= HpMax/3 && gritou == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"> [GRITO DE DESESPERO] {Name} solta um grito estridente!");
                Console.ResetColor();
                foreach (var alvo in alvos.Where(x => x.HpAtual > 0))
                {
                    int chance = rand.Next(1, 6);
                    if(chance > 3)
                    {
                        alvo.TurnoSilence += chance;
                        Console.WriteLine($"> {alvo.Name} recebeu {chance} turnos de silence!");
                    }
                    else
                    {
                        alvo.TurnoStun += chance;
                        Console.WriteLine($"> {alvo.Name} recebeu {chance} turnos de stun!");
                    }
                }
                gritou = true;
            }
        }
    }
}