using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Services;
using Task_U.Models;
using Task_U.Data;
using System.Security.Cryptography.X509Certificates;

#nullable enable

namespace Task_U.Core
{
    public class Item
    {
        public int Id { get; set; }
        public required string Name {get; set;}
        public required string Desc {get; set;}
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
                    Usuario.curar(Name, Mod);
                    var itemInv = context.InventarioItens.Where(x => x.ItemId == this.Id).FirstOrDefault();
                    if(itemInv != null) context.InventarioItens.Remove(itemInv);
                    context.SaveChanges();
                }
                if(Atr == (int)Atributo.Mod)
                {
                    Console.WriteLine($"{Usuario.Name} recebeu + {Mod} temporário!");
                    Usuario.BuffMod += Mod;
                    var itemInv = context.InventarioItens.Where(x => x.ItemId == this.Id).FirstOrDefault();
                    if(itemInv != null) context.InventarioItens.Remove(itemInv);
                    context.SaveChanges();   
                }
            }
        }
    }
}