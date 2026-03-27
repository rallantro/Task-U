#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task_U.Data;
using Task_U.Core;
using Task_U.Core.Combat;
using Task_U.Services;
using System.Diagnostics;
using Task_U.Models;

namespace Task_U.Services
{
    public class CombatService
    {
        public void Combate(InimigoBase inimigo, List<PersonagemBase> equipe, AppDbContext context, AdventureService adventure)
        {
            var user = context.Users.Find(1);
            var inventario = new InventarioServices();
            CombateEngine combate = new CombateEngine();
            combate.Combate(user, inventario, inimigo, equipe, context, adventure);
        }
    } 
}