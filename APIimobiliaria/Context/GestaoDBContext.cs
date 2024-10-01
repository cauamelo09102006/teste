using APIimobiliaria.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIimobiliaria.Context
{
    public class GestaoDBContext : DbContext
    {

        public GestaoDBContext(DbContextOptions<GestaoDBContext> options)
     : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Imovel> Imoveis { get; set; }
        public DbSet<Aluguel> Alugueis { get; set; }
        public DbSet<Venda> Vendas { get; set; }

    }
}
