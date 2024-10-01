using System.Reflection.Metadata;

namespace APIimobiliaria.Entities
{
    public class Imovel
    {
        public int Id { get; set; }
        public string Endereço { get; set; }
        public string Descrição { get; set; }
        public string IPTU { get; set; }
        public string Condicoes { get; set; }
        public string ImagemCaminho { get; set; } // Adicionando o campo para o caminho da imagem
    }
}
