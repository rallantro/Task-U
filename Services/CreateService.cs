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
            var Vampire = new Vampire();
            Vampire.Name = "Valentine";
            Vampire.Desc = $"Valentine é um jovem de 18 anos com uma presença magnética e perigosa, ostentando cabelos negros curtos com mechas vermelhas e olhos escarlates que brilham no escuro. Ele veste um conjunto tático urbano com um sobretudo de couro preto e detalhes em metal, carregando diversos frascos de contenção presos a um arnês no peito, onde armazena o sangue que manipula com precisão cirúrgica. {Environment.NewLine} [Habilidade: Transfusão Parasitária] Valentine drena a vitalidade de um aliado para curar suas próprias feridas. Caso ele já esteja com a saúde plena, ele sacrifica o próprio sangue para injetar vigor no aliado, mantendo o fluxo constante de vida e dor no campo de batalha. {Environment.NewLine} [Passiva: Êxtase do Hemomante] Ao atacar, Valentine canaliza a agonia de aliados feridos para aumentar seu dano. Se estiver sozinho ou saudável demais, ele golpeia as próprias veias, sacrificando 75% de sua vida máxima para garantir um ataque devastador. Após realizar 4 drenagens, Valentine entra em Frenesi, consumindo o sangue do aliado para multiplicar drasticamente o bônus de dano baseado na vida perdida da equipe.";
            Vampire.Atk = 15;
            Vampire.HpMax = 40;
            Vampire.Mod = 5;
            Vampire.Rarity = 4;
            Vampire.SummonQuote = "Meu sangue ou o seu? Ah, que tal os dois?";
            context.Personagens.Add(Vampire);
            context.SaveChanges();

            var Star = new Star();
            Star.Name = "Astraea";
            Star.Desc = $"Astraea possui uma beleza serena e quase intocável. Seus cabelos negros longos e lisos emolduram um rosto pálido com olhos escuros que parecem conter o vazio do espaço. Ela veste uma versão moderna de um uniforme escolar japonês mesclado com elementos de alta costura: uma saia plissada preta com detalhes em fios de ouro, meias altas e um sobretudo longo com forro em degradê de azul-noite para dourado. Em combate, ela segura um rosário de âmbar que emite um brilho suave toda vez que ela sussurra uma prece.  {Environment.NewLine} [Habilidade: Prece do Alvorecer] Astraea sacrifica sua própria energia vital para restaurar o vigor do seu aliado. Quando está sozinha, sua fé se volta para dentro, regenerando suas próprias feridas com a luz estelar. {Environment.NewLine} [Passiva: Cintilação do Desespero] Quando seu aliado atinge o limite do cansaço (20% de vida), Astraea intensifica suas orações, extraindo mais poder das estrelas distantes para garantir que a chama da vida não se apague.";
            Star.Atk = 12;
            Star.HpMax = 50;
            Star.Mod = 4;
            Star.Rarity = 3;
            Star.SummonQuote = "Sob minha luz, ninguém cairá.";
            context.Personagens.Add(Star);
            context.SaveChanges();
        }
    }
}