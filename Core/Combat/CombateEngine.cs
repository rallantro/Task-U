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
            ConsoleKeyInfo escolha = combateUI.Chamada(inimigo);
            if (escolha.KeyChar != '1')
            {
                return;
            }
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
            inimigo.BuffAtk = 0;
            inimigo.BuffMod = 0;
            inimigo.debuffRes = 1;
            inimigo.BuffSpeed = 0;
            foreach (var personagem in equipe.Where(x => x.HpAtual > 0))
            {
                personagem.VerificarNodes(context);
                personagem.BuffAtk = 0;
                personagem.BuffMod = 0;
                personagem.BuffSpeed = 0;
                personagem.debuffRes = 1;
                personagem.inimigoAlvo = inimigo;
            }

            double tempoAcumulado = 0;
            double tempoLimite = 100;

            while (equipe.Any(x => x.HpAtual > 0) && inimigo.HpAtual > 0)
            {

                var menorAvPlayer = equipe.Where(x => x.HpAtual > 0).OrderBy(x => x.AvAtual).FirstOrDefault();
                if (menorAvPlayer != null && menorAvPlayer.AvAtual <= inimigo.AvAtual)
                {
                    foreach (var personagem in equipe.Where(x => x.HpAtual > 0 && x.Name != menorAvPlayer.Name))
                    {
                        personagem.AvAtual = Math.Max(0, personagem.AvAtual - menorAvPlayer.AvAtual);
                    }
                    inimigo.AvAtual -= menorAvPlayer.AvAtual;
                    tempoAcumulado += menorAvPlayer.AvAtual;
                    menorAvPlayer.AvAtual = 10000 / menorAvPlayer.SpeedTotal();
                    turnoJogador.turno(combateUI, equipe, menorAvPlayer, inimigo, turnoAtual, inventario, context);
                }
                else
                {
                    foreach (var personagem in equipe.Where(x => x.HpAtual > 0 && x.Name != inimigo.Name))
                    {
                        personagem.AvAtual = Math.Max(0, personagem.AvAtual - inimigo.AvAtual);
                    }
                    tempoAcumulado += inimigo.AvAtual;
                    inimigo.AvAtual = 10000 / inimigo.SpeedTotal();
                    turnoInimigo.turno(combateUI, equipe, inimigo, turnoAtual, user);
                }

                if (tempoAcumulado >= tempoLimite)
                {
                    inimigo.BuffAtk = 0;
                    inimigo.BuffMod = 0;
                    inimigo.BuffSpeed = 0;
                    foreach (var personagem in equipe.Where(x => x.HpAtual > 0))
                    {
                        personagem.BuffAtk = 0;
                        personagem.BuffMod = 0;
                        personagem.BuffSpeed = 0;
                        personagem.inimigoAlvo = inimigo;
                    }
                    tempoAcumulado = 0;
                    turnoAtual++;
                }
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
                    if (item != null)
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
            }
            else
            {
                combateUI.Derrota(inimigo);
            }

            foreach (var personagem in equipe)
            {
                personagem.Resetar();
                Item? item = personagem.itemEquipado();
                if (item != null)
                {
                    item.Resetar();
                }
            }
            inimigo.Resetar();

        }
    }
}