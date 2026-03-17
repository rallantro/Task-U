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
            var fada = new Fada
            {
                Name = "Fada de Nanobots",
                Desc = "Um enchame de nano robôs que se moldam na forma de uma fada, uma esfera brilhante e azul, como vírus de aprimoramento que consome tecnologia e magia.",
                HpMax = 100,
                Rarity = 1,
                Atk = 4,
                CrystalDrop = 1,
                Mod = 10,
                HabilidadeChance = 70,
                DeathQuote = "Prriiii..."
            };
            context.Inimigos.Add(fada);
            context.SaveChanges();

            var QFada = new FadaRa
            {
                Name = "Rainha dos Nanobots",
                Desc = "Protegida no subterrâneo, um aglomerado de nanobots que assumiu uma forma feminina majestosa, capaz de controlar outros enxames. Brilha com luz própria e emite um zumbido harmônico.",
                HpMax = 400,
                Rarity = 4,
                Atk = 10,
                CrystalDrop = 8,
                Mod = 8,
                HabilidadeChance = 70,
                DeathQuote = "Isso... É impossível... Priiiii"
            };
            context.Inimigos.Add(QFada);
            context.SaveChanges();

            var Bukubus = new SlimeA
            {
                Name = "Bukubus",
                Desc = "Bukubus é um slime que transborda uma alegria efervescente e uma energia indomável, sendo a personificação viva de uma maré agitada. Seu corpo é inteiramente composto por uma substância aquosa translúcida de um azul cristalino, revelando uma musculatura levemente definida sob sua superfície fluida. Seus cabelos são mechas de água viva que parecem flutuar no ar, finalizados com uma pequena bolha saltitante no topo. Ele veste uma bermuda tática urbana em tons de azul-marinho e roxo, caída, mostrando parte de sua roupa de baixo e tiras de compressão que reforçam seu estilo streetwear futurista. Com um sorriso radiante e olhos que brilham como o reflexo do sol no oceano, ele se lança ao combate tratando cada confronto como uma grande e divertida brincadeira na arrebentação. {Environment.NewLine} [Habilidade: Disparo Hidro-Cinético] Bukubus molda a água ao seu redor em esferas de alta pressão e as dispara contra os inimigos. O dano é potencializado pelo seu estado de Fluxo atual e, ao atingir o alvo, as bolhas grudam em suas armas, reduzindo drasticamente a força ofensiva do adversário. {Environment.NewLine} [Passiva: Efervescência de Maré] Ao atingir sua plenitude vital, o corpo de Bukubus transborda, criando uma barreira superficial de bolhas que explode em um contra-ataque aquático assim que é rompida. Durante o combate, cada golpe recebido ou cura absorvida agita suas correntes internas, acumulando Fluxo; quando esse fluxo atinge o ápice, Bukubus torna-se capaz de absorver impactos massivos e devolvê-los como uma onda de choque devastadora.",
                HpMax = 65,
                Rarity = 4,
                Atk = 10,
                Mod = 4,
                SummonQuote = "Splash! O slime da maré chegou para animar a festa!"
            };
            context.Personagens.Add(Bukubus);
            context.SaveChanges();
            
        }
    }
}