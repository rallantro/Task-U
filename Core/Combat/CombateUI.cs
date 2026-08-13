using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel.Resolution;
using Task_U.Data;

namespace Task_U.Core.Combat
{
    public class CombateUI
    {
        public void Cabecalho(List<PersonagemBase> equipe, InimigoBase inimigo, int turno, string Atual)
        {
            Console.Clear();

            var simulacaoFila = new List<string>();
            var clones = equipe.Where(x => x.HpAtual > 0)
                               .Select(p => new { Nome = p.Name, AvSimulado = p.AvAtual, AvReset = 10000.0 / p.SpeedTotal() })
                               .Concat(new[] { new { Nome = inimigo.Name, AvSimulado = inimigo.AvAtual, AvReset = 10000.0 / inimigo.SpeedTotal() } })
                               .ToList();

            for (int i = 0; i < 6; i++)
            {
                var proximo = clones.OrderBy(c => c.AvSimulado).First();
                simulacaoFila.Add(proximo.Nome);
                double avAvancado = proximo.AvSimulado;
                clones = clones.Select(c => new
                {
                    Nome = c.Nome,
                    AvSimulado = (c.Nome == proximo.Nome) ? c.AvReset : (c.AvSimulado - avAvancado),
                    AvReset = c.AvReset
                }).ToList();
            }

            int larguraStatus = 40;   
            int larguraPista = 38;    
            int bordaLateral = 2;     
            int separadorCentral = 3; 
            int larguraTotal = larguraStatus + separadorCentral + larguraPista + bordaLateral; // 40+3+38+2 = 83
            string titulo = $"[ RODADA GLOBAL: {turno:D3} ]";
            int espacosTitulo = (larguraTotal - titulo.Length) / 2;
            Console.Write("╔" + new string('═', espacosTitulo));
            Console.Write(titulo);
            Console.WriteLine(new string('═', larguraTotal - espacosTitulo - titulo.Length - 2) + "╗");
            string cabecalhoStatus = " STATUS DA EQUIPE".PadRight(larguraStatus);
            string cabecalhoAcao = "     ORDEM DE PRÓXIMA AÇÃO".PadRight(larguraPista);
            Console.WriteLine($"║{cabecalhoStatus}│{cabecalhoAcao}║");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("╟" + new string('─', larguraStatus) + "┼" + new string('─', larguraPista) + "╢");
            Console.ResetColor();
            foreach (var p in equipe)
            {
                string nome = p.Name.PadRight(12);
                string hp = $"HP: {p.HpAtual,3}/{p.HpMax,3}".PadRight(13);
                string shield = p.Shield > 0 ? $"(Shd:{p.Shield,2})" : "          ";
                string statusLinha = $" {nome} │ {hp} {shield}".PadRight(larguraStatus); // agora com largura fixa
                string acaoLinha;
                if (p.HpAtual <= 0)
                {
                    acaoLinha = "[ X - MORTO - X ]".PadRight(larguraPista);
                }
                else
                {
                    if (p.Name == Atual)
                        acaoLinha = "[ ▶ AGINDO AGORA ]".PadRight(larguraPista);
                    else if (simulacaoFila.Count > 0 && simulacaoFila[0] == p.Name)
                    {
                        acaoLinha = "[ PRÓXIMO A AGIR! ]".PadRight(larguraPista);
                    }
                    else
                    {
                        var turnos = simulacaoFila.Select((nome, idx) => new { nome, idx = idx + 1 })
                                                  .Where(x => x.nome == p.Name)
                                                  .Select(x => x.idx)
                                                  .ToList();
                        if (turnos.Count > 1)
                            acaoLinha = $"[ {turnos[0]}º e {turnos[1]}º a agir ]".PadRight(larguraPista);
                        else if (turnos.Count == 1)
                            acaoLinha = $"[ {turnos[0]}º a agir ]".PadRight(larguraPista);
                        else
                            acaoLinha = "[ +6 turnos longe ]".PadRight(larguraPista);
                    }
                }
                Console.Write("║");
                if (p.HpAtual <= p.HpMax / 5) Console.ForegroundColor = ConsoleColor.Red;
                else if (p.HpAtual <= 0) Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(statusLinha);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("│");

                Console.ForegroundColor = (p.HpAtual > 0) ? ConsoleColor.Cyan : ConsoleColor.DarkRed;
                Console.Write(acaoLinha);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("║");
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("╟" + new string('─', larguraStatus) + "┼" + new string('─', larguraPista) + "╢");
            Console.ResetColor();
            string inimNome = inimigo.Name.ToUpper().PadRight(12);
            string inimHp = $"HP: {inimigo.HpAtual,4}/{inimigo.HpMax,4}".PadRight(13);
            string inimShield = inimigo.Shield > 0 ? $"(Shd:{inimigo.Shield,2})" : "          ";
            string inimStatus = $" {inimNome} │ {inimHp} {inimShield}".PadRight(larguraStatus);

            string inimAcao;
            var inimTurnos = simulacaoFila.Select((nome, idx) => new { nome, idx = idx + 1 })
                                          .Where(x => x.nome == inimigo.Name)
                                          .Select(x => x.idx)
                                          .ToList();
            bool inimigoProximo = simulacaoFila.Count > 0 && simulacaoFila[0] == inimigo.Name;
            if (inimigo.Name == Atual)
                inimAcao = "[ ▶ AGINDO AGORA ]".PadRight(larguraPista);
            else if (inimigoProximo)
                inimAcao = "[ PRÓXIMO A AGIR! ]".PadRight(larguraPista);

            else if (inimTurnos.Count > 1)
                inimAcao = $"[ {inimTurnos[0]}º e {inimTurnos[1]}º a agir ]".PadRight(larguraPista);
            else if (inimTurnos.Count == 1)
                inimAcao = $"[ {inimTurnos[0]}º a agir ]".PadRight(larguraPista);
            else
                inimAcao = "[ +6 turnos longe ]".PadRight(larguraPista);

            Console.Write("║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(inimStatus);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("│");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(inimAcao);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("║");
            Console.WriteLine("╚" + new string('═', larguraStatus) + "╧" + new string('═', larguraPista) + "╝");
            Console.ResetColor();
        }

        public void ExibirAcoes(PersonagemBase personagem, bool Atacou, bool UsouHabilidade, bool UsouItem)
        {
            Console.WriteLine($"\n ▶ AGINDO AGORA: {personagem.Name.ToUpper()}");
            Console.WriteLine("  ┌──────────────────────────────────────────┐");
            Console.WriteLine("  │ O que você deseja fazer?                 │");
            Console.WriteLine("  ├──────────────────────────────────────────┤");
            if (Atacou)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  │  [1] Ataque Básico                       │");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("  │  [1] Ataque Básico                       │");
            }
            if (UsouHabilidade)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  │  [2] Habilidade Especial                 │");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("  │  [2] Habilidade Especial                 │");
            }
            if (UsouItem)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  │  [3] Usar Item                           │");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("  │  [3] Usar Item                           │");
            }
            Console.WriteLine("  │  [4] Encerrar Turno                      │");
            Console.WriteLine("  └──────────────────────────────────────────┘");
            Console.Write("  >> Opção: ");
            Console.ResetColor();
        }


