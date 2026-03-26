#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo_Gacha.Data;
using Todo_Gacha.Core;
using Todo_Gacha.Core.Combat;
using Todo_Gacha.Services;
using System.Diagnostics;
using Todo_Gacha.Models;

namespace Todo_Gacha.Services
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