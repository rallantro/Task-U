#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Data;
using Todo_Gacha.Models;
using Todo_Gacha.Services;

namespace Todo_Gacha.Core.Combat
{
    public class TurnoInimigo
    {
        private Random rand = new Random();
        public void turno(CombateUI combateUI, List<PersonagemBase> equipe, InimigoBase inimigo, int x, User user)
        {
            combateUI.Cabecalho(equipe, inimigo, x);

            if (inimigo.TurnoStun > 0)
            {
                combateUI.ExibirMensagem($"{inimigo.Name} está atordoado e não pode agir!", ConsoleColor.Red);
                combateUI.AguardarTecla();
                inimigo.TurnoStun--;
                return;
            }

            inimigo.Passiva(user);
            var vivos = equipe.Where(p => p.HpAtual > 0).ToList();
            if (vivos.Count() == 0)
            {
                return;
            }
            inimigo.alvos = vivos;
            if (inimigo.TurnoSilence > 0)
            {
                combateUI.ExibirMensagem($"{inimigo.Name} está silenciado e não pode usar suas habilidades!", ConsoleColor.Yellow);
                combateUI.AguardarTecla();
            }
            else
            {
                inimigo.Habilidade();
            }

            int danoInimigo = inimigo.Damage();

            int chanceTotal = 0;
            foreach (var personagem in vivos)
            {
                chanceTotal += personagem.chanceAlvo;
            }
            int chance = rand.Next(0, chanceTotal);
            PersonagemBase alvo = vivos.FirstOrDefault();
            foreach (var personagem in vivos)
            {
                if (chance < personagem.chanceAlvo)
                {
                    alvo = personagem;
                    break;

                }
                chance -= personagem.chanceAlvo;
            }
            alvo.tomarDano(inimigo.Name, danoInimigo);
            combateUI.ExibirMensagem("\nPressione qualquer tecla para o próximo turno...", ConsoleColor.White);
            combateUI.AguardarTecla();
            inimigo.TurnoSilence = Math.Max(0, inimigo.TurnoSilence-1);
        }
    }
}