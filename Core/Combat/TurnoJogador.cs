using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Gacha.Data;
using Todo_Gacha.Services;

namespace Todo_Gacha.Core.Combat
{
    public class TurnoJogador
    {

        public void turno(CombateUI combateUI, List<PersonagemBase> equipe, PersonagemBase personagem, InimigoBase inimigo, int x, InventarioServices inventario, AppDbContext context)
        {
            if (personagem.TurnoStun > 0)
            {
                combateUI.ExibirMensagem($"{personagem.Name} está atordoado e não pode agir!", ConsoleColor.Red);
                Console.ReadKey();
                personagem.TurnoStun--;
            }

            if (inimigo.HpAtual <= 0)
            {
                return;
            }
            if (personagem.HpAtual <= 0)
            {
                return;
            }
            combateUI.Cabecalho(equipe, inimigo, x);
            personagem.Passiva();
            Console.ReadKey();
            //Turno do Player
            bool Atacou = false;
            bool UsouHabilidade = false;
            bool UsouItem = false;
            bool MenuShow = true;
            Console.ForegroundColor = ConsoleColor.White;



            while (MenuShow)
            {
                combateUI.Cabecalho(equipe, inimigo, x);
                combateUI.ExibirAcoes(personagem, Atacou, UsouHabilidade, UsouItem);
                switch (combateUI.EscolhaJogador(1, 4))
                {
                    case 1:
                        if (!Atacou)
                        {
                            int dano = personagem.Damage();
                            inimigo.tomarDano(personagem.Name, dano);
                            Atacou = true;
                        }
                        Console.ReadKey();
                        break;
                    case 2:
                        if (personagem.TurnoSilence > 0)
                        {
                            combateUI.ExibirMensagem($"{personagem.Name} está silenciado e não pode usar habilidades!", ConsoleColor.Red);
                            Console.ReadKey();
                        }
                        else if (!UsouHabilidade)
                        {
                            personagem.Habilidade();
                            UsouHabilidade = true;
                        }
                        Console.ReadKey();
                        break;
                    case 3:
                        escolherItem(UsouItem, combateUI, equipe, personagem, inimigo, x, inventario, context);

                        break;
                    case 4:
                        combateUI.ExibirMensagem($"Fim do turno.", ConsoleColor.White);
                        Console.ReadKey();
                        MenuShow = false;
                        break;

                }
                
            }
            personagem.TurnoSilence--;
        }

        public void escolherItem(bool UsouItem, CombateUI combateUI, List<PersonagemBase> equipe, PersonagemBase personagem, InimigoBase inimigo, int x, InventarioServices inventario, AppDbContext context)
        {
            combateUI.Cabecalho(equipe, inimigo, x);
            var consumiveis = new List<Item>();
            int contador = 1;
            inventario.MostrarItens(context, true, consumiveis, 1, ref contador);
            if (consumiveis.Count == 0)
            {
                combateUI.ExibirMensagem($"Você não possui itens utilizáveis.", ConsoleColor.Red);
                return;
            }
            combateUI.ExibirMensagem("--", ConsoleColor.White);
            combateUI.ExibirMensagem($"Qual item deseja usar?", ConsoleColor.White);

            var num = combateUI.EscolhaJogador(1, consumiveis.Count());
            var itemEscolhido = consumiveis[num - 1];
            combateUI.ExibirMensagem($"{personagem.Name} usou {itemEscolhido.Name}!", ConsoleColor.White);
            itemEscolhido.Uso(personagem, context);
            UsouItem = true;
            Console.ReadKey();
        }
    }
}