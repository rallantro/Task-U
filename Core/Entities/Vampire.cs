using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Todo_Gacha.Core.Entities
{
    public class Vampire : PersonagemBase
    {
        private int FrenesiDown;
        private bool Frenesi;
        public override int Damage()
        {
            int vidaPerdida;
            if (HpAtual == HpMax || aliado.HpAtual <= 0 || aliado == null)
            {
                vidaPerdida = HpMax - HpMax/4;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> {Name}: Ahh! Meu sangue também gosta de brincar..."); 
                Console.ResetColor();
                tomarDano(Name, vidaPerdida);
            }
            else
            {
              vidaPerdida = aliado.HpMax - aliado.HpAtual;  
            }
            if (!Frenesi)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> {Name}: É tão vermelho..."); 
                Console.ResetColor();
                return AtkTotal() + vidaPerdida;  
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($">  [FRENESI] {Name} derramará muito sangue!"); 
                Console.WriteLine($"> {Name}: ISSO É DELICIOSO!"); 
                Console.ResetColor();
                Frenesi = false;
                FrenesiDown = 0;
                return AtkTotal() + vidaPerdida * Mod/2;
            }
            
        }

        public override void Habilidade()
        {
            if (aliado != null)
            {
                
                if(HpAtual >= HpMax)
                {
                    var curaTotal = Math.Max(0, Mod - Shield);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> [HABILIDADE] {Name} cura {curaTotal} pontos de vida de {aliado.Name}!");
                    Console.WriteLine($"> {Name}: Argh... Dá para tentar ficar vivo?"); 
                    Console.ResetColor();
                    HpAtual -= curaTotal;
                    aliado.HpAtual += curaTotal;
                }
                else
                {
                    var curaTotal = Math.Max(0, Mod - aliado.Shield);
                    if (curaTotal > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"> [HABILIDADE] {Name} drena {Mod} pontos de vida de {aliado.Name}!");
                        Console.WriteLine($"> {Name}: Isso pode mechucar um pouco..."); 
                        Console.ResetColor();
                        aliado.tomarDano(Name, Mod);
                        HpAtual += curaTotal;
                        FrenesiDown++;  
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [HABILIDADE] {Name} tenta drenar alguém, mas está sozinho!");
                Console.WriteLine($"> {Name}: Detesto batalhas sozinho..."); 
                Console.ForegroundColor = ConsoleColor.White;
            }
            
        }

        public override void Passiva()
        {
            if (FrenesiDown >= 4)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [PASSIVA] {Name} está entrando em frenesi!");
                Console.WriteLine($"> {Name}: S-SANGUEEEEEE!");
                Console.ResetColor();
                aliado.tomarDano(Name, Mod);
                Frenesi = true;
            }    
        }
    }
}