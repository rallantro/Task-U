#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Task_U.Data;
using Task_U.Core;
using Task_U.Services;

namespace Task_U.Models
{
    public class Gacha
    {
        public Gacha()
        {
        }

        Random random = new Random();

        public int legChance = 10;

        public int maxPityLeg = 100;
        public int maxPityEpic = 10;

        public bool luckEvent = false;

        public void luckyEvent()
        {
            luckEvent = true;
        }

        public void Pull(BannerService banner)
        {
            using var context = new AppDbContext();
            int number = random.Next(1, 1001);
            var user = context.Users.Find(1);
            if (user == null)
            {
                throw new Exception("Erro crítico! O usuário não foi encontrado, seu banco de dados pode estar corrompido, por favor procure contatar o suporte.");
            }

            user.Crystals -= 10;

            int pityLeg = user.PityLeg;
            int pityEpic = user.PityEpic;

            pityLeg++;
            pityEpic++;

            int currentChance = (pityLeg >= 75) ? legChance + (20 * (pityLeg - 74)) : 10;

            if (luckEvent)
            {
                currentChance *= 2;
                luckEvent = false;
            }


            Console.WriteLine("...");
            Thread.Sleep(1500);

            Console.ForegroundColor = ConsoleColor.White;
            string[] suspenseFrames = {
                    "[ . ]",
                    "[ . . ]",
                    "[ . . . ]",
                    "«  . O .  »",
                    "« « . O . » »"
                };
            foreach (var frame in suspenseFrames)
            {
                Console.Clear();
                Console.WriteLine("\n\n\n");
                Console.WriteLine(new string(' ', (60 - frame.Length) / 2) + frame);
                Thread.Sleep(300);
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Thread.Sleep(100);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            if (number <= currentChance || pityLeg == maxPityLeg)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                string[] frames = { "───", "─── ✧ ───", "─── ✧ ✦ ✧ ───", "─── ✧ ✦ ❂ ✦ ✧ ───" };
                foreach (var frame in frames)
                {
                    Console.Clear();
                    Console.WriteLine("\n\n\n");
                    Console.WriteLine(new string(' ', (60 - frame.Length) / 2) + frame);
                    Thread.Sleep(400);
                }
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Clear();
                Thread.Sleep(100);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
                string t1 = "!!! PULL LENDÁRIO !!!";
                string t2 = "S S R";
                Console.WriteLine("║" + new string(' ', (58 - t1.Length) / 2) + t1 + new string(' ', 58 - ((58 - t1.Length) / 2) - t1.Length) + "║");
                Console.WriteLine("║" + new string(' ', (58 - t2.Length) / 2) + t2 + new string(' ', 58 - ((58 - t2.Length) / 2) - t2.Length) + "║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
                Thread.Sleep(1000);
                ganharPersonagem(user, context, banner, 4);
                pityLeg = 0;
                pityEpic = 0;
            }
            else if (number <= 60 || pityEpic == maxPityEpic)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                string[] frames = { "───", "─── ✧ ───", "─── ✧ ✦ ✧ ───", "─── ✧ ✦ ✦ ✧ ───" };
                foreach (var frame in frames)
                {
                    Console.Clear();
                    Console.WriteLine("\n\n\n");
                    Console.WriteLine(new string(' ', (60 - frame.Length) / 2) + frame);
                    Thread.Sleep(400);
                }
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.Clear();
                Thread.Sleep(100);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
                string t1 = "! PULL ÉPICO !";
                string t2 = "S R";
                Console.WriteLine("║" + new string(' ', (58 - t1.Length) / 2) + t1 + new string(' ', 58 - ((58 - t1.Length) / 2) - t1.Length) + "║");
                Console.WriteLine("║" + new string(' ', (58 - t2.Length) / 2) + t2 + new string(' ', 58 - ((58 - t2.Length) / 2) - t2.Length) + "║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
                Thread.Sleep(1000);

                ganharPersonagem(user, context, banner, 3);
                pityEpic = 0;
            }
            else if (number <= 250)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Cyan;
                string[] framesRaro = { "───", "─── ✧ ───", "─── ✧ ✦ ✧ ───" };
                foreach (var frame in framesRaro)
                {
                    Console.Clear();
                    Console.WriteLine("\n\n\n");
                    Console.WriteLine(new string(' ', (60 - frame.Length) / 2) + frame);
                    Thread.Sleep(200);
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
                string r1 = "PULL RARO";
                string r2 = "[ RANK R ]";
                Console.WriteLine("║" + new string(' ', (58 - r1.Length) / 2) + r1 + new string(' ', 58 - ((58 - r1.Length) / 2) - r1.Length) + "║");
                Console.WriteLine("║" + new string(' ', (58 - r2.Length) / 2) + r2 + new string(' ', 58 - ((58 - r2.Length) / 2) - r2.Length) + "║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════╝");

                var ganhou = new ItemInventario();
                var reward = banner.raroPull(context);
                ganhou.ItemId = reward.Id;
                ganhou.UserId = 1;
                context.InventarioItens.Add(ganhou);

                Console.WriteLine("\n");
                string nomeItem = $">> {reward.Name.ToUpper()} <<";
                Console.WriteLine(new string(' ', (60 - nomeItem.Length) / 2) + nomeItem);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                string desc = reward.Desc;
                int larguraMaxima = 56;

                if (desc.Length > larguraMaxima)
                {
                    Console.WriteLine("  " + desc);
                }
                else
                {
                    int pad = (60 - desc.Length) / 2;
                    Console.WriteLine(new string(' ', pad) + desc);
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                Console.WriteLine("\n\n\n");
                Console.WriteLine(new string(' ', (60 - 15) / 2) + "--- COMUM ---");
                Thread.Sleep(500);

                var ganhou = new ItemInventario();
                var reward = banner.commumPull(context);
                ganhou.ItemId = reward.Id;
                ganhou.UserId = 1;
                context.InventarioItens.Add(ganhou);

                Console.WriteLine("\n");
                Console.WriteLine(new string(' ', (60 - reward.Name.Length - 4) / 2) + $"[ {reward.Name} ]");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                string desc = reward.Desc;
                int larguraMaxima = 56;

                if (desc.Length > larguraMaxima)
                {
                    Console.WriteLine("  " + desc);
                }
                else
                {
                    int pad = (60 - desc.Length) / 2;
                    Console.WriteLine(new string(' ', pad) + desc);
                }

            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" ─────────────────── PROGRESSO DE GARANTIA ───────────────────");

            Console.ForegroundColor = ConsoleColor.Magenta;
            string progressoEpic = $"[SR] Garantido em: {10 - pityEpic} giros";
            Console.WriteLine("  " + progressoEpic.PadRight(30));

            Console.ForegroundColor = ConsoleColor.Yellow;
            string progressoLeg = $"[SSR] Garantido em: {100 - pityLeg} giros";
            Console.WriteLine("  " + progressoLeg.PadRight(30));

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" ─────────────────────────────────────────────────────────────");
            Console.ResetColor();

            user.PityEpic = pityEpic;
            user.PityLeg = pityLeg;
            context.Users.Update(user);

            context.SaveChanges();
            Console.ReadLine();
        }

        private void ganharPersonagem(User user, AppDbContext context, BannerService banner, int rarity)
        {
            var ganhou = new PersonagemInventario();
            var reward = rarity == 3 ? banner.EpicPull(context) : banner.LegendPull(context);
            PersonagemInventario? old = context.InventarioPersonagens.FirstOrDefault(p => p.UserId == user.Id && p.PersonagemId == reward.Id);
            if (old == null)
            {
                ganhou.PersonagemId = reward.Id;
                ganhou.UserId = 1;
                ganhou.Quantidade = 1;
                context.InventarioPersonagens.Add(ganhou);
                if (!string.IsNullOrEmpty(reward.SummonQuote))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n\n\n");
                    string quote = $"\"{reward.SummonQuote}\"";
                    int paddingQuote = (60 - quote.Length) / 2;

                    if (paddingQuote > 0)
                    {
                        Console.WriteLine(new string(' ', paddingQuote) + quote);
                    }
                    else
                    {
                        Console.WriteLine("  " + quote);
                    }

                    Thread.Sleep(2000);
                    Console.Clear();
                }

                Console.ForegroundColor = reward.Rarity == 3 ? ConsoleColor.Magenta : ConsoleColor.Yellow;
                Console.WriteLine("\n");
                string nomeCentralizado = $"-- {reward.Name.ToUpper()} --";
                Console.WriteLine(new string(' ', (60 - nomeCentralizado.Length) / 2) + nomeCentralizado);
                string descricao = $"  \"{reward.Desc}\"";
                Console.ForegroundColor = ConsoleColor.White;
                EscreverComGlitch(descricao);
                Console.WriteLine();
            }
            else if (old.Quantidade < 6)
            {
                old.Quantidade++;
                Console.Clear();
                Console.ForegroundColor = reward.Rarity == 3 ? ConsoleColor.Magenta : ConsoleColor.Yellow;
                Console.WriteLine("\n\n");
                Console.WriteLine("  [>>] NODE EM PROCESSO DE LIBERAÇÃO!");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  ─────────────────────────────────────────────────────────────");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"  Sigilo de <{reward.Name.ToUpper()}> detectada no banco.");
                Console.WriteLine("  Iniciando protocolo de Recompilação de Núcleo...");
                Thread.Sleep(800);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n  >> STATUS: NODE {old.Quantidade - 1} DESBLOQUEADO."); // Mostra o nível da constelação
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  Poder do Sigilo otimizado com novos pacotes de dados.");
                Console.WriteLine("  ─────────────────────────────────────────────────────────────");
                Console.ResetColor();
                Thread.Sleep(1500);
            }
            else
            {
                int bitsGanhos = reward.Rarity == 3 ? 25 : 200;
                user.Bits += bitsGanhos;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n");
                Console.WriteLine("  [!] SIGILO DUPLICADO DETECTADO");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  ─────────────────────────────────────────────────────────────");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"  O Módulo <{reward.Name.ToUpper()}> já atingiu a estabilidade máxima.");
                Console.WriteLine($"  Iniciando protocolo de decomposição de excesso...");
                Thread.Sleep(800);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n  >> CONVERSÃO CONCLUÍDA: +" + bitsGanhos + " BITS ADICIONADOS.");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  ─────────────────────────────────────────────────────────────");
                Console.ResetColor();
                Thread.Sleep(1500);

            }
            Console.ResetColor();
        }
        private void EscreverComGlitch(string texto)
        {
            string caracteres = "$#@&%█▓▒░";
            Random rand = new Random();

            char[] glitchFrame = texto.Select(c => c == ' ' ? ' ' : caracteres[rand.Next(caracteres.Length)]).ToArray();
            string frameStr = new string(glitchFrame);


            Console.Write("\r" + frameStr);
            Thread.Sleep(40);

            Console.Write("\r" + texto);
            Console.WriteLine();
        }

    }

}