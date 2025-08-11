using jogos;
using System.IO;
using System.Collections.Generic;
using Microsoft.Identity.Client;



namespace Pedidos
{
    class Pedido
    {
        public int ID { get; private set; }

        public string Jogador { get; private set; }

        public List<Jogos>? Itens { get; set; }

        private double valorTotal { get; set; }

        public Pedido(int ID, string Jogador, List<Jogos> Itens)
        {

            this.ID = ID;
            this.Jogador = Jogador;
            this.Itens = Itens;

        } 

    }


  
}
