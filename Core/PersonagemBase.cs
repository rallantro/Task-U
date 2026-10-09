#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Task_U.Services;
using Task_U.Models;
using Task_U.Data;
using Task_U.Core.StatusEffects;

namespace Task_U.Core
{
    public class PersonagemBase
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Desc { get; set; }
        public int Rarity { get; set; }
        public int Atk { get; set; }
        public int HpMax { get; set; }

        public int Speed { get; set; }

        [NotMapped]
        public double AvAtual { get; set; }

        private int _HpAtual;

        [NotMapped]
        public int chanceAlvo { get; set; }

        [NotMapped]
        public List<StatusEffect> status { get; set; } = new();

        [NotMapped]
        public InimigoBase? inimigoAlvo { get; set; }

        [NotMapped]
        public virtual int HpAtual { get { return _HpAtual; } set { _HpAtual = Math.Max(0, Math.Min(value, HpMax)); } }
        [NotMapped]
        public int Shield { get; set; }
        [NotMapped]
        public int BuffAtk { get; set; }

        [NotMapped]
        public int BuffMod { get; set; }

        [NotMapped]
        public int BuffSpeed { get; set; }

        [NotMapped]
        public int Nodes { get; set; }

        [NotMapped]
        public double debuffRes { get; set; } = 1;

        [NotMapped]
        public bool Stuneed { get; set; }

        [NotMapped]
        public bool Silenced { get; set; }

        [NotMapped]
        public bool Blinded { get; set; }

        public int Mod { get; set; }

        [NotMapped]
        public User? user { get; set; }

        [NotMapped]
        public PersonagemBase? aliado { get; set; }

        public required string SummonQuote { get; set; }

        public void VerificarNodes(AppDbContext context)
        {
            var inventario = context.InventarioPersonagens.FirstOrDefault(x => x.PersonagemId == Id);
            Nodes = inventario?.NodesNivel ?? 0;
        }

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

        public Item? itemEquipado()
        {
            if (this == user?.Slot1_PersonagemAtivo) return user.Slot1_ItemAtivo;
            else if (this == user?.Slot2_PersonagemAtivo) return user.Slot2_ItemAtivo;
            else return null;
        }

        public int ModTotal()
        {
            Item? item = itemEquipado();
            if (item != null && item.Atr == 3)
            {
                return Math.Max(0, Mod + BuffMod + item.Mod);
            }
            else
            {
                return Math.Max(0, Mod + BuffMod);
            }

        }

        public virtual int SpeedTotal()
        {
            Item? item = itemEquipado();
            int bonusItem = (item != null && item.Atr == 4) ? item.Mod : 0;
            int bonusEffects = status.OfType<BonusSPD>().Sum(s => s.Mod ?? 0);
            int debuff = status.OfType<DebuffSPD>().Sum(s => s.Mod ?? 0);
            return Math.Max(1, Speed + BuffSpeed + bonusItem + bonusEffects - debuff);
        }

        public int AtkTotal()
        {
            Item? item = itemEquipado();
            if (item != null && item.Atr == 2)
            {
                return Math.Max(0, Atk + BuffAtk + item.Mod);
            }
            else
            {
                return Math.Max(0, Atk + BuffAtk);
            }
        }

        public virtual int Damage()
        {
            return AtkTotal();
        }

        public virtual void tomarDano(string inimigo, int dano)
        {
            int danoTotal = Math.Max(0, (int)Math.Ceiling(dano * debuffRes) - Shield);
            int danoShield = Math.Min(Shield, (int)Math.Ceiling(dano * debuffRes));
            Shield -= danoShield;
            HpAtual = Math.Max(0, HpAtual -= danoTotal);
            if (danoShield > 0 && danoTotal == 0)
            {
                Console.WriteLine($"{Name} bloqueou completamente o ataque de {inimigo} com seu escudo!");
            }
            else
            {
                string danoOg = $"";
                if (debuffRes != 1)
                {
                    danoOg = $" (dano original: {dano - Shield})";
                }
                Console.WriteLine($"{inimigo} atacou {Name} e causou {danoTotal} de dano{danoOg}!");

            }
        }

        public virtual void curar(string aliado, int cura)
        {
            HpAtual = Math.Min(HpAtual + cura, HpMax);
        }

        public virtual void Habilidade()
        {

        }

        public virtual void Passiva()
        {

        }

        public virtual void Resetar()
        {
            BuffAtk = 0;
            BuffSpeed = 0;
            Silenced = false;
            Stuneed = false;
            Blinded = false;
            Shield = 0;
            BuffAtk = 0;
            BuffMod = 0;
            chanceAlvo = 50;
            inimigoAlvo = null;
            aliado = null;
            status.Clear();
        }
    }
}