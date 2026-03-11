// See https://aka.ms/new-console-template for more information
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.VisualBasic;
using Todo_Gacha.Models;
using Todo_Gacha.Data;
using System.Diagnostics;

Gacha gacha = new Gacha();

TarefaService service = new TarefaService();

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
    Console.WriteLine("3 - Pull");
    Console.WriteLine("4 - Encerrar");


    switch (Console.ReadLine())
    {
        case "1" :
            
        break;

        case "2":
            service.verTarefas();
            Console.ReadLine();
        break;

        default:

        break;
    }
}




