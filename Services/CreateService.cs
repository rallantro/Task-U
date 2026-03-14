using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Data;
using Todo_Gacha.Core;
using Todo_Gacha.Services;

namespace Todo_Gacha.Services
{
    public class CreateService
    {
        public void CreateCharacter(AppDbContext context)
        {
            var James = new Grafiteiro();
            James.Name = "Jax";
            James.Desc = $"Jax é um adolescente de pele clara e cabelos espetados em um tom de vermelho vibrante, combinando com seus olhos cor de âmbar que brilham com travessura. Ele ostenta um estilo Y2K com calças cargo largas, um cinto de utilidades cheio de sprays e uma camisa oversized vermelha, sempre exibindo um sorriso descontraído enquanto desliza com seu skate pelas ruas. {Environment.NewLine} [Habilidade: Agitar a Lata] Jax entra em modo de pintura, chacoalhando seus sprays para preparar uma obra-prima. Ele alterna entre focar na arte ou explodir tudo o que pintou na cara dos inimigos. {Environment.NewLine} [Passiva: Camadas de Tinta] Enquanto estiver pintando, Jax acumula pressão e pigmentos a cada turno. Quanto mais tempo ele passar grafitando, maior e mais colorida será a detonação final.";
            James.SummonQuote = "Minha arte logo vai fazer KABOM!";
            James.Atk = 2;
            James.Hp = 60;
            James.Mod = 5;
            James.Rarity = 3;
            context.Personagens.Add(James);
            context.SaveChanges();
        }
    }
}