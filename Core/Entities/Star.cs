using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo_Gacha.Core.Entities
{
    public class Star : PersonagemBase
    {
        private bool podeOrar = false;
        private bool curou;
        private int orarCD;
        private int curaBonus;
        public override int Damage()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow; 
            Console.WriteLine($"> {Name}: Que sua dor me ilumine!"); 
            Console.ResetColor();
            HpAtual += (AtkTotal() * 2/7) +  (HpMax/8);
            Console.WriteLine($"> {Name} se curou em {(AtkTotal() * 2/7) +  (HpMax/8)}."); 
            return AtkTotal();
        }
        public override void Habilidade()
        {
            if (aliado != null && aliado.HpAtual < aliado.HpMax)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [HABILIDADE] {Name} roga as estrelas, curando {aliado.HpMax * Mod / 9 + curaBonus} de {aliado.Name}!"); 
                Console.WriteLine($"> {Name}: Rogo por {aliado.Name}, estrelas, que sua luz brilhe por mim!"); 
                Console.ResetColor();
                HpAtual -= HpAtual/9;
                if (curaBonus > 0)
                {
                    orarCD = 0;
                }
                aliado.HpAtual += aliado.HpMax * Mod / 10 + curaBonus;   
                orarCD += Math.Min(4, orarCD +1);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [HABILIDADE] {Name} roga as estrelas, curando a si mesma..."); 
                Console.WriteLine($"> {Name}: Que o brilho de cada estrela brilhe através de meu corpo!"); 
                Console.ResetColor();
                HpAtual += HpAtual * 2/9;
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
            if (aliado != null && aliado.HpAtual <= aliado.HpMax/5 && podeOrar)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"> [PASSIVA] Cintilação do Desespero! {Name} intensificou suas orações!"); 
                Console.WriteLine($"> {Name}: ESTRELAS OUÇAM A MIM!"); 
                Console.ResetColor();
                curaBonus = aliado.HpMax * 2/3 + (Mod * 2/3); 
                podeOrar = false;
            }
            else
            {
                curaBonus = 0;
            }
        }
    }
}