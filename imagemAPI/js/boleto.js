function boleto() {
    let BoletoUrl = 'pdfs/comprovante.jpg';


    if (BoletoUrl) {
        const link = document.createElement('a');
        link.href = BoletoUrl;
        link.download = BoletoUrl.split('/').pop(); // Nome do arquivo para download
        link.click(); // Iniciar download
      }
}