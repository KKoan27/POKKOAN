
async function buscar(){
    try{
   let responsin =  await fetch("https://jsonplaceholder.typicode.com/todos")
    const data = await responsin.json();
    return data;
    } catch(error){
        console.log("Erro : ", error);
        return [];
    }

}




async function dados(){

    const CardID = document.querySelector(".userid");


    let userdata = await buscar();
    userdata.forEach(userdatas => {
        CardID.innerHTML += `
                    <article class="cardID">
                        <p> ${userdatas.id}</p>
                        <p> ${userdatas.title}</p>
                        <p> ${userdatas.completed}
                    </article>
    `;
        
    });
}


