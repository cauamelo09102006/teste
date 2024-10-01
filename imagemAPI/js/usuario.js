document.getElementById('cadastroForm').addEventListener('submit', async function(event) {
    event.preventDefault();
    
    const nome = document.getElementById('nome').value;
    const email = document.getElementById('email').value;
    const senha = document.getElementById('senha').value;
    const telefone = document.getElementById('telefone').value;
    const endereco = document.getElementById('endereco').value;
    const cpf = document.getElementById('cpf').value;
    
    const novoUsuario = {
        nome: nome,
        email: email,
        senha: senha,
        telefone: telefone,
        endereco: endereco,
        cpf: cpf
    };
    
    try {
        const response = await fetch('http://localhost:5090/api/Usuario/Usuario', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(novoUsuario)
        });

        if (!response.ok) {
            throw new Error('Erro ao cadastrar usuário.');
        }

        const novoUsuarioJson = await response.json();
        const usuarioId = novoUsuarioJson.id;

        // Armazenar o ID do usuário no localStorage
        localStorage.setItem('usuarioId', usuarioId.toString());

        alert('Usuário cadastrado com sucesso!');
        window.location.href = 'index.html'; // Redirecionar para a página inicial após o cadastro
    } catch (error) {
        console.error('Erro ao cadastrar usuário:', error);
        alert('Erro ao cadastrar usuário. Verifique o console para mais detalhes.');
    }
});