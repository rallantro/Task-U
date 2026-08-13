using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_U.Core.Entities
{
    public class Star : PersonagemBase
    {
        private bool podeOrar = false;
        private int orarCD;
        private int curaBonus;
        public override int Damage()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow; 
            Console.WriteLine($"> {Name}: Que sua dor me ilumine!"); 
            Console.ResetColor();
            HpAtual += (AtkTotal() + Mod +  (HpMax/8))/2;
            Console.WriteLine($"> {Name} se curou em {(AtkTotal() + Mod +  (HpMax/8))/2}."); 
            return AtkTotal();
        }
        public override void Habilidade()
        {
            if (aliado != null && aliado.HpAtual < aliado.HpMax && HpAtual > HpMax/5 && aliado.HpAtual > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [HABILIDADE] {Name} roga as estrelas, curando {aliado.HpMax / 15 * Mod + curaBonus} pontos de vida de {aliado.Name}!"); 
                Console.WriteLine($"> {Name}: Rogo por {aliado.Name}, estrelas, que sua luz brilhe por mim!"); 
                Console.ResetColor();
                HpAtual -= HpAtual/9;
                if (curaBonus > 0)
                {
                    orarCD = 0;
                }
                aliado.curar(Name, (aliado.HpMax / 15 * Mod ) + curaBonus);   
                orarCD += Math.Min(4, orarCD +1);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [HABILIDADE] {Name} roga as estrelas, curando a si mesma..."); 
                Console.WriteLine($"> {Name}: Que o brilho de cada estrela brilhe através de meu corpo!"); 
                Console.ResetColor();
                HpAtual += HpMax / 15 * Mod + curaBonus;
            }
            
        }
        public override void Passiva()
        {
            if (orarCD >= 4 && podeOrar == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [PASSIVA] O brilho das estrelas retornou para {Name}!"); 
                Console.WriteLine($"> {Name} agora pode usar [Cintilação de Desespero]"); 
                Console.ResetColor();
                podeOrar = true;
            }
            if (aliado != null && aliado.HpAtual <= aliado.HpMax/5 && aliado.HpAtual > 0 && podeOrar)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [PASSIVA] Cintilação do Desespero! {Name} intensificou suas orações!"); 
                Console.WriteLine($"> {Name}: ESTRELAS OUÇAM A MIM!"); 
                Console.ResetColor();
                curaBonus = aliado.HpMax * 2/3 + (Mod * 2/3); 
                podeOrar = false;
            }
            else if (HpAtual <= HpMax/6 && podeOrar)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [PASSIVA] Cintilação do Desespero! {Name} intensificou suas orações!"); 
                Console.WriteLine($"> {Name}: ESTRELAS OUÇAM A MIM!"); 
                Console.ResetColor();
                curaBonus = HpMax / 5 + (Mod * 2/3); 
                podeOrar = false;
            }
            else
            {
                curaBonus = 0;
            }
        }

        public override void Resetar()
        {
            podeOrar = false;
            orarCD = 0;
            curaBonus = 0;
            base.Resetar();
        }
    }
}