        public int EscolhaJogador(int minOpcao, int maxOpcao)
        {
            int escolha;
            while (true)
            {
                string? entrada = Console.ReadLine();
                if (int.TryParse(entrada, out escolha) && escolha >= minOpcao && escolha <= maxOpcao)
                    return escolha;
                Console.WriteLine($"Opção inválida. Digite um número entre {minOpcao} e {maxOpcao}.");
            }
        }

        public ConsoleKeyInfo Chamada(InimigoBase inimigo)
        {
            Console.Clear();
            int larguraCard = 70;
            string alerta = "⚠  ALERTA DE COMBATE  ⚠";
            Console.WriteLine(new string(' ', (larguraCard - alerta.Length) / 2) + alerta);
            if (inimigo.Rarity == 2) Console.ForegroundColor = ConsoleColor.Blue;
            else if (inimigo.Rarity == 3) Console.ForegroundColor = ConsoleColor.Magenta;
            else if (inimigo.Rarity == 4) Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔" + new string('═', larguraCard - 2) + "╗");
            string nomeInimigo = $" INIMIGO: {inimigo.Name.ToUpper()} ";
            int espacoNome = (larguraCard - 2 - nomeInimigo.Length) / 2;
            Console.WriteLine("║" + new string(' ', espacoNome) + nomeInimigo + new string(' ', larguraCard - 2 - espacoNome - nomeInimigo.Length) + "║");
            Console.WriteLine("╟" + new string('─', larguraCard - 2) + "╢");
            string[] palavras = inimigo.Desc.Split(' ');
            string linhaAtual = "║  ";

            foreach (var palavra in palavras)
            {
                if ((linhaAtual + palavra).Length > larguraCard - 4)
                {
                    Console.WriteLine(linhaAtual.PadRight(larguraCard - 1) + "║");
                    linhaAtual = "║  ";
                }
                linhaAtual += palavra + " ";
            }
            Console.WriteLine(linhaAtual.PadRight(larguraCard - 1) + "║");

            Console.WriteLine("╚" + new string('═', larguraCard - 2) + "╝");
            Console.ResetColor();
            Console.WriteLine("\n[ Deseja Continuar? ]");
            Console.WriteLine("\n[1 - Sim] [Qualquer outra tecla para retornar...]");
            return Console.ReadKey();
        }

