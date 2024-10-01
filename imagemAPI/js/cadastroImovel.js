document.addEventListener('DOMContentLoaded', function() {
    const usuarioId = localStorage.getItem('usuarioId');
    if (!usuarioId) {
        alert('Usuário não está logado.');
    }
});

document.getElementById('tipoImovel').addEventListener('change', function() {
    const tipoImovel = this.value;
    const aluguelFields = document.getElementById('aluguelFields');
    const vendaFields = document.getElementById('vendaFields');

    if (tipoImovel === 'aluguel') {
        aluguelFields.classList.remove('hidden');
        vendaFields.classList.add('hidden');
    } else if (tipoImovel === 'venda') {
        vendaFields.classList.remove('hidden');
        aluguelFields.classList.add('hidden');
    } else {
        aluguelFields.classList.add('hidden');
        vendaFields.classList.add('hidden');
    }
});

document.getElementById('formNovoImovel').addEventListener('submit', async function(event) {
    event.preventDefault();

    const usuarioId = localStorage.getItem('usuarioId');
    const endereco = document.getElementById('endereco').value;
    const descricao = document.getElementById('descricao').value;
    const iptu = document.getElementById('iptu').value;
    const condicoes = document.getElementById('condicoes').value;
    const tipoImovel = document.getElementById('tipoImovel').value;
    const imagem = document.getElementById('imagem').files[0];

    const novoImovel = {
        endereço: endereco,
        descrição: descricao,
        iptu: iptu,
        condicoes: condicoes
    };

    try {
        // 1. Envia os dados do novo imóvel para criar o imóvel na API
        const responseImovel = await fetch('http://localhost:5090/api/Imovel/Imoveis', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(novoImovel)
        });

        if (!responseImovel.ok) {
            throw new Error('Erro ao salvar o imóvel.');
        }

        const novoImovelJson = await responseImovel.json();
        const imovelId = novoImovelJson.id;

        // 2. Envia os dados específicos do aluguel ou venda
        if (tipoImovel === 'aluguel') {
            const area = document.getElementById('areaAluguel').value;
            const comodos = document.getElementById('comodosAluguel').value;
            const disponivel = document.getElementById('disponivelAluguel').value;
            const mobiliado = document.getElementById('mobiliadoAluguel').value;
            const valor = document.getElementById('valorAluguel').value;

            const novoAluguel = {
                usuarioId: usuarioId,
                imovelId: imovelId,
                tipo: tipoImovel,
                area: area,
                comodos: comodos,
                disponivel: disponivel,
                mobiliado: mobiliado,
                valor: valor
            };

            const responseAluguel = await fetch('http://localhost:5090/api/Aluguel', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(novoAluguel)
            });

            if (!responseAluguel.ok) {
                throw new Error('Erro ao salvar os dados do aluguel.');
            }
        } else if (tipoImovel === 'venda') {
            const area = document.getElementById('areaVenda').value;
            const comodos = document.getElementById('comodosVenda').value;
            const disponivel = document.getElementById('disponivelVenda').value;
            const mobiliado = document.getElementById('mobiliadoVenda').value;
            const valor = document.getElementById('valorVenda').value;

            const novoVenda = {
                usuarioId: usuarioId,
                imovelId: imovelId,
                tipo: tipoImovel,
                area: area,
                comodos: comodos,
                disponivel: disponivel,
                mobiliado: mobiliado,
                valor: valor
            };

            const responseVenda = await fetch('http://localhost:5090/api/Venda', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(novoVenda)
            });

            if (!responseVenda.ok) {
                throw new Error('Erro ao salvar os dados da venda.');
            }
        }

        // 3. Envia a imagem para o endpoint de upload de imagem do imóvel
        const formData = new FormData();
        formData.append('file', imagem);

        const responseImagem = await fetch(`http://localhost:5090/api/Imovel/Imoveis/${imovelId}/upload`, {
            method: 'POST',
            body: formData
        });

        if (!responseImagem.ok) {
            throw new Error('Erro ao fazer upload da imagem.');
        }

        alert('Imóvel cadastrado com sucesso! ID: ' + imovelId);

        // Limpar o formulário após o envio
        document.getElementById('formNovoImovel').reset();
        document.getElementById('aluguelFields').classList.add('hidden');
        document.getElementById('vendaFields').classList.add('hidden');
    } catch (error) {
        console.error('Erro ao salvar o imóvel:', error);
        alert('Erro ao salvar o imóvel. Verifique o console para mais detalhes.');
    }
});