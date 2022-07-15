using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCredito.Contexto;
using ApiCredito.Modelos;
using ApiCredito.Negocio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCredito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditosController : ControllerBase
    {
        private readonly BancoDbContext _bancoContext;
        public CreditosController(BancoDbContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        /// <summary>
        /// Seleciona todos os Créditos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Credito>> Get()
        {
            try
            {
                return _bancoContext.Creditos.ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter os Créditos no banco de dados");
            }
        }

        /// <summary>
        /// Seleciona um Crédito específico.
        /// </summary>
        /// <param name="creditoID">ID do crédito no qual será selecionado.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Credito> Get(int creditoID)
        {
            var credito = _bancoContext.Creditos.FirstOrDefault(c => c.ID == creditoID);

            if (credito == null)
            {
                // 404
                return NotFound();
            }

            return credito;
        }

        /// <summary>
        /// Liberação do Crédito.
        /// </summary>
        /// <param name="credito">Crédito no qual será concebido.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post([FromBody] Credito credito)
        {
            try
            {
                Validacao(credito);
                _bancoContext.Creditos.Add(credito);
                _bancoContext.SaveChanges();
                string status = "";
                if (credito.StatusDoCredito == Enum.StatusDoCredito.Aprovado)
                {
                    status = "Aprovado";
                }
                else
                {
                    status = "Recusado";
                }

                var mensagem = $"Operação Concluída com Sucesso." +
                    $"\n Status do Crédito: {status} " +
                    $"\n Valor Total com Juros: R${credito.ValorTotal}" +
                    $"\n Valor do Juro: R${credito.ValorDoJuros}";
                return Ok(mensagem);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Validar as regras necessárias para conceber o crédito ao usuário.
        /// </summary>
        /// <param name="credito">A entidade do Crédito.</param>
        private static void Validacao(Credito credito)
        {
            var negocio = new CreditoNegocio();

            negocio.Validar(credito);
            if (credito.StatusDoCredito == Enum.StatusDoCredito.Aprovado)
            {
                negocio.CalcularJuros(credito);
            }
        }

        /// <summary>
        /// Altera as informações contidas no Crédito
        /// </summary>
        /// <param name="id">ID do crédito que se deseja alterar.</param>
        /// <param name="credito">Entidade do Crédito.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, Credito credito)
        {
            if (id != credito.ID)
            {
                return BadRequest("Não foi possível atualizar");
            }
            _bancoContext.Entry(credito).State = EntityState.Modified;
            try
            {
                Validacao(credito);
                _bancoContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteCredito(id))
                {
                    return NotFound($"Crédito com id = {id} não foi encontrado.");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        /// <summary>
        /// Exclui um Crédito específico.
        /// </summary>
        /// <param name="creditoID">ID do Crédito.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult<Credito> Delete(int creditoID)
        {
            try
            {
                var credito = _bancoContext.Creditos.FirstOrDefault(c => c.ID == creditoID);
                if (credito == null)
                {
                    return NotFound("Não foi encontrado.");
                }
                _bancoContext.Creditos.Remove(credito);
                _bancoContext.SaveChangesAsync();
                return credito;
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        private bool ExisteCredito(int creditoID)
        {
            return _bancoContext.Creditos.Any(c => c.ID == creditoID);
        }
    }
}