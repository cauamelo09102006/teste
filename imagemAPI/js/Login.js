document.getElementById("loginForm").addEventListener("submit", async function(event) {
    event.preventDefault();
   
    const email = document.getElementById('email').value;
    const senha = document.getElementById('senha').value;
    const loginError = document.getElementById('loginError');
 
    loginError.style.display = 'none';
    loginError.textContent = '';
 
    if (email === '' || senha === '') {
        alert("Preencha todos os campos");
        return;
    }
 
    try {
        const response = await fetch('http://localhost:5090/api/Autenticacao/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email: email, senha: senha })
        });
       
        const responseBody = await response.json();
        console.log('Resposta da API:', responseBody);
 
            localStorage.setItem('usuarioId', responseBody.usuarioId.toString());  
            alert('Login bem-sucedido!');
            window.location.href = "index.html";
 
 
    } catch (error) {
        console.error('Erro ao fazer login:', error);
        loginError.style.display = 'inline';
        loginError.textContent = 'Erro ao fazer login: ' + error.message;
    }
});
tem menu de contexto