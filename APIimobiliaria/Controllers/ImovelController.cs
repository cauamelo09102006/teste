using Microsoft.AspNetCore.Mvc;
using APIimobiliaria.Context;
using APIimobiliaria.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIimobiliaria.Controllers
{
    [Route("api/Imovel/[controller]")]
    [ApiController]
    public class ImoveisController : ControllerBase
    {
        private readonly GestaoDBContext _context;

        public ImoveisController(GestaoDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Imovel>>> GetImoveis()
        {
            return await _context.Imoveis.ToListAsync();
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<ImovelDetalhesDto>> GetImovel(int id)
        {
            var imovel = await _context.Imoveis.FindAsync(id);

            if (imovel == null)
            {
                return NotFound();
            }

            var alugueis = await _context.Alugueis.Where(a => a.ImovelId == id).ToListAsync();
            var vendas = await _context.Vendas.Where(v => v.ImovelId == id).ToListAsync();

            var imovelDetalhes = new ImovelDetalhesDto
            {
                Imovel = imovel,
                Alugueis = alugueis,
                Vendas = vendas
            };

            return Ok(imovelDetalhes);
        }

      
      
        [HttpPost]
        public async Task<ActionResult<Imovel>> PostImovel(Imovel imovel)
        {
            _context.Imoveis.Add(imovel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImovel), new { id = imovel.Id }, imovel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutImovel(int id, Imovel imovel)
        {
            if (id != imovel.Id)
            {
                return BadRequest("O Id fornecido não corresponde ao Id do imovel.");
            }

            _context.Entry(imovel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Imovel atualizado com sucesso!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImovelExists(id))
                {
                    return NotFound("Imovel não encontrado.");
                }
                else
                {
                    throw;
                }
            }
        }


      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImovel(int id)
        {
            var imovel = await _context.Imoveis.FindAsync(id);
            if (imovel == null)
            {
                return NotFound();
            }

            _context.Imoveis.Remove(imovel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método para upload de imagem
        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadImagem(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Arquivo de imagem inválido.");
            }

            var imovel = await _context.Imoveis.FindAsync(id);
            if (imovel == null)
            {
                return NotFound("Imóvel não encontrado.");
            }

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine("wwwroot/images", fileName);

            // Certifique-se de que o diretório existe
            if (!Directory.Exists("wwwroot/images"))
            {
                Directory.CreateDirectory("wwwroot/images");
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            imovel.ImagemCaminho = $"/images/{fileName}";
            _context.Entry(imovel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { caminho = imovel.ImagemCaminho });
        }

        // Método para download de imagem
        [HttpGet("{id}/imagem")]
        public async Task<IActionResult> GetImagem(int id)
        {
            var imovel = await _context.Imoveis.FindAsync(id);
            if (imovel == null || string.IsNullOrEmpty(imovel.ImagemCaminho))
            {
                return NotFound("Imagem não encontrada.");
            }

            var imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imovel.ImagemCaminho.TrimStart('/'));

            if (!System.IO.File.Exists(imageFilePath))
            {
                return NotFound("Imagem não encontrada no servidor.");
            }

            var image = System.IO.File.OpenRead(imageFilePath);
            return File(image, "image/jpeg"); // Ajuste o tipo MIME conforme necessário
        }


        private bool ImovelExists(int id)
        {
            return _context.Imoveis.Any(e => e.Id == id);
        }
    }

    public class ImovelDetalhesDto
    {
        public Imovel Imovel { get; set; }
        public IEnumerable<Aluguel> Alugueis { get; set; }
        public IEnumerable<Venda> Vendas { get; set; }
    }

}