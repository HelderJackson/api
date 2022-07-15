using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiCredito.Enum;

namespace ApiCredito.Modelos
{
    [Table("Creditos")]
    public class Credito
    {
        /// <summary>
        /// ID do Crédito.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Valor do Crédito que se deseja liberar.
        /// </summary>
        [Required]
        public decimal ValorDoCredito { get; set; }

        /// <summary>
        /// Tipo de Crédito.
        /// </summary>
        [Required]
        public TipoDeCredito TipoDeCredito { get; set; }

        /// <summary>
        /// Quantidade de Parcela para o pagamento do crédito concebido.
        /// </summary>
        [Required]
        public int QuantidadeDeParcelas { get; set; }

        /// <summary>
        /// Data de vencimento do primeiro pagamento.
        /// </summary>
        [Required]
        public DateTime DataDeVencimento { get; set; }

        public StatusDoCredito StatusDoCredito { get; set; }

        public decimal? ValorTotal { get; set; }
        public decimal? ValorDoJuros { get; set; }
    }
}