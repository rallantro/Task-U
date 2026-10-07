using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Task_U.Core.Entities;
using System.Xml;


namespace Task_U.Core.StatusEffects
{
    public class ScribeBoom : StatusEffect
    {
        private double Res;
        private int Dmg;
        private int time;
        private bool Node;
        public override void Aplicar(InimigoBase personagem)
        {
            if (Duration > 0)
            {
                personagem.BuffSpeed -= Mod.Value;
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"[{Name.ToUpper()}] {personagem.Name} perdeu {Mod.Value} de velocidade!");
                Console.ResetColor();
                if (Duration == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"[FRANZIDO CRIOGÊNICO] O tear que prendia {personagem.Name} foi estourado!");
                    Console.ResetColor();
                    if (personagem.HpAtual <= personagem.HpMax * 0.3 && Node)
                    {
                        Dmg = (int)Math.Ceiling(Dmg * 1.7);
                        var Silence = new Silence("Carcela Glácia", 2);
                        personagem.status.Add(Silence);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"[CARCELA GLÁCIA] {personagem.Name} foi silênciado pelo tear gélido, por 2 turnos!");
                        Console.ResetColor();
                    }
                    int danoTotal = (int)Math.Max(0, Math.Ceiling(Dmg * personagem.debuffRes) - personagem.Shield);
                    int danoShield = Math.Min(personagem.Shield, (int)Math.Max(0, Math.Ceiling(Dmg * personagem.debuffRes)));
                    personagem.Shield -= danoShield;
                    personagem.HpAtual -= danoTotal;
                    if (danoShield > 0 && danoTotal == 0)
                    {
                        Console.WriteLine($"{Name} bloqueou completamente o franzido criogêncio com seu escudo!");
                    }
                    else
                    {
                        if (danoShield > 0 && personagem.Shield == 0)
                        {
                            Console.WriteLine($"O franzido criogêncio explodiu em {Name} e destruiu seu escudo!");
                        }
                        Console.WriteLine($"O franzido criogêncio explodiu em {Name} e causou {danoTotal} de dano!");
                    }
                    var deRes = new DebuffRes("Laçada Gélida", Math.Max(time, 2), Res);
                    deRes.Aplicar(personagem);
                    personagem.status.Add(deRes);
                    deRes.Aplicar(personagem);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"[LAÇADA GÉLIDA] {personagem.Name} está exposto ao fluxo do tempo! O choque o enfraqueceu!");
                    Console.ResetColor();
                    Console.WriteLine($"{personagem.Name} recebeu {deRes.Name}. (Recebe +{deRes.Mod * 100}% de dano por {deRes.Duration} turno(s))");
                }
                Duration--;
                time++;
            }
            base.Aplicar(personagem);
        }

        [SetsRequiredMembers]
        public ScribeBoom(string name, int duration, int mod, double res, int dmg, bool node) : base(name, duration, mod)
        {
            Res = res;
            Dmg = dmg;
            Node = node;
        }
    }
}