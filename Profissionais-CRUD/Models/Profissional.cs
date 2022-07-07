using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Profissionais_CRUD.Models
{
    public class Profissional
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(300)]
        public string NomeCompleto { get; set; }
        [Required]
        public string CPF { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        [Required]
        [MaxLength(1)]
        public string Sexo { get; set; }
        [Required]
        public bool Ativo { get; set; }
        [Required]
        public int NumeroRegistro { get; set; }
        [MaxLength(8)]
        public string CEP { get; set; }
        [MaxLength(300)]
        public string Cidade { get; set; }
        public decimal ValorRenda { get; set; }
        [Required]
        public DateTime DataCriacao { get; set; }
    }
}
