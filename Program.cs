// See https://aka.ms/new-console-template for more information
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.VisualBasic;
using Todo_Gacha.Models;
using Todo_Gacha.Core;
using Todo_Gacha.Services;
using Todo_Gacha.Data;
using System.Diagnostics;
using Todo_Gacha.Core.Entities;

Gacha gacha = new Gacha();

TarefaService service = new TarefaService();
using var context = new AppDbContext();



service.AtualizarTarefas();

bool MenuShow = true;

while (MenuShow)
{
    Console.Clear();
    Console.WriteLine("===============================");
    Console.WriteLine("MENU PRINCIPAL");
    Console.WriteLine("===============================");
    Console.WriteLine(" ");
    Console.WriteLine("Digite a sua opção:");
    Console.WriteLine("1 - Ver Status");
    Console.WriteLine("2 - Ver Tarefas");
    Console.WriteLine("3 - Concluir Tarefas");
    Console.WriteLine("4 - Desejar");
    Console.WriteLine("5 - Encerrar");


    switch (Console.ReadLine())
    {
        case "1" :
            service.verStatus();
            Console.ReadLine();
        break;

        case "2":
            service.verTarefas();
            Console.ReadLine();
        break;

        case "3":
            Console.WriteLine("--");
            Console.WriteLine("Digite o ID da Tarefa Realizada:");
            service.ConcluirTarefa(int.Parse(Console.ReadLine()), gacha);
            Console.ReadLine();
        break;

        case "4":
            gacha.Pull();
            Console.ReadLine();
        break;

        case "5":
            Environment.Exit(0);
        break;

        default:
            Console.WriteLine("Por favor digite apenas uma das opções acima!");
            Console.ReadLine();
        break;
    }
}




