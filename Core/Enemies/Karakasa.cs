using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Models;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Enemies
{
    public class Karakasa : InimigoBase
    {
        private int lambidas;
        public override int Damage()
        {
            lambidas += 1;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"> [PERO PERO] {Name} absorveu energia através de sua lambida!");
            Console.ResetColor(); 
            return base.Damage();
            
        }
        public override void Habilidade()
        {
            int useSkill = rand.Next(1, 101);
            if (useSkill <= HabilidadeChance && alvos != null && lambidas > 0)
            {
                PersonagemBase alvo = EscolherAlvo();
                alvo.tomarDano(Name, Mod * lambidas);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"> [REFLETIR] {Name} se abre criando um feixe de luz que bloqueia o campo de visão de {alvo.Name}, por {lambidas} turnos.");
                Console.ResetColor(); 
                var blindExistente = alvo.status.OfType<Blind>().FirstOrDefault();
                if (blindExistente != null)
                {
                    blindExistente.Duration += lambidas;
                }
                else
                {
                    var blind = new Blind("Blind", lambidas);
                    alvo.status.Add(blind);
                }
                lambidas = 0;
            }
        }

        public override void Resetar()
        {
            lambidas = 0;
            base.Resetar();
        }
    }
}