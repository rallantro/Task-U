using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Services;
using Todo_Gacha.Models;
using Todo_Gacha.Data;
using System.Security.Cryptography.X509Certificates;

namespace Todo_Gacha.Core
{
    public class Item
    {
        public int Id { get; set; }
        public string Name {get; set;}
        public string Desc {get; set;}
        public int Rarity {get; set;}
        public enum ItemType { Consumivel = 1, Modificador = 2 }
        public enum Atributo { Hp = 1, Atk = 2, Mod = 3 }
        public int Type {get; set;} //Tipo 1 é Consumível, Tipo 2 é de Modificador
        public int Atr {get; set;} //Define qual atributo fará o que. 1 - Hp, 2 - Atk, 3 - Mod
        public int Mod {get; set;}

        public virtual void Effect(){

        }

        public void Uso(PersonagemBase Usuario, AppDbContext context){
            if (Type == (int)ItemType.Consumivel)
            {
                if(Atr == (int)Atributo.Hp)
                {
                    Console.WriteLine($"{Usuario.Name} recuperou {Mod} pontos de vida!");
                    Usuario.HpAtual = Math.Min(Usuario.HpAtual + Mod, Usuario.HpMax);;
                    var itemInv = context.InventarioItens.Where(x => x.ItemId == this.Id).FirstOrDefault();
                    context.InventarioItens.Remove(itemInv);
                    context.SaveChanges();
                }
            }
        }
    }
}