async function carregarDetalhes() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const imovelId = urlParams.get('id');

    try {
        const responseImovel = await fetch(`http://localhost:5090/api/Imovel/Imoveis/${imovelId}`);
        if (!responseImovel.ok) {
            throw new Error('Imóvel não encontrado.');
        }
        const imovelDetalhes = await responseImovel.json()
        const imovel = imovelDetalhes.imovel;


        
        document.getElementById('endereco').textContent = imovel.endereço;
        document.getElementById('descricao').textContent = imovel.descrição;
        document.getElementById('iptu').textContent = imovel.iptu;
        document.getElementById('condicoes').textContent = imovel.condicoes;

        const imageResponse = await fetch(`http://localhost:5090/api/Imovel/Imoveis/${imovelId}/imagem`);
        if (!imageResponse.ok) {
            throw new Error('Imagem não encontrada.');
        }
        const blob = await imageResponse.blob();
        const imageUrl = URL.createObjectURL(blob);
        document.getElementById('imovelImage').src = imageUrl;

        if (imovelDetalhes.alugueis.length > 0) {
            localStorage.setItem('tipoImovel', 'aluguel');
            const aluguel = imovelDetalhes.alugueis[0];
            document.getElementById('tipoAluguel').textContent = aluguel.tipo;
            document.getElementById('areaAluguel').textContent = aluguel.area;
            document.getElementById('comodosAluguel').textContent = aluguel.comodos;
            document.getElementById('disponivelAluguel').textContent = aluguel.disponivel;
            document.getElementById('mobiliadoAluguel').textContent = aluguel.mobiliado;
            document.getElementById('valorAluguel').textContent = aluguel.valor.toFixed(2);
            document.getElementById('aluguelDetalhes').style.display = 'block';

            const responseUsuarioAluguel = await fetch(`http://localhost:5090/api/Usuario/Usuario/${aluguel.usuarioId}`);
            if (responseUsuarioAluguel.ok) {
                const usuarioAluguel = await responseUsuarioAluguel.json();
                document.getElementById('nomeUsuarioAluguel').textContent = usuarioAluguel.nome;
                document.getElementById('emailUsuarioAluguel').textContent = usuarioAluguel.email;
                document.getElementById('telefoneUsuarioAluguel').textContent = usuarioAluguel.telefone;
            }
        } else {
            document.getElementById('aluguelDetalhes').style.display = 'none';
        }

        if (imovelDetalhes.vendas.length > 0) {
            localStorage.setItem('tipoImovel', 'venda');
            const venda = imovelDetalhes.vendas[0];
            document.getElementById('tipoVenda').textContent = venda.tipo;
            document.getElementById('areaVenda').textContent = venda.area;
            document.getElementById('comodosVenda').textContent = venda.comodos;
            document.getElementById('disponivelVenda').textContent = venda.disponivel;
            document.getElementById('mobiliadoVenda').textContent = venda.mobiliado;
            document.getElementById('valorVenda').textContent = venda.valor.toFixed(2);
            document.getElementById('vendaDetalhes').style.display = 'block';

            const responseUsuarioVenda = await fetch(`http://localhost:5090/api/Usuario/Usuario/${venda.usuarioId}`);
            if (responseUsuarioVenda.ok) {
                const usuarioVenda = await responseUsuarioVenda.json();
                document.getElementById('nomeUsuarioVenda').textContent = usuarioVenda.nome;
                document.getElementById('emailUsuarioVenda').textContent = usuarioVenda.email;
                document.getElementById('telefoneUsuarioVenda').textContent = usuarioVenda.telefone;
            }
        } else {
            document.getElementById('vendaDetalhes').style.display = 'none';
        }
    } catch (error) {
        console.error('Erro ao obter os detalhes do imóvel:', error);
        alert('Imóvel não encontrado.');
    }
}

// Carregar os detalhes do imóvel ao carregar a página
carregarDetalhes();