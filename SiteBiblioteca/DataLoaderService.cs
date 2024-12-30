using Microsoft.Extensions.Configuration;
using SiteBiblioteca.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SiteBiblioteca.Data;

public class DataLoaderService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public DataLoaderService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public void LoadDataIfEmpty()
    {
        // Verificar se a tabela Livros está vazia
        if (!_context.livros.Any())
        {
            // Carregar dados do arquivo de configuração
            var livrosConfig = _configuration.GetSection("Livros").Get<LivrosConfig>();

            // Adicionar livros e autores no banco de dados
            foreach (var livro in livrosConfig.Livros)
            {
                _context.livros.Add(livro);

                // Verificar se o autor já está na base de dados
                if (!_context.autores.Any(a => a.Id == livro.autor.Id))
                {
                    _context.autores.Add(livro.autor);
                }
            }

            _context.SaveChanges();
        }

        if (!_context._dadosBiblioteca.Any())
        {
            // Carregar dados do arquivo de configuração
            var dadosBibliotecaDefault = _configuration.GetSection("DadosBiblioteca").Get<DadosBiblioteca>();

            _context._dadosBiblioteca.Add(dadosBibliotecaDefault);

            _context.SaveChanges();
        }
    }
}
