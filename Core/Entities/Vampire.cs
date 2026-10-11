using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Task_U.Core.StatusEffects;

namespace Task_U.Core.Entities
{
    public class Vampire : PersonagemBase
    {
        private int FrenesiDown;
        private int FrenesiTurns;
        private int SangueStack;
        private double modificador;
        private bool Frenesi;
        public override int Damage()
        {
            int vidaPerdida;
            if (HpAtual == HpMax || aliado == null || aliado.HpAtual <= 0)
            {
                modificador = Nodes >= 1 ? 0.25 : 0.35;
                vidaPerdida = (int)Math.Ceiling(HpMax * 0.35);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> {Name}: Ahh! Meu sangue também gosta de brincar...");
                Console.ResetColor();
                tomarDano(Name, (int)Math.Ceiling(HpMax * modificador));
            }
            else
            {
                vidaPerdida = aliado.HpMax - aliado.HpAtual;
            }
            if (!Frenesi)
            {
                modificador = Nodes >= 3 ? 1.88 : 1.58;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> {Name}: É tão vermelho...");
                Console.ResetColor();
                int dano = (int)Math.Ceiling(vidaPerdida * AtkTotal() * 0.03);
                return AtkTotal() + dano;
            }
            else if (aliado != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($">  [FRENESI] {Name} derramará muito sangue!");
                Console.WriteLine($"> {Name}: ISSO É DELICIOSO!");
                Console.ResetColor();

                modificador = Nodes >= 2 ? 3.5 : 2.7;
                int dano = (int)Math.Ceiling(AtkTotal() * (1 - (double)aliado.HpAtual / aliado.HpMax + 0.35) * modificador + vidaPerdida * AtkTotal() * 0.08);
                int cura = (int)Math.Ceiling((AtkTotal() + dano) * 0.33);
                double pctAliadoFerido = 1 - (double)aliado.HpAtual / aliado.HpMax;
                double pctVampireFerido = 1 - (double)HpAtual / HpMax;
                double totalFerido = pctAliadoFerido + pctVampireFerido;

                int curaAliado = (int)Math.Ceiling(cura * (pctAliadoFerido / totalFerido));
                curar(Name, cura - curaAliado);
                aliado.curar(Name, curaAliado);
                return AtkTotal() + dano;
            }
            else
            {
                return AtkTotal();
            }

        }

        public override void Habilidade()
        {
            if (aliado != null)
            {

                modificador = Nodes >= 1 ? 0.18 : 0.22;
                int danoAliado = (int)Math.Ceiling(aliado.HpAtual * modificador);
                modificador = Nodes >= 4 ? 0.72 : 0.58;
                SangueStack = Math.Min(100, SangueStack + (int)Math.Ceiling(danoAliado * modificador));
                aliado.HpAtual -= danoAliado;
                AplicarBuff(aliado);
                AplicarBuff(this);
                FrenesiDown++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [HABILIDADE] {Name} tenta drenar alguém, mas está sozinho!");
                Console.WriteLine($"> {Name}: Detesto batalhas sozinho...");
                Console.ResetColor();
            }

        }

        public override void Passiva()
        {
            if (Frenesi)
            {
                FrenesiTurns--;
                if (FrenesiTurns <= 0)
                {
                    SangueStack = 0;
                    Frenesi = false;
                    Console.WriteLine($"> [FRENESI] {Name} se acalma...");
                }
            }
            if (FrenesiDown >= 5 && aliado != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [PASSIVA] {Name} está entrando em frenesi!");
                Console.WriteLine($"> {Name}: S-SANGUEEEEEE!");
                Console.ResetColor();
                Frenesi = true;
                FrenesiTurns = 3;
                FrenesiDown = 0;
            }
        }

        public override void Resetar()
        {
            FrenesiDown = 0;
            Frenesi = false;
            FrenesiTurns = 0;
            SangueStack = 0;
            base.Resetar();
        }

        private void AplicarBuff(PersonagemBase personagem)
        {
            modificador = Nodes >= 5 ? 0.42 : 0.35;
            var buff = personagem.status.FirstOrDefault(x => x.Name == "Fervor Sanguíneo");
            if (buff == null)
            {
                buff = new BonusDMG("Fervor Sanguíneo", 1, (int)Math.Ceiling(personagem.AtkTotal() * (double)SangueStack/50 * modificador));
                personagem.status.Add(buff);
            }
            else
            {
                buff.Duration = 1;
                buff.Mod = (int)Math.Ceiling(personagem.AtkTotal() * (double)SangueStack/50 * modificador);
            }
        }
    }
}