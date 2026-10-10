using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Itens
{
    public class presaFernir : Item
    {
        private int speed;
        public override void Effect(PersonagemBase personagem)
        {
            if (personagem.inimigoAlvo != null)
            {
                speed = Math.Max(5, (int)Math.Ceiling((1 - (double)(personagem.inimigoAlvo.HpAtual / personagem.inimigoAlvo.HpMax)) * 25.0));
            }
            else
            {
                speed = 5;
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"> [ESPÍRITO DO LOBO DIVINO] As veias de {personagem.Name} emitem um leve brilho dourado. A velocidade... A emoção da caçada!");
            Console.WriteLine($"> {personagem.Name} recebe +{speed} de velocidade.");
            Console.ResetColor();
            var existente = personagem.status.OfType<BonusSPD>()
        .FirstOrDefault(s => s.Name == "Espírito do Lobo Divino");

            if (existente != null)
            {
                existente.Mod = speed;
                existente.Duration = 1;
            }
            else
            {
                var buff = new BonusSPD("Espírito do Lobo Divino", 1, speed);
                personagem.status.Add(buff);
            }
        }
    }
}