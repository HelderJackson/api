using System;
using ApiCredito.Modelos;

namespace ApiCredito.Negocio
{
    public class CreditoNegocio
    {
        public void Validar(Credito credito)
        {
            // Inicia flag de status
            credito.StatusDoCredito = Enum.StatusDoCredito.Aprovado;

            if (credito.ValorDoCredito > 100000000)
            {
                //throw new Exception("O valor máximo a ser liberado para qualquer tipo de empréstimo é de R$ 1.000.000,00.");
                credito.StatusDoCredito = Enum.StatusDoCredito.Recusado;
            }

            if (credito.QuantidadeDeParcelas < 5 || credito.QuantidadeDeParcelas > 72)
            {
                //throw new Exception("A quantidade mínima de parcelas é de 5x e a máxima é de 72x.");
                credito.StatusDoCredito = Enum.StatusDoCredito.Recusado;
            }

            if (credito.TipoDeCredito == Enum.TipoDeCredito.CreditoPessoaJuridica && credito.ValorDoCredito > 1500000)
            {
                //throw new Exception("Para o crédito de pessoa jurídica, o valor mínimo a ser liberado é de R$ 15.000, 00");
                credito.StatusDoCredito = Enum.StatusDoCredito.Recusado;
            }

            if (credito.DataDeVencimento > DateTime.Now.AddDays(40))
            {
                //throw new Exception("A data do primeiro vencimento sempre será no mínimo 15 dias e no máximo 40 dias a partir da data atual.");
                credito.StatusDoCredito = Enum.StatusDoCredito.Recusado;
            }
        }

        public void CalcularJuros(Credito credito)
        {
            switch (credito.TipoDeCredito)
            {
                case Enum.TipoDeCredito.CreditoDireto:
                    credito.ValorDoJuros = credito.ValorDoCredito * 2/100;
                    credito.ValorTotal = credito.ValorDoCredito + credito.ValorDoJuros;
                    break;
                case Enum.TipoDeCredito.CreditoConsignado:
                    credito.ValorDoJuros = credito.ValorDoCredito * 1/100;
                    credito.ValorTotal = credito.ValorDoCredito + credito.ValorDoJuros;
                    break;
                case Enum.TipoDeCredito.CreditoPessoaJuridica:
                    credito.ValorDoJuros = credito.ValorDoCredito * 5/100;
                    credito.ValorTotal = credito.ValorDoCredito + credito.ValorDoJuros;
                    break;
                case Enum.TipoDeCredito.CreditoPessoaFisica:
                    credito.ValorDoJuros = credito.ValorDoCredito * 3/100;
                    credito.ValorTotal = credito.ValorDoCredito + credito.ValorDoJuros;
                    break;
                case Enum.TipoDeCredito.CreditoImobiliario:
                    credito.ValorDoJuros = credito.ValorDoCredito * 9/100;
                    credito.ValorTotal = credito.ValorDoCredito + credito.ValorDoJuros;
                    break;
                default:
                    break;
            }
        }
    }
}