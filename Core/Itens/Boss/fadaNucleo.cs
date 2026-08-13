using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Itens
{
    public class fadaNucleo : Item
    {
        private bool usou;
        public override void Effect(PersonagemBase personagem)
        {
            if (personagem.HpAtual < personagem.HpMax / 5 && !usou)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [APRIMORAMENTO DE EMERGÊNCIA] As nano-fadas do núcleo se desesperam para socorrer {personagem.Name}!");
                Console.WriteLine($"> {personagem.Name} foi curado em {personagem.HpMax / 4} e recebeu regeneração por 10 turnos!");
                Console.ResetColor();
                personagem.curar("Núcleo de Aprimoramentos", personagem.HpMax / 4);
                var heal = new HealFixedStack("Aprimoramentos de Fada", 4, personagem.HpMax / 10);
                personagem.status.Add(heal);
                usou = true;
            }
        }

        public override void Resetar()
        {
            usou = false;
            base.Resetar();
        }
    }
}