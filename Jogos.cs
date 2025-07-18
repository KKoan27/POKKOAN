

using db;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Net;
using MySql.Data.MySqlClient;


namespace jogos
{
    class Jogos
    {
        private int id { get; set; }
        public string nome { get; set; }
        public double valor { get; private set; }

        public string descricao { get; set; }



        // Contrutor
        public Jogos(string nome, double valor, string descricao)
        {
            this.nome = nome;
            this.valor = valor;
            this.descricao = descricao;


        }


        // Opções de serialização como campo readonly 
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = false, // Mantém compacto
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals
        };




        // METÓDOS:


        public List<Jogos> GetJogosFromDB(string? busca = null)
        {
            var jogos = new List<Jogos>();
            DB.query = "SELECT * FROM Jogos";

            try
            {
                MySqlConnection Conn = new MySqlConnection(DB.connectstring);
                Conn.Open();

                using MySqlCommand command = new MySqlCommand(DB.query, Conn);
                using MySqlDataReader reader = command.ExecuteReader();
                // AQUI O VALOR TA SENDO DEVOLVIDO E NAO CONSUMINDO!!!
                while (reader.Read())
                {
                    jogos.Add(new Jogos(
                        reader.GetString(reader.GetOrdinal("nome")),
                        reader.GetDouble(reader.GetOrdinal("valor")),
                        reader.GetString(reader.GetOrdinal("descricao"))
                    ));
                }

                if (!string.IsNullOrEmpty(busca))
                {
                    return jogos.Where(j =>
                        j.nome.Contains(busca, (StringComparison)5))
                        .ToList();
                }

                return jogos;


            }
            catch (Exception e)
            {
                Console.WriteLine($"ERRO: {e.Message}");
                throw;
            }
        }

        // Método alternativo se precisar retornar JSON diretamente
        public string GetJogosAsJson(string? busca = null)
        {
            var jogos = GetJogosFromDB(busca);
            return JsonSerializer.Serialize(jogos, _jsonOptions);
        }



        public string execpostpedido(HttpListenerRequest request)
        {
            DB.query = "INSERT INTO PEDIDOS (usuario,descrição,pagamento)";

            using StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding);

            Console.WriteLine($"{reader.ReadToEnd()}");


            return "";

        }

          public string execpostgame(Jogos body)
        {

             DB.query = $"INSERT INTO JOGOS (nome,valor,descricao) values (\"{body.nome}\", \"{body.valor}\", \"{body.descricao}\");";
            try
            {


                using (MySqlConnection Conn = new MySqlConnection(DB.query))
                {

                    Conn.Open();

                    using (MySqlCommand command = new MySqlCommand(DB.query , Conn))
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