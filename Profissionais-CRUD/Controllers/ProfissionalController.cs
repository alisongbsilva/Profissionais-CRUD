using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profissionais_CRUD.Models;
using Profissionais_CRUD.Util;
using Profissionais_CRUD.ViewModels;
using System.Linq;

namespace Profissionais_CRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfissionalController : ControllerBase
    {
        public DataContext Context { get; }

        public ProfissionalController(DataContext context)
        {
            Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string nome = null, int numeroRegistroInicio = 0, int numeroRegistroFim = 0, bool somenteAtivos = false)
        {
            var query = Context.Profissionais.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
                query = query.Where(p => p.NomeCompleto.Contains(nome));

            if (numeroRegistroInicio != 0)
                query = query.Where(p => p.NumeroRegistro >= numeroRegistroInicio);

            if (numeroRegistroFim != 0)
                query = query.Where(p => p.NumeroRegistro <= numeroRegistroFim);

            if (somenteAtivos)
                query = query.Where(p => p.Ativo);

            query = query.OrderBy(p => p.NomeCompleto);

            var result = await query.ToListAsync();

            return Ok(result.Select(p => new ProfissionalViewModel
            {
                NumeroRegistro = p.NumeroRegistro,
                Nome = p.NomeCompleto,
                CPF = p.CPF,
                Ativo = p.Ativo,
                DataCriacao = p.DataCriacao

            }));
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var profissional = await Context.Profissionais.FindAsync(id);
            if (profissional == null)
                return BadRequest("Profissional não encontrado");
            return Ok(profissional);
        }

        [HttpPost]
        public async Task<IActionResult> AddProfissional(Profissional profissional)
        {

            if (Context.Profissionais.Any(p => p.NomeCompleto == profissional.NomeCompleto))
                return BadRequest("Nome de usuário já existente.");

            var ultimoRegistro = Context.Profissionais.Max(p => p.NumeroRegistro);

            profissional.NumeroRegistro = ultimoRegistro == 0 ? 10000001 : ultimoRegistro + 1;

            if (!Validacoes.ValidaCPF(profissional.CPF))
                return BadRequest("CPF inválido!");

            if (profissional.DataNascimento.AddYears(18) > DateTime.Now)
                return BadRequest("O Profissional não pode ter menos que 18 anos de idade.");

            profissional.DataCriacao = DateTime.Now;

            Context.Profissionais.Add(profissional);
            await Context.SaveChangesAsync();


            return Ok(profissional);

        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateProfissional(Profissional updateQuery)
        {
            var dbProfissional = await Context.Profissionais.AnyAsync(p => p.Id == updateQuery.Id);
            if (!dbProfissional)
                return BadRequest("Profissional não encontrado");
            
            Context.Profissionais.Update(updateQuery);

            await Context.SaveChangesAsync();

            return Ok(updateQuery);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteProfissional(int id)
        {
            var dbProfissional = await Context.Profissionais.FindAsync(id);
            if (dbProfissional == null)
                return BadRequest("Profissional não encontrado");

            Context.Profissionais.Remove(dbProfissional);
            await Context.SaveChangesAsync();

            return Ok("Profissional removido com sucesso!");
        }

    }
}
