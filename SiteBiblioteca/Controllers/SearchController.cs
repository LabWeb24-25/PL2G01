using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SiteBiblioteca.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Pesquisar(string query)
        {
            var results = new List<string>();

            // Simulação de lógica de pesquisa
            if (!string.IsNullOrEmpty(query) && query.Contains("Genshin Impact", StringComparison.OrdinalIgnoreCase))
            {
                results.Add("Genshin Impact Art Book");
            }

            ViewData["SearchTerm"] = query;
            ViewData["Results"] = results;
            return View();
        }
    }
}
