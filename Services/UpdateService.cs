using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Task_U.Data;
using Task_U.Models;
using Task_U.Core;
using Task_U.Services;
using Task_U.Core.Entities;
using Task_U.Core.Enemies;
using Task_U.Core.Itens;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Task_U.Services
{
    public class UpdateService
    {
        private Config baseConfig = new Config { Name = "A Brasa Ardente", Value = "1.6.0" };
        public void Verify(AppDbContext context, Config ActualVersion)
        {
            var oldVersion = context.Config.Find(1);
            if (oldVersion == null)
            {
                oldVersion = baseConfig;
                context.Config.Add(oldVersion);
                context.SaveChanges();
            }

            if (oldVersion.Value != ActualVersion.Value)
            {
                UpdateVersion(oldVersion, context);
            }
        }

        public void UpdateVersion(Config oldVersion, AppDbContext context)
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("    ATUALIZANDO SEU JOGO...     ");
            Console.WriteLine("=================================\n");

            var version = context.Config.Find(1) ?? baseConfig;
            List<(string name, string version, Action<AppDbContext> action)> supportVersions = new List<(string name, string, Action<AppDbContext>)>{
                {("Os Tempos Caídos","1.4.8", UpdatePatch_1_4_8)},
                {("A Brasa Ardente","1.6.0", UpdatePatch_1_6_0)}
            };
            int oldPostion = supportVersions.FindIndex(x => x.version == oldVersion.Value);
            if (oldPostion == -1)
            {
                throw new ArgumentException($"A sua versão ({oldVersion.Value}) não é suportada para atualização. Por favor, reinstale o jogo a partir da versão mínima (1.4.8).");
            }
            for (int i = oldPostion + 1; i < supportVersions.Count; i++)
            {
                var patch = supportVersions[i];
                Console.WriteLine($"-> Aplicando patch {patch.version}: {patch.name}...");
                supportVersions[i].action(context);
                version.Name = patch.name;
                version.Value = patch.version;
                context.SaveChanges();
                Console.WriteLine($"   [✔] Sucesso!\n");
            }
            Console.WriteLine("Atualização concluída! Iniciando o jogo...");
            Thread.Sleep(1500);
        }

        static void UpdatePatch_1_4_8(AppDbContext context)
        {
            // versão base do jogo
        }
        static void UpdatePatch_1_6_0(AppDbContext context)
        {
            // ============ CORREÇÕES =========================================
            var dragao = context.Inimigos.FirstOrDefault(x => x.Name == "Anomalia Sombria");
            var resquicio = context.Itens.FirstOrDefault(x => x.Name == "Resquício do Caos");
            if (dragao != null && resquicio != null)
            {
                dragao.ItemDropId = resquicio.Id;
            }
            else
            {
                throw new ArgumentException($"Base de dados corrompida: o item 'Resquício do Caos' ou o inimigo 'Anomalia Sombria' não existe. " +
    $"Reinstale o jogo a partir da versão 1.4.8.");
            }
            context.Database.ExecuteSqlRaw(
               "UPDATE Itens SET Discriminator = 'resquicioDragao' WHERE Name = 'Resquício do Caos' AND Discriminator = 'Item'"
           );


            // ============ ATUALIZAÇÕES DE PERSONAGENS EXISTENTES ============

            var jax = context.Personagens.FirstOrDefault(x => x.Name == "Jax");
            if (jax == null)
            {
                jax = new Grafiteiro
                {
                    Name = "Jax",
                    Atk = 12,
                    HpMax = 300,
                    Mod = 6,
                    Rarity = 3,
                    Speed = 105,
                    Desc = "Jax é um adolescente de pele clara e cabelos espetados em um tom de vermelho vibrante, combinando com seus olhos cor de âmbar que brilham com travessura. Ele ostenta um estilo Y2K com calças cargo largas, um cinto de utilidades cheio de sprays e uma camisa oversized vermelha, sempre exibindo um sorriso descontraído enquanto desliza com seu skate pelas ruas. \r\n[Habilidade: Muralha de Tinta] Jax usa tinta acumulada para: Explosão (dano massivo), Debuff (cegueira) ou Buff (Ataque e Velocidade para si e aliado).\r\n[Passiva: Camadas de Tinta] No modo de pintura, acumula camadas a cada turno. Se ultrapassar o limite, explode automaticamente no próximo ataque, causando um dano massivo.",
                    SummonQuote = "Minha arte logo vai fazer KABOM!"
                };
                context.Personagens.Add(jax);
            }
            else
            {
                jax.Desc = "Jax é um adolescente de pele clara e cabelos espetados em um tom de vermelho vibrante, combinando com seus olhos cor de âmbar que brilham com travessura. Ele ostenta um estilo Y2K com calças cargo largas, um cinto de utilidades cheio de sprays e uma camisa oversized vermelha, sempre exibindo um sorriso descontraído enquanto desliza com seu skate pelas ruas. \r\n[Habilidade: Muralha de Tinta] Jax usa tinta acumulada para: Explosão (dano massivo), Debuff (cegueira) ou Buff (Ataque e Velocidade para si e aliado).\r\n[Passiva: Camadas de Tinta] No modo de pintura, acumula camadas a cada turno. Se ultrapassar o limite, explode automaticamente no próximo ataque, causando um dano massivo.";
                jax.Atk = 12;
                jax.HpMax = 300;
                jax.Mod = 6;
                jax.Speed = 105;
                jax.SummonQuote = "Minha arte logo vai fazer KABOM!";
            }

            var star = context.Personagens.FirstOrDefault(x => x.Name == "Astraea");
            if (star == null)
            {
                star = new Star
                {
                    Name = "Astraea",
                    Atk = 12,
                    HpMax = 250,
                    Mod = 4,
                    Rarity = 3,
                    Speed = 90,
                    Desc = "Astraea possui uma beleza serena e quase intocável. Seus cabelos negros longos e lisos emolduram um rosto pálido com olhos escuros que parecem conter o vazio do espaço. Ela veste uma versão moderna de um uniforme escolar japonês mesclado com elementos de alta costura: uma saia plissada preta com detalhes em fios de ouro, meias altas e um sobretudo longo com forro em degradê de azul-noite para dourado. Em combate, ela segura um rosário de âmbar que emite um brilho suave toda vez que ela sussurra uma prece.\r\n[Habilidade: Prece Estelar] Astraea roga às estrelas para curar a si mesma ou a seu aliado. Ao sacrificar a própria vida para salvar o companheiro, o excesso de luz transborda como escudo. Em nível máximo, entra em Plenitude e torna-se imune à morte temporariamente.\r\n[Passiva: Graça & Fortalecimento Estelar] Ao atacar, cura o aliado mais ferido ou a si mesma. Suas preces constantes canalizam a Graça Estelar, concedendo bônus massivo para a próxima cura após acumular sacrifícios, enquanto concede constantemente Fortalecimento Estelar para aumentar o dano do seu aliado a cada turno.",
                    SummonQuote = "Sob minha luz, ninguém cairá."
                };
                context.Personagens.Add(star);
            }
            else
            {
                star.Desc = "Astraea possui uma beleza serena e quase intocável. Seus cabelos negros longos e lisos emolduram um rosto pálido com olhos escuros que parecem conter o vazio do espaço. Ela veste uma versão moderna de um uniforme escolar japonês mesclado com elementos de alta costura: uma saia plissada preta com detalhes em fios de ouro, meias altas e um sobretudo longo com forro em degradê de azul-noite para dourado. Em combate, ela segura um rosário de âmbar que emite um brilho suave toda vez que ela sussurra uma prece.\r\n[Habilidade: Prece Estelar] Astraea roga às estrelas para curar a si mesma ou a seu aliado. Ao sacrificar a própria vida para salvar o companheiro, o excesso de luz transborda como escudo. Em nível máximo, entra em Plenitude e torna-se imune à morte temporariamente.\r\n[Passiva: Graça & Fortalecimento Estelar] Ao atacar, cura o aliado mais ferido ou a si mesma. Suas preces constantes canalizam a Graça Estelar, concedendo bônus massivo para a próxima cura após acumular sacrifícios, enquanto concede constantemente Fortalecimento Estelar para aumentar o dano do seu aliado a cada turno.";
                star.Atk = 12;
                star.HpMax = 250;
                star.Mod = 4;
                star.Speed = 90;
                star.SummonQuote = "Sob minha luz, ninguém cairá.";
            }
            var barbara = context.Personagens.FirstOrDefault(x => x.Name == "Shiro");
            if (barbara == null)
            {
                barbara = new Barbaro
                {
                    Name = "Shiro",
                    Atk = 10,
                    HpMax = 200,
                    Mod = 3,
                    Rarity = 3,
                    Speed = 85,
                    Desc = "Shiro é uma mulher esbelta e serena, envolta em um longo vestido branco de babados que combina com sua pele e cabelos alvos. Seus olhos amarelos transmitem uma calma profunda e, com um sorriso gentil, ela busca resolver tudo pelo diálogo, sem jamais notar a força devastadora que carrega.\r\n[Habilidade: Diálogo Pacificador] Shiro se coloca na frente dos inimigos para tentar conversar, atraindo a atenção do combate para si (90% de chance de ser focada) enquanto consome sua energia/fúria acumulada para se curar.\r\n[Passiva: Força Desconhecida] À medida que sofre dano, sua raiva/fúria oculta se acumula passivamente, convertendo-se em defesa contra golpes inimigos e aumentando seu ataque quanto menor for sua vida. Em nível máximo, recusa-se a cair e sobrevive ao golpe fatal ativando fúria máxima.",
                    SummonQuote = "Não se preocupe, eu cuidarei para que nada interrompa nossa conversa."
                };
                context.Personagens.Add(barbara);
            }
            else
            {
                barbara.Desc = "Shiro é uma mulher esbelta e serena, envolta em um longo vestido branco de babados que combina com sua pele e cabelos alvos. Seus olhos amarelos transmitem uma calma profunda e, com um sorriso gentil, ela busca resolver tudo pelo diálogo, sem jamais notar a força devastadora que carrega.\r\n[Habilidade: Diálogo Pacificador] Shiro se coloca na frente dos inimigos para tentar conversar, atraindo a atenção do combate para si (90% de chance de ser focada) enquanto consome sua energia/fúria acumulada para se curar.\r\n[Passiva: Força Desconhecida] À medida que sofre dano, sua raiva/fúria oculta se acumula passivamente, convertendo-se em defesa contra golpes inimigos e aumentando seu ataque quanto menor for sua vida. Em nível máximo, recusa-se a cair e sobrevive ao golpe fatal ativando fúria máxima.";
                barbara.Atk = 10;
                barbara.HpMax = 200;
                barbara.Mod = 3;
                barbara.Speed = 85;
                barbara.SummonQuote = "Não se preocupe, eu cuidarei para que nada interrompa nossa conversa.";
            }


            var voodo = context.Personagens.FirstOrDefault(x => x.Name == "Lucien");
            if (voodo == null)
            {
                voodo = new Voodo
                {
                    Name = "Lucien",
                    Atk = 10,
                    Rarity = 3,
                    HpMax = 230,
                    Mod = 4,
                    Speed = 85,
                    Desc = "Lucien é um garoto de aura mórbida e expressão cansada, com olheiras profundas sob olhos violetas e cabelos negros bagunçados. Ele veste um conjunto gótico-urbano em roxo e preto, com um sobretudo francês e correntes, carregando um boneco de pano remendado que parece absorver toda a alegria ao seu redor. \r\n[Habilidade: Campo Putrefe] Lucien consome uma porção da própria vida para espalhar uma névoa que reduz o dano recebido por si e seu aliado. Ao ser aprimorado, pode oferecer metade de sua alma para reviver e igualar a vida do companheiro.\r\n[Passiva: Costura da Alma & Putrefação Absorvida] Ao atacar, amarra um fio invisível ao seu boneco para se curar. Enquanto a névoa do Campo Putrefe estiver ativa, o boneco absorve a podridão ao redor, restaurando continuamente sua vida a cada turno.",
                    SummonQuote = "Espero que isso seja mais divertido que os mortos."
                };
                context.Personagens.Add(voodo);
            }
            else
            {
                voodo.Name = "Lucien";
                voodo.Atk = 10;
                voodo.HpMax = 230;
                voodo.Mod = 4;
                voodo.Speed = 85;
                voodo.Desc = "Lucien é um garoto de aura mórbida e expressão cansada, com olheiras profundas sob olhos violetas e cabelos negros bagunçados. Ele veste um conjunto gótico-urbano em roxo e preto, com um sobretudo francês e correntes, carregando um boneco de pano remendado que parece absorver toda a alegria ao seu redor. \r\n[Habilidade: Campo Putrefe] Lucien consome uma porção da própria vida para espalhar uma névoa que reduz o dano recebido por si e seu aliado. Ao ser aprimorado, pode oferecer metade de sua alma para reviver e igualar a vida do companheiro.\r\n[Passiva: Costura da Alma & Putrefação Absorvida] Ao atacar, amarra um fio invisível ao seu boneco para se curar. Enquanto a névoa do Campo Putrefe estiver ativa, o boneco absorve a podridão ao redor, restaurando continuamente sua vida a cada turno.";
                voodo.SummonQuote = "Espero que isso seja mais divertido que os mortos.";
            }

            // ============ NOVOS PERSONAGENS ============

            var scribe = context.Personagens.FirstOrDefault(x => x.Name == "Rimla");
            if (scribe == null)
            {
                scribe = new Scribe
                {
                    Name = "Rimla",
                    Desc = $"Rimla é a personificação da elegância gélida, unindo o requinte da alta alfaiataria com a precisão letal da tecnologia criogênica. Uma mulher pálida com um longo cabelo branco e liso, olhos azuis safira e longos cílios, trajando um terno impecável de corte futurista com detalhes tecnológicos discretos e luvas num tom azul-escuro profundo, ela desfila pelo campo de batalha com uma compostura inabalável. Ao seu redor, agulhas prateadas flutuam sob seu controle mental, puxando fios de geada luminosos capazes de costurar o próprio fluxo do tempo. Embora não ostente uma força bruta avassaladora, sua grife é renomeada e seu estilo de combate é cirúrgico: ela entrelaça o tempo e a matéria com a graciosidade de quem produz uma obra-prima de luxo.{Environment.NewLine}[Habilidade: Arremate Glacial] Rimla consome suas runas acumuladas para costurar o alvo ao peso de um inverno eterno, causando {250}% do seu ataque em dano. A habilidade prende o inimigo em um tear temporal que reduz sua velocidade e explode ao final da duração, aplicando a fragilidade 「Laçada Gélida」.{Environment.NewLine}[Passiva: Ponto Sem Nó & Tormento Gélido] Ao atacar, Rimla puxa sua linha congelante e tece até 3 runas de geada. Sua mera presença evoca um frio glacial que afeta os adversários, causando dano contínuo baseado nas suas runas e aplicando 「Laçada Gélida Menor」, aumentando o dano recebido pelo inimigo em {30}%.",
                    HpMax = 220,
                    Rarity = 3,
                    Atk = 10,
                    Mod = 5,
                    Speed = 100,
                    SummonQuote = "Os fios do tempo precisam ser alinhados.",
                };
                context.Personagens.Add(scribe);
            }
            else
            {
                scribe.HpMax = 220;
                scribe.Rarity = 3;
                scribe.Atk = 10;
                scribe.Mod = 5;
                scribe.Speed = 100;
                scribe.SummonQuote = "Os fios do tempo precisam ser alinhados.";
            }

            var fire = context.Personagens.FirstOrDefault(x => x.Name == "Logi");
            if (fire == null)
            {
                fire = new Fire
                {
                    Name = "Logi",
                    Desc = $"Logi é o jovem príncipe do reino de fogo de Muspelheim, unindo a realeza nórdica ao carisma magnético de um verdadeiro showman. Com pele parda, cabelos ruivos intensos e olhos amarelos dourados, ele desfila com um sorriso radiante e uma postura teatral inesquecível. Suas vestes combinam uma leve armadura de obsidiana com detalhes escarlates fluidos, complementada por uma espada de esgrima feita de magma ardente. Transformando cada batalha em um grande espetáculo pirotécnico, ele manipula o calor com elegância e precisão artística.{Environment.NewLine}[Habilidade: Labareda Catártica / Forma Flamejante] Logi consome seu Clímax Ígneo para desferir um grand finale devastador com sua espada de esgrima, ou assume sua Forma Flamejante para incendiar o campo de batalha, aplicando queimaduras implacáveis e fortalecendo seus aliados com um fervor contagiante.{Environment.NewLine}[Passiva: Aquecimento] A energia de Logi cresce conforme o show avança, acumulando carga térmica que aumenta seu poder de ataque e aquece a atmosfera ao seu redor com uma presença magnética e abrasadora.",
                    HpMax = 180,
                    Rarity = 4,
                    Atk = 10,
                    Mod = 5,
                    Speed = 85,
                    SummonQuote = "Haverá choro e ranger de dentes, mas no fim, eu estarei BRILHANDO!",
                };
                context.Personagens.Add(fire);
            }
            else
            {
                fire.HpMax = 180;
                fire.Rarity = 4;
                fire.Atk = 10;
                fire.Mod = 5;
                fire.Speed = 85;
                fire.SummonQuote = "Haverá choro e ranger de dentes, mas no fim, eu estarei BRILHANDO!";
            }

            // ============ UPDATE ITEM ============


            // ============ NOVOS ITENS ============
            var presa = context.Itens.FirstOrDefault(x => x.Name == "Presa de Fernir");
            if (presa == null)
            {
                presa = new presaFernir
                {
                    Name = "Presa de Fernir",
                    Desc = "Um colar ornado por uma presa afiada de vinte e cinco centímetros, cuja base traz incensadas runas douradas que emanam um brilho suave e o poder ancestral da fera divina mãe. O amuleto vibra com o eco distante de uma caçada celestial, despertando um instinto predatório avassalador em quem o carrega. Efeito: Recebe um aumento de velocidade equivalente à vida perdida do inimigo, alcançando até +25 de Velocidade Total no seu auge. (+8 de ATK)",
                    Atr = 2,
                    Type = 2,
                    Mod = 8,
                    Rarity = 3
                };
                context.Itens.Add(presa);
                context.SaveChanges();
            }
            else
            {
                presa.Name = "Presa de Fernir";
                presa.Desc = "Um colar ornado por uma presa afiada de vinte e cinco centímetros, cuja base traz incensadas runas douradas que emanam um brilho suave e o poder ancestral da fera divina mãe. O amuleto vibra com o eco distante de uma caçada celestial, despertando um instinto predatório avassalador em quem o carrega. Efeito: Recebe um aumento de velocidade equivalente à vida perdida do inimigo, alcançando até +25 de Velocidade Total no seu auge. (+8 de ATK)";
                presa.Atr = 2;
                presa.Type = 2;
                presa.Mod = 8;
                presa.Rarity = 3;
            }


            // ============ UPDATE INIMIGOS ============


            // ============ NOVOS INIMIGOS ============
            var fernir = context.Inimigos.FirstOrDefault(x => x.Name == "Fernir Filhote");
            if (fernir == null)
            {
                fernir = new FenrirF
                {
                    Name = "Fernir Filhote",
                    Desc = "Uma silhueta esguia e imponente, moldada pela própria noite e erguida sobre uma rocha ancestral que domina o fiorde. A pelagem negra, densa e volumosa, é um firmamento vivo onde runas douradas de poder divino pulsam e queimam, como constelações traçando o destino do mundo e a linhagem de Loki. Seus olhos são fendas de puro fogo escarlate, um aviso ardente da fúria ancestral que dorme sob a pele e que, um dia, devorará o próprio sol. Ele não busca a companhia dos mortais, mas reina como o guardião solitário e divino das passagens entre os mundos, aguardando o ranger das correntes de Gleipnir sob o brilho da aurora boreal.",
                    HpMax = 1560,
                    Atk = 25,
                    Speed = 100,
                    HabilidadeChance = 65,
                    CrystalDrop = 3,
                    Mod = 10,
                    ItemDropId = presa.Id
                };
                context.Inimigos.Add(fernir);
            }
            else
            {
                fernir.Name = "Fernir Filhote";
                fernir.Desc = "Uma silhueta esguia e imponente, moldada pela própria noite e erguida sobre uma rocha ancestral que domina o fiorde. A pelagem negra, densa e volumosa, é um firmamento vivo onde runas douradas de poder divino pulsam e queimam, como constelações traçando o destino do mundo e a linhagem de Loki. Seus olhos são fendas de puro fogo escarlate, um aviso ardente da fúria ancestral que dorme sob a pele e que, um dia, devorará o próprio sol. Ele não busca a companhia dos mortais, mas reina como o guardião solitário e divino das passagens entre os mundos, aguardando o ranger das correntes de Gleipnir sob o brilho da aurora boreal.";
                fernir.HpMax = 1560;
                fernir.Atk = 25;
                fernir.Speed = 100;
                fernir.HabilidadeChance = 65;
                fernir.CrystalDrop = 3;
                fernir.Mod = 10;
                fernir.ItemDropId = presa.Id;
            }

            context.SaveChanges();
        }
    }
}