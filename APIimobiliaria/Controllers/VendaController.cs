using Microsoft.AspNetCore.Mvc;
using APIimobiliaria.Context;
using APIimobiliaria.Entities;

namespace APIimobiliaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : ControllerBase
    {
        private readonly GestaoDBContext _context;

        public VendaController(GestaoDBContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> ObterImovelPorId(int id)
        {
            var vendaEncontrado = await _context.Vendas.FindAsync(id);
            if (vendaEncontrado == null)
            {
                return NotFound($"Venda com ID {id} não encontrado.");
            }

            return Ok(vendaEncontrado);
        }


        [HttpPost]
        public async Task<IActionResult> CriarVenda(Venda venda)
        {
            if (venda == null)
            {
                return BadRequest("Dados inválidos para criar um venda.");
            }

            var usuarioExistente = await _context.Usuarios.FindAsync(venda.UsuarioId);
            if (usuarioExistente == null)
            {
                return BadRequest("Usuario não encontrado. O venda deve estar associado a um usuario existente.");
            }
            var imovelExistente = await _context.Imoveis.FindAsync(venda.ImovelId);
            if (imovelExistente == null)
            {
                return BadRequest("Imovel não encontrado. O venda deve estar associado a um imovel existente.");
            }

            venda.Usuario = usuarioExistente;
            venda.Imovel = imovelExistente;
            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterImovelPorId), new { id = venda.Id }, venda);
        }

    

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarVenda(int id, Venda vendaAtualizado)
    {
        if (id != vendaAtualizado.Id)
        {
            return BadRequest("ID do Venda não corresponde aos dados fornecidos.");
        }

        var vendaExistente = await _context.Vendas.FindAsync(id);
        if (vendaExistente == null)
        {
            return NotFound($"Alguel com ID {id} não encontrado.");
        }


        vendaExistente.Tipo = vendaAtualizado.Tipo;
        vendaExistente.Area = vendaAtualizado.Area;
        vendaExistente.Comodos = vendaAtualizado.Comodos;
        vendaExistente.Disponivel = vendaAtualizado.Disponivel;
        vendaExistente.Mobiliado = vendaAtualizado.Mobiliado;
        vendaExistente.Valor = vendaAtualizado.Valor;
        await _context.SaveChangesAsync();

        return Ok(vendaExistente);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> ExcluirAlguel(int id)
    {
        var vendaParaExcluir = await _context.Vendas.FindAsync(id);
        if (vendaParaExcluir == null)
        {
            return NotFound($"Pedido com ID {id} não encontrado.");
        }

        _context.Vendas.Remove(vendaParaExcluir);
        await _context.SaveChangesAsync();

        return NoContent();
    }



}
}