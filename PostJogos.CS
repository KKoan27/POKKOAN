using MySql.Data.MySqlClient;
using System.Text.Json.Nodes;
using System.Text.Json;
using Azure;
using jogos;




namespace PostJogos
{

    class PostJogos
    {

        private string connectstring = "Server=localhost;Port=3306;Database=pokkoan;Uid=root;Pwd=12345;";
        public string query = "Select * From Jogos";

        public Jogos search;
        public List<Jogos> jogos = [];
        public string ResponseJson;

        public string exec(string? busca, string body)
        {

           


                try
                {

                    using (MySqlConnection Conn = new MySqlConnection(connectstring))
                    {

                        Conn.Open();

                        using (MySqlCommand command = new MySqlCommand(query, Conn))
                        {

                            using (MySqlDataReader reader = command.ExecuteReader())
                            {

                                while (reader.Read())
                                {

                                    jogos.Add(new Jogos(
                                        reader.GetString(reader.GetOrdinal("nome")),
                                        reader.GetDouble(reader.GetOrdinal("valor")),
                                        reader.GetString(reader.GetOrdinal("descricao"))
                                        ));
                                }
                                if (busca != null)
                                {

                                    foreach (var i in jogos)
                                    {
                                        if (i.nome == busca)
                                        {
                                            search = new Jogos(i.nome, i.valor, i.descricao);
                                            ResponseJson = JsonSerializer.Serialize(search);

                                        }

                                    }
                                }
                                else
                                {
                                    ResponseJson = JsonSerializer.Serialize(jogos);
                                }
                            }
                        }

                    }
                    return ResponseJson;


                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERRO: {e.Message}");
                    throw new ApplicationException("Falha na operação", e);
                }
            }
            
        }
    }