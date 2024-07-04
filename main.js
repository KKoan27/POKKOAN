'use strict';
const switcher = document.querySelector('.btn');

switcher.addEventListener('click', function () {
    // Alterna a classe 'dark-theme' no elemento body
    document.body.classList.toggle('dark-theme');
    
    // Obtém a classe atual do elemento body
    var classname = document.body.className;
    
    // Verifica se a classe atual é 'dark-theme'
    if (classname.includes('dark-theme')) {
        this.textContent = "Light"; // Altera o texto do botão para "Light"
    } else {
        this.textContent = "Dark"; // Altera o texto do botão para "Dark"
    }
    
    // Loga a classe atual no console
    console.log('current class name: ' + classname);
});
