using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task_U.Data;
using Task_U.Core;
using Task_U.Services;
using Task_U.Models;

namespace Task_U.Core.Combat
{
    public class CombateEngine
    {
        private CombateUI combateUI = new CombateUI();
        private TurnoJogador turnoJogador = new TurnoJogador();
        private TurnoInimigo turnoInimigo = new TurnoInimigo();
        public void Combate(User user, InventarioServices inventario, InimigoBase inimigo, List<PersonagemBase> equipe, AppDbContext context, AdventureService adventure)
        {
            combateUI.Chamada(inimigo);
            inimigo.HpAtual = inimigo.HpMax;
            foreach (var personagem in equipe)
            {
                personagem.user = user;
                personagem.HpAtual = personagem.HpMax;
                personagem.chanceAlvo = 50;
                var aliado = equipe.FirstOrDefault(x => x.Id != personagem.Id);
                personagem.aliado = aliado;

            }
            int turnoAtual = 1;
            while (equipe.Any(x => x.HpAtual > 0) && inimigo.HpAtual > 0)
            {
                inimigo.BuffAtk = 0;
                foreach (var personagem in equipe.Where(x => x.HpAtual > 0))
                {
                    personagem.BuffAtk = 0;
                    personagem.BuffMod = 0;
                    personagem.inimigoAlvo = inimigo;
                }

                foreach (var personagem in equipe.Where(x => x.HpAtual > 0))
                {
                    turnoJogador.turno(combateUI, equipe, personagem, inimigo, turnoAtual, inventario, context);
                }

                if (inimigo.HpAtual > 0 && equipe.Any(p => p.HpAtual > 0))
                {
                    turnoInimigo.turno(combateUI, equipe, inimigo, turnoAtual, user);
                }

                turnoAtual++;
            }

            if (inimigo.HpAtual <= 0)
            {
                Item? item = null;
                if (inimigo.CrystalDrop > 0)
                {
                    user.Crystals += inimigo.CrystalDrop;
                }
                if (inimigo.ItemDropId != null)
                {
                    item = context.Itens.Find(inimigo.ItemDropId);
                    var reward = new ItemInventario();
                    if(item != null)
                    {
                        reward.ItemId = item.Id;
                        reward.UserId = user.Id;
                        context.InventarioItens.Add(reward); 
                    }
                }
                combateUI.Vitoria(inimigo, item);
                user.DerrotouInimigo = true;
                adventure.AtualizarInimigo(context);
                context.SaveChanges();
            } else
            {
                combateUI.Derrota(inimigo);
            }
            
        }
    }
}