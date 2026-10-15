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
        private Random rand = new Random();
        public override int Damage()
        {
            int vidaPerdida;
            if (HpAtual == HpMax || aliado == null || aliado.HpAtual <= 0)
            {
                modificador = Nodes >= 1 ? 0.25 : 0.35;
                vidaPerdida = (int)Math.Ceiling(HpMax * 0.35);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> {Name}: Como é bom sentir dor...");
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
                Console.WriteLine($"> {Name}: Ahhhh! Como eu queria ter um pouco mais de sangue...");
                Console.ResetColor();
                int dano = (int)Math.Ceiling(vidaPerdida * AtkTotal() * 0.03);
                if (Nodes >= 6)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> [MAR VERMELHO] {Name} usurpa o sangue inimigo.");
                    Console.WriteLine($"> {Name}: Vem brincar também...");
                    Console.ResetColor();
                    SangueStack += (int)Math.Ceiling((AtkTotal() + dano) * 0.45);
                }
                return AtkTotal() + dano;
            }
            else if (aliado != null)
            {
                string[] falas =
                {
                    "ARHHHH O GOSTO DO SANGUE!",
                    "MINHAS VEIAS ESTÃO FERVENDO!",
                    "ISSO VAI SER UM BANQUETE!",
                    "MAIS MAIS MAIS MAIS!",
                    "QUE DELEITE!",
                    "UM MAR DE SANGUE!"
                };
                string falaEscolhida = falas[rand.Next(falas.Length)];
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($">  [FRENESI] Os olhos de {Name} brilham em vermelho. Com um sorriso largo ele pula no inimigo e espalha sangue por todos os lados.");
                Console.WriteLine($"> {Name}: {falaEscolhida}");
                Console.ResetColor();

                modificador = Nodes >= 2 ? 3.5 : 2.7;
                int dano = (int)Math.Ceiling(AtkTotal() * (1 - (double)aliado.HpAtual / aliado.HpMax + 0.35) * modificador + vidaPerdida * AtkTotal() * 0.08);
                int cura = (int)Math.Ceiling((AtkTotal() + dano) * 0.24);
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
            if (aliado != null && aliado.HpAtual > 0)
            {
                string[] falas =
                {
                    "Você vai me perdoar, eu juro...",
                    $"{aliado.Name}, você vai me perdoar...",
                    "Consegue sentir o prazer?",
                    "Só vou pegar o necessário...",
                    "Não faz esse rostinho, eu sei que você está gostando...",
                    "Isso é para seu próprio bem..."
                };
                string falaEscolhida = falas[rand.Next(falas.Length)];
                modificador = Nodes >= 1 ? 0.18 : 0.22;
                int danoAliado = (int)Math.Ceiling(aliado.HpAtual * modificador);
                modificador = Nodes >= 4 ? 0.72 : 0.58;
                SangueStack = Math.Min(100, SangueStack + (int)Math.Ceiling(danoAliado * modificador));
                aliado.HpAtual -= danoAliado;
                 Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [TRANSFUSÃO VISCERAL] {Name} usa o sangue de {aliado.Name} para fortalecer a equipe!");
                Console.WriteLine($"> {Name}: {falaEscolhida}");
                Console.WriteLine($"> {aliado.Name} recebeu {danoAliado} de dano!");
                Console.ResetColor();
                AplicarBuff(aliado);
                AplicarBuff(this);
                FrenesiDown++;
               
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [TRANSFUSÃO VISCERAL] {Name} tenta drenar alguém, mas está sozinho!");
                Console.WriteLine($"> {Name}: Preciso de mais alguém para brincar...");
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> [FRENESI DE SANGUE] {Name} se acalma...");
                    Console.WriteLine($"> {Name}: Preciso de sangue pra brincar...");
                    Console.ResetColor();
                }
            }
            if (FrenesiDown >= 5 && aliado != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> [FRENESI DE SANGUE] Os olhos de {Name} brilham em vermelho, suas veias saltam e suas unhas parecem ficar mais afiadas.");
                Console.WriteLine($"> {Name}: FINALMENTE! FINALMENTE LIVRE!");
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
            int valor = (int)Math.Ceiling(personagem.AtkTotal() * (double)SangueStack / 50 * modificador);
            if (buff == null)
            {
                buff = new BonusDMG("Fervor Sanguíneo", 1, valor);
                personagem.status.Add(buff);
            }
            else
            {
                buff.Duration = 1;
                buff.Mod = valor;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"> [SANGUE FERVENTE] {personagem.Name} recebeu o sangue de {Name}. (+{valor} de Ataque)");
            Console.ResetColor();
        }
    }
}