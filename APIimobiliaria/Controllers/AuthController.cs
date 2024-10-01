using Microsoft.AspNetCore.Mvc;
using APIimobiliaria.Context;
using APIimobiliaria.Entities;
 
 
namespace APIimobiliaria.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly GestaoDBContext _context;
 
        public AutenticacaoController(GestaoDBContext dbContext)
        {
            _context = dbContext;
        }
 
        [HttpPost("login")]
        public IActionResult Login([FromBody] Usuario usuario)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == usuario.Email && u.Senha == usuario.Senha);
            if (user == null)
            {
                return Unauthorized();
            }
         
            var response = new { Message = "Logado!", UsuarioId = user.Id };
            Console.WriteLine($"Login Response: {System.Text.Json.JsonSerializer.Serialize(response)}"); // Log para verificar
            return Ok(response);
        }
    }
}