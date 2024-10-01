using Microsoft.AspNetCore.Mvc;
using APIimobiliaria.Context;
using APIimobiliaria.Entities;

namespace APIimobiliaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AluguelController : ControllerBase
    {
        private readonly GestaoDBContext _context;

        public AluguelController(GestaoDBContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> ObterAluguelPorId(int id)
        {
            var aluguelEncontrado = await _context.Alugueis.FindAsync(id);
            if (aluguelEncontrado == null)
            {
                return NotFound($"Aluguel com ID {id} não encontrado.");
            }

            return Ok(aluguelEncontrado);
        }


        [HttpPost]
        public async Task<IActionResult> CriarAlguel(Aluguel aluguel)
        {
            if (aluguel == null)
            {
                return BadRequest("Dados inválidos para criar um Aluguel.");
            }

            var usuarioExistente = await _context.Usuarios.FindAsync(aluguel.UsuarioId);
            if (usuarioExistente == null)
            {
                return BadRequest("Usuario não encontrado. O aluguel deve estar associado a um usuario existente.");
            }
            var imovelExistente = await _context.Imoveis.FindAsync(aluguel.ImovelId);
            if (imovelExistente == null)
            {
                return BadRequest("Imovel não encontrado. O aluguel deve estar associado a um imovel existente.");
            }

            aluguel.Usuario = usuarioExistente;
            aluguel.Imovel = imovelExistente;
            _context.Alugueis.Add(aluguel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterAluguelPorId), new { id = aluguel.Id }, aluguel);
        }

    

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarAluguel(int id, Aluguel aluguelAtualizado)
    {
        if (id != aluguelAtualizado.Id)
        {
            return BadRequest("ID do Alguel não corresponde aos dados fornecidos.");
        }

        var aluguelExistente = await _context.Alugueis.FindAsync(id);
        if (aluguelExistente == null)
        {
            return NotFound($"Alguel com ID {id} não encontrado.");
        }


        aluguelExistente.Tipo = aluguelAtualizado.Tipo;
        aluguelExistente.Area = aluguelAtualizado.Area;
        aluguelExistente.Comodos = aluguelAtualizado.Comodos;
        aluguelExistente.Disponivel = aluguelAtualizado.Disponivel;
        aluguelExistente.Mobiliado = aluguelAtualizado.Mobiliado;
        aluguelExistente.Valor = aluguelAtualizado.Valor;
        await _context.SaveChangesAsync();

        return Ok(aluguelExistente);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> ExcluirAlguel(int id)
    {
        var aluguelParaExcluir = await _context.Alugueis.FindAsync(id);
        if (aluguelParaExcluir == null)
        {
            return NotFound($"Pedido com ID {id} não encontrado.");
        }

        _context.Alugueis.Remove(aluguelParaExcluir);
        await _context.SaveChangesAsync();

        return NoContent();
    }



}
}