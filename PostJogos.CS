using MySql.Data.MySqlClient;
using System.Text.Json;
using jogos;
using System.Runtime.InteropServices;




namespace PostJogosNamespace
{

    class PostJogos
    {

        static private string connectstring ="Server=localhost;Port=3306;Database=pokkoan;Uid=root;Pwd=admin;";
         

        static public string exec(Jogos body)
        {

        



            string query = $"INSERT INTO JOGOS (nome,valor,descricao) values (\"{body.nome}\", \"{body.valor}\", \"{body.descricao}\");";
            try
            {


                using (MySqlConnection Conn = new MySqlConnection(connectstring))
                {

                    Conn.Open();

                    using (MySqlCommand command = new MySqlCommand(query, Conn))
                    {

                        // Interessante colocar o resultado int  (que vem do metodo) em uma var para que controle melhor o sucesso
                        command.ExecuteNonQuery();
                    }

                }
                return $"VALORES INSERIDOS NO BANCO DE DADOS!   Nome - {body.nome}";

            }
            catch (Exception e)
            {
                Console.WriteLine($"ERRO: {e.Message}");
                throw new ApplicationException("Falha na Inserção de dados");

            }
            }
            
        }
    }