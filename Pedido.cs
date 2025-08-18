using jogos;
using System.IO;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using db;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;



namespace Pedidos
{
     class Pedido
    {
        public int ID { get; private set; }

        public string Jogador { get; private set; }

        public List<Jogos>? Itens { get; set; }

        private double valorTotal { get; set; }

        public Pedido( string Jogador, List<Jogos> Itens)
        {
            this.Jogador = Jogador;
            this.Itens = Itens;

        }




        static public void PostPedidos(Pedido order)
        {


            // Receber a requisição
            //Validar os jogos recebidos = puxa os jogos do DB e verificar se eles existem e estão ativos, caso sim, jogar os ID's deles em um array e somar valor total (utilizando o campo valor) deles em uma variavel 

            // Gerar o Pedido = Criar o pedido vazio no BD e recuperar o ID dele

            // Associar Jogos ao Pedido = para os jogos validados, fazer um insert com o id do pedido gerado e tambem com os id's dos jogos.

            

            // Finalizar Resposta = retornar falando se deu certo ou errado com o id do pedido e o valor total
            



            using (MySqlConnection Conn = new MySqlConnection(DB.connectstring))
            {
                Conn.Open();

                using (MySqlCommand command = new MySqlCommand("INSERT INTO pedidos ()", Conn)) ;


            }
            ;





                
            }   
        }
        
 
    }


  