        public void ExibirMensagem(string Mensagem, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(Mensagem);
            Console.ResetColor();
        }

        public void Vitoria(InimigoBase inimigo, Item? item)
        {
            Console.Clear();
            int larguraVitoria = 60;
            if (inimigo.DeathQuote != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\n  {inimigo.Name} balbucia suas últimas palavras:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"  \"{inimigo.DeathQuote}\"");
                Console.WriteLine(new string('─', larguraVitoria));
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            string vitoriaMsg = "VITÓRIA!!";
            Console.WriteLine("\n" + new string(' ', (larguraVitoria - vitoriaMsg.Length) / 2) + vitoriaMsg);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(new string(' ', (larguraVitoria - 30) / 2) + "==============================");
            if (inimigo.CrystalDrop > 0 || item != null)
            {
                int larguraTotal = 58;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("    RECOMPENSAS DO COMBATE:");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("  ┌──────────────────────────────────────────────────────────┐");
                if (inimigo.CrystalDrop > 0)
                {
                    string texto = $"• {inimigo.CrystalDrop} Cristais";
                    Console.WriteLine($"  │ {texto.PadRight(larguraTotal - inimigo.CrystalDrop.ToString().Length)}│");
                }

                if (item != null)
                {
                    string texto = $"• {item.Name} (Novo Item!)";
                    Console.WriteLine($"  │ {texto.PadRight(larguraTotal - texto.Length)} │");
                }
                Console.WriteLine("  └──────────────────────────────────────────────────────────┘");
            }
            Console.WriteLine("\n [ Pressione qualquer tecla para voltar ao menu... ]");
            Console.ResetColor();
            Console.ReadKey();
        }

        public void Derrota(InimigoBase inimigo)
        {
            Console.Clear();
            int larguraDerrota = 60;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(new string(' ', (larguraDerrota - 22) / 2) + "▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄");
            Console.ForegroundColor = ConsoleColor.Red;
            string msgDerrota = "█    VOCÊ CAIU...    █";
            Console.WriteLine(new string(' ', (larguraDerrota - msgDerrota.Length) / 2) + msgDerrota);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(new string(' ', (larguraDerrota - 22) / 2) + "▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            string contexto = $"{inimigo.Name} superou sua equipe desta vez.";
            Console.WriteLine(new string(' ', (larguraDerrota - contexto.Length) / 2) + contexto);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  DICAS DE COMBATE:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ┌──────────────────────────────────────────────────────────┐");
            Console.WriteLine("  │ • Verifique se seus itens equipados auxiliam sua equipe. │");
            Console.WriteLine("  │ • Alguns personagens têm vantagem contra esse inimigo.   │");
            Console.WriteLine("  │ • Use a Habilidade Especial no momento certo!            │");
            Console.WriteLine("  └──────────────────────────────────────────────────────────┘");
            Console.ReadKey();
        }

        public void TurnoInimigo(InimigoBase inimigo)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n ▶ AGINDO AGORA: {inimigo.Name.ToUpper()}");
            Console.ResetColor();
        }

        public void AguardarTecla() => Console.ReadKey();
    }
}