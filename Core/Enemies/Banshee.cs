using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Task_U.Models;
using Task_U.Core.StatusEffects;

namespace Task_U.Core
{
    public class Banshee : InimigoBase
    {
        private bool gritou = false;

        public override void aplicarEfeitos()
        {
            var stun = status.FirstOrDefault(x => x.Name =="Stun");
            if (stun != null && stun.Duration < 3 && stun.Duration > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"> [INTANGIBILIDADE CIBERNÉTICA] {Name} se solta de suas amarras...");
                Console.ResetColor();
                status.Remove(stun);
            }
            base.aplicarEfeitos();
        }
        public override int Damage()
        {
            foreach (var alvo in alvos)
            {
                if (alvo.status.Count(x => x.Name == "Silence") > 0)
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
            if (inimigo.status.Count(x => x.Name == "Silence") > 0)
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
                PersonagemBase alvo = EscolherAlvo();
                if (alvo.status.Count(x => x.Name == "Silence") > 0)
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
                var silence = new Silence("Silence", 1);
                alvo.status.Add(silence);
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
                        var silence = new Silence("Silence", chance);
                        alvo.status.Add(silence);
                        Console.WriteLine($"> {alvo.Name} recebeu {chance} turnos de silence!");
                    }
                    else
                    {
                        var stun = new Stun("Stun", chance);
                        alvo.status.Add(stun);
                        Console.WriteLine($"> {alvo.Name} recebeu {chance} turnos de stun!");
                    }
                }
                gritou = true;
            }
        }

        public override void Resetar()
        {
            gritou = false;
            base.Resetar();
        }
    }
}