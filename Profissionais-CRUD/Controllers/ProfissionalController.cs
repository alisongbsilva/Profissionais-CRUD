using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profissionais_CRUD.Models;

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
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Context.Profissionais.ToListAsync());
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
            Context.Profissionais.Add(profissional);
            await Context.SaveChangesAsync();

            return Ok(await Context.Profissionais.ToListAsync());
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateProfissional(Profissional updateQuery)
        {
            var dbProfissional = await Context.Profissionais.FindAsync(updateQuery.Id);
            if (dbProfissional == null)
                return BadRequest("Profissional não encontrado");

            dbProfissional.NomeCompleto = updateQuery.NomeCompleto;
            dbProfissional.CPF = updateQuery.CPF;
            dbProfissional.DataNascimento = updateQuery.DataNascimento;
            dbProfissional.Sexo = updateQuery.Sexo;
            dbProfissional.Ativo = updateQuery.Ativo;
            dbProfissional.CEP = updateQuery.CEP;
            dbProfissional.Cidade = updateQuery.Cidade;
            dbProfissional.ValorRenda = updateQuery.ValorRenda;

            await Context.SaveChangesAsync();

            return Ok(await Context.Profissionais.ToListAsync());
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
