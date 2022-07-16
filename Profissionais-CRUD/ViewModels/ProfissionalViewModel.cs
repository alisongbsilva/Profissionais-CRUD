namespace Profissionais_CRUD.ViewModels
{
    public class ProfissionalViewModel
    {
        public int NumeroRegistro { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
