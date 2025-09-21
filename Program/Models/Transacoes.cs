using System;
using System.ComponentModel.DataAnnotations;

namespace AgendaContatos.Models
{
    public class Transacoes
    {
        public int Id { get; set; }

        public int ContatoId {  get; set; }

        [Required, StringLength(150)]
        public string Nome { get; set; } = default!;

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }
    }
}
