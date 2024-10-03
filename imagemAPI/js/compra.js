let selectedPaymentMethod = ''; // Variável para armazenar o método de pagamento selecionado
const tipoImovel = localStorage.getItem('tipoImovel'); // Recupera o tipo de imóvel (aluguel ou venda)

// Função para mostrar a mensagem de pagamento
function showPaymentMessage(paymentMethod) {
  selectedPaymentMethod = paymentMethod;
  document.getElementById("payment-message").style.display = "block";
}

// Função para confirmar o pagamento e fazer o download do contrato correspondente
function confirmPayment() {
  let contratoUrl = '';

  if (tipoImovel === 'aluguel') {
    contratoUrl = 'pdfs/contrato.pdf'; // URL do contrato de aluguel
  } else if (tipoImovel === 'venda') {
    contratoUrl = 'pdfs/contratovenda.pdf'; // URL do contrato de venda
  }

  // Fazer download do contrato
  if (contratoUrl) {
    const link = document.createElement('a');
    link.href = contratoUrl;
    link.download = contratoUrl.split('/').pop(); // Nome do arquivo para download
    link.click(); // Iniciar download
  }

  // Redirecionar para a página correspondente ao método de pagamento
  if (selectedPaymentMethod === 'pix') {
    window.location.href = 'pix.html';
  } else if (selectedPaymentMethod === 'boleto') {
    window.location.href = 'boleto.html';
  } else if (selectedPaymentMethod === 'cartao') {
    window.location.href = 'cartao.html';
  }
}
