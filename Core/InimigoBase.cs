using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Services;
using System.ComponentModel.DataAnnotations.Schema;
using Task_U.Models;
using Task_U.Data;
using Task_U.Core;
using Task_U.Core.StatusEffects;

namespace Task_U.Core
{
    public class InimigoBase
    {
        public enum TipoBoss
        {
            Buffer,
            Dot,
            Controller,
            Tank,
            Healer,
            Berserker,
        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Desc { get; set; }
        public int Atk { get; set; }
        public int HpMax { get; set; }
        public int Mod { get; set; }
        public int HabilidadeChance { get; set; }
        public int CrystalDrop { get; set; }
        public int? ItemDropId { get; set; }
        public string? DeathQuote { get; set; }
        public int Rarity { get; set; }

        public int Speed { get; set; }

        [NotMapped]
        public double AvAtual { get; set; }

        public TipoBoss? tipoBoss { get; set; }
        private int _HpAtual;

        [NotMapped]
        public Random rand = new Random();

        [NotMapped]
        public int HpAtual { get { return _HpAtual; } set { _HpAtual = Math.Max(0, Math.Min(value, HpMax)); } }
        [NotMapped]
        public int Shield { get; set; }

        [NotMapped]
        public int BuffAtk { get; set; }

        [NotMapped]
        public int BuffMod { get; set; }

        [NotMapped]
        public int BuffSpeed { get; set; }

        [NotMapped]
        public double debuffRes { get; set; } = 1;

        [NotMapped]
        public List<StatusEffect> status { get; set; } = new();

        [NotMapped]
        public bool Stuneed { get; set; }

        [NotMapped]
        public bool Silenced { get; set; }

        [NotMapped]
        public bool Blinded { get; set; }

        [NotMapped]
        public List<PersonagemBase>? alvos { get; set; }

        public virtual void aplicarEfeitos()
        {
            foreach (var effect in status.ToList())
            {
                effect.Aplicar(this);
                if (effect.Duration == 0)
                {
                    status.Remove(effect);
                }
            }
        }


        public virtual void tomarDano(PersonagemBase inimigo, int dano)
        {
            int danoTotal = (int)Math.Max(0, Math.Ceiling(dano * debuffRes) - Shield);
            int danoShield = Math.Min(Shield, (int)Math.Max(0, Math.Ceiling(dano * debuffRes)));
            Shield -= danoShield;
            HpAtual -= danoTotal;
            if (danoShield > 0 && danoTotal == 0)
            {
                Console.WriteLine($"{Name} bloqueou completamente o ataque de {inimigo.Name} com seu escudo!");
            }
            else
            {
                if (danoShield > 0 && Shield == 0)
                {
                    Console.WriteLine($"{inimigo.Name} atacou {Name} e destruiu seu escudo!");
                }
                string danoOg = $"";
                if (debuffRes != 1)
                {
                    danoOg = $" (dano original: {dano - Shield})";
                }
                Console.WriteLine($"{inimigo.Name} atacou {Name} e causou {danoTotal} de dano{danoOg}!");
            }
        }

        public virtual int Damage()
        {
            return Math.Max(0, Atk + BuffAtk);
        }

        public virtual int SpeedTotal()
        {
            return Math.Max(1, Speed + BuffSpeed);
        }
        public virtual void Habilidade()
        {

        }

        public virtual void Passiva(User user)
        {

        }

        public virtual PersonagemBase EscolherAlvo()
        {
            int chanceTotal = 0;
            if (alvos == null || alvos.Count == 0)
            {
                throw new Exception("Erro desconhecido. O inimigo não conseguiu ver nenhum alvo na equipe. Contate o suporte.");
            }
            foreach (var personagem in alvos)
            {
                chanceTotal += personagem.chanceAlvo;
            }
            int chance = rand.Next(0, chanceTotal);
            foreach (var personagem in alvos)
            {
                if (chance < personagem.chanceAlvo)
                {
                    return personagem;
                }
                chance -= personagem.chanceAlvo;
            }
            return alvos[0];
        }

        public virtual void aoUsarSkill(PersonagemBase personagem)
        {

        }

        public virtual void Resetar()
        {
            BuffAtk = 0;
            Silenced = false;
            Blinded = false;
            Stuneed = false;
            Shield = 0;
        }
    }
}