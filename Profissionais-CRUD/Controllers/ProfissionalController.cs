using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profissionais_CRUD.Models;
using Profissionais_CRUD.Util;

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

            List<int> listaRegistros = new List<int>();

            await foreach (var prof in Context.Profissionais)
            {
                if (prof.NomeCompleto == profissional.NomeCompleto)
                    return BadRequest("Nome de usuário já existente.");

                listaRegistros.Add(prof.NumeroRegistro);
            }

            var ultimoRegistro = listaRegistros.LastOrDefault();
            if (ultimoRegistro < 10000001)
                profissional.NumeroRegistro = 10000001;
            else
                profissional.NumeroRegistro = ultimoRegistro + 1;

            if (!Validacoes.ValidaCPF(profissional.CPF))
                return BadRequest("CPF inválido!");

            if (profissional.DataNascimento.AddYears(18) > DateTime.Now)
                return BadRequest("O Profissional não pode ter menos que 18 anos de idade.");

            profissional.DataCriacao = DateTime.Now;

            Context.Profissionais.Add(profissional);
            await Context.SaveChangesAsync();


            return Ok(await Context.Profissionais.ToListAsync());

        }

        private object OrderBy(int id)
        {
            throw new NotImplementedException();
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
