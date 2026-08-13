using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_U.Data;
using Task_U.Core;
using Task_U.Services;
using Task_U.Core.Entities;
using Task_U.Core.Enemies;
using Task_U.Core.Itens;

namespace Task_U.Services
{
    public class CreateService
    {
        public void CreateCharacter(AppDbContext context)
        {
            var Scribe = new Scribe
            {
                Name = "Rimla",
                Desc = $"Rimla é a personificação da elegância gélida, unindo o requinte da alta alfaiataria com a precisão letal da tecnologia criogênica. Uma mulher palida com um longo cabelo branco e liso, olhos azuis safira e longos cílios, trajando um terno impecável de corte futurista com detalhes tecnológicos discretos e luvas num tom azul-escuro profundo, ela desfila pelo campo de batalha com uma compostura inabalável. Ao seu redor, agulhas prateadas flutuam sob seu controle mental, puxando fios de geada luminosos capazes de costurar o próprio fluxo do tempo. Embora não ostente uma força bruta avassaladora, sua grife é renomeada e seu estilo de combate é cirúrgico: ela entrelaça o tempo e a matéria com a graciosidade de quem produz uma obra-prima de luxo.{Environment.NewLine}[Habilidade: Arremate Glacial] Rimla consome suas runas acumuladas para costurar o alvo ao peso de um inverno eterno, causando {250}% do seu ataque em dano. A habilidade prende o inimigo em um tear temporal que reduz sua velocidade e explode ao final da duração, aplicando a fragilidade 「Laçada Gélida」.{Environment.NewLine}[Passiva: Ponto Sem Nó & Tormento Gélido] Ao atacar, Rimla puxa sua linha congelante e tece até 3 runas de geada. Sua mera presença evoca um frio glacial que afeta os adversários, causando dano contínuo baseado nas suas runas e aplicando 「Laçada Gélida Menor」, aumentando o dano recebido pelo inimigo em {30}%.",
                HpMax = 220,
                Rarity = 3,
                Atk = 10,
                Mod = 5,
                Speed = 100,
                SummonQuote = "Os fios do tempo precisam ser alinhados.",
            };
            var Fire = new Fire
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
            context.Personagens.Add(Scribe);
            context.Personagens.Add(Fire);
            context.SaveChanges();

        }
    }
}