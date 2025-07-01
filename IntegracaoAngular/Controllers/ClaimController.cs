using Application.Commands;
using Application.Queries;
using Application.Queries.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace IntegracaoAngular.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ClaimController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClaimController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("salvar-sinistro")]
        [SwaggerOperation(
          Summary = "Adiciona um sinistro",
          Description = "Adiciona um sinistro"
          )]
        [SwaggerResponse(201, "Sinistro criado com sucesso", typeof(GetClaimResponse))]
        [SwaggerResponse(400, "Erro ao criar o sinistro")]
        public async Task<ActionResult<bool>> SaveClaim([FromBody] SaveClaimCommand command)
        {
            var saved = await _mediator.Send(command);
            return Ok(saved);
        }

        [HttpGet("get-claim/{id}")]
        [SwaggerOperation(
            Summary = "Busca um sinistro pelo ID",
            Description = "Busca um sinistro pelo ID"
            )]
        [SwaggerResponse(200, "Sinistro encontrado com sucesso", typeof(GetClaimResponse))]
        [SwaggerResponse(404, "Erro ao buscar o sinistro")]
        public async Task<ActionResult<GetClaimResponse>> GetClaim([FromRoute] int id)
        {
            var claim = await _mediator.Send(new GetClaimByIdQuery(id));

            if (claim == null)
                return NotFound();

            return Ok(claim);
        }

        [HttpGet("get-all-claims")]
        [SwaggerOperation(
            Summary = "Busca todos os sinistros",
            Description = "Retorna uma lista com todos os sinistros cadastrados"
            )]
        [SwaggerResponse(200, "Lista de sinistros retornada com sucesso", typeof(IEnumerable<GetClaimResponse>))]
        public async Task<ActionResult<IEnumerable<GetClaimResponse>>> GetAllClaims()
        {
            var claims = await _mediator.Send(new GetAllClaimsQuery());
            return Ok(claims);
        }

        [HttpDelete("delete-claim/{id}")]
        [SwaggerOperation(
            Summary = "Exclui um sinistro",
            Description = "Exclui um sinistro pelo ID"
            )]
        [SwaggerResponse(200, "Sinistro excluído com sucesso")]
        [SwaggerResponse(404, "Sinistro não encontrado")]
        public async Task<ActionResult> DeleteClaim([FromRoute] int id)
        {
            var result = await _mediator.Send(new DeleteClaimCommand(id));

            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpPut("update-claim")]
        [SwaggerOperation(
            Summary = "Atualiza um sinistro",
            Description = "Atualiza os dados de um sinistro existente"
            )]
        [SwaggerResponse(200, "Sinistro atualizado com sucesso")]
        [SwaggerResponse(400, "Erro na validação dos dados")]
        [SwaggerResponse(404, "Sinistro não encontrado")]
        public async Task<ActionResult> UpdateClaim([FromBody] UpdateClaimCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpPost("get-claims-filter")]
        [SwaggerOperation(
            Summary = "Busca os sinistros usando filtros",
            Description = "Busca os sinistros usando filtros"
            )]
        [SwaggerResponse(200, "Sinistros encontrados com sucesso", typeof(IEnumerable<GetClaimResponse>))]
        [SwaggerResponse(404, "Erro ao buscar os sinistros")]
        public async Task<ActionResult<GetClaimResponse>> GetClaimsFilter([FromBody] GetClaimsFilterQuery command)
        {
            var claims = await _mediator.Send(command);
            return Ok(claims);
        }
    }
}
