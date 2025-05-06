const API_URL = "http://localhost:8080/";

async function buscar() {
    try {
        const response = await fetch(API_URL, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return await response.json();
    } catch(error) {
        console.error("Erro na requisição: ", error);
        return [];
    }
}

async function mostrarDados() {
    const cardContainer = document.querySelector(".userid");
    
    try {
        // Limpa o conteúdo anterior
        cardContainer.innerHTML = "";
        
        const games = await buscar();
        
        if (games.length === 0) {
            cardContainer.innerHTML = "<p>Nenhum jogo encontrado</p>";
            return;
        }
        
        games.forEach(game => {
            cardContainer.innerHTML += `
                <article class="cardID">
                    <h3>${game.nome}</h3>
                    <p>Preço: R$ ${game.valor.toFixed(2)}</p>
                    <p>${game.descricao}</p>
                </article>
            `;
        });
    } catch(error) {
        console.error("Erro ao exibir dados: ", error);
        cardContainer.innerHTML = "<p>Erro ao carregar os jogos</p>";
    }
}

// HTML deve chamar mostrarDados()