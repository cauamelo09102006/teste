using Microsoft.AspNetCore.Mvc;
using APIimobiliaria.Context;
using APIimobiliaria.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIimobiliaria.Controllers
{
    [Route("api/Usuario/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly GestaoDBContext _context;

        public UsuarioController(GestaoDBContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            return await _context.Usuarios.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<Usuario>> PostFornecedor(Usuario usuario)
        {
            
            if (_context.Usuarios.Any(u => u.Email == usuario.Email))
            {
                return BadRequest("Este e-mail já está em uso. Tente outro.");
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> PutUsuarios (int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest("O Id fornecido não corresponde ao Id do usuario.");
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Usuario atualizado com sucesso!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound("Usuario não encontrado.");
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}