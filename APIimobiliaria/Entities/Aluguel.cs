namespace APIimobiliaria.Entities
{
    public class Aluguel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ImovelId { get; set; }
        public string Tipo { get; set; }
        public string Area { get; set; }
        public string Comodos { get; set; }
        public string Disponivel { get; set; }
        public string Mobiliado { get; set; }
        public decimal Valor { get; set; }
        public Usuario Usuario { get; set; }
        public Imovel Imovel { get; set; }
    }
}
