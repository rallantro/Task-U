using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Data;
using Todo_Gacha.Core;
using Todo_Gacha.Services;
using Todo_Gacha.Core.Entities;

namespace Todo_Gacha.Services
{
    public class CreateService
    {
        public void CreateCharacter(AppDbContext context)
        {
            var Lab = new Lab
            {
                Name = "Kael",
                Desc = $"Kael personifica o arquétipo do gênio incompreendido e cativante. Apesar das olheiras profundas que denunciam noites em claro e do cabelo ruivo perfeitamente bagunçado, ele possui uma beleza magnética e traços marcantes, acentuados por uma barba por fazer estilosa e olhos verdes intensos. Ele veste um jaleco branco de polímero inteligente, com circuitos embutidos que brilham sutilmente, repleto de ferramentas modulares e um tablet tático holográfico. Sua personalidade combina uma gentileza genuína com um descarado talento para a improvisação tecnológica, criando soluções brilhantes a partir do caos. {Environment.NewLine} [Habilidade: Protocolo de Assistência Adaptativa] Kael implanta sistemas de suporte tático que se ajustam instantaneamente às condições vitais do alvo. Se o aliado mantiver integridade estrutural superior a 50%, ele injeta um Módulo de Sobrecarga, amplificando diretamente o potencial dos modificadores de atributo do alvo. Caso a integridade do aliado caia abaixo de 50%, Kael alterna para Reparo de Emergência ou Sinal de Socorro, realizando restaurações teciduais diretas (Cura) com eficácia triplicada. {Environment.NewLine} [Passiva: Matriz de Sobrevivência Reativa] O traje tático de Kael calibra sua prioridade de alvo tático com base em sua própria vitalidade: ao transbordar energia (integridade alta), ele gera distrações propositais para atrair 90% da atenção inimiga, atuando como um chamariz; ao sentir-se vulnerável, ativa um campo de camuflagem reativa, reduzindo sua detecção para apenas 20% de chance. Além disso, em cenários críticos onde o aliado atinge o limiar de 20% de integridade, Kael mobiliza um Campo de Força Experimental, gerando uma barreira de energia imediata com eficácia triplicada para garantir a sobrevivência do alvo.",
                HpMax = 45,
                Rarity = 3,
                Atk = 10,
                Mod = 3,
                SummonQuote = "Eu acho que eu preciso voltar pro laboratório eu ouvi minha esposa me chamando... Você não ouviu? É que... Ah..."
            };
            context.Personagens.Add(Lab);
            context.SaveChanges();
            
        }
    }
}