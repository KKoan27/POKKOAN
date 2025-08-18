

using db;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Net;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;


namespace jogos
{
    public class Jogos
    {
        private int id { get; set; }
        public string nome { get; set; }
        public double valor { get; private set; }

        public string descricao { get; set; }

        public int AvalPos { get; set; }
        public int AvalNeg { get; set;}



        // Contrutor
        public Jogos(string nome, double valor, string descricao)
        {
            this.nome = nome;
            this.valor = valor;
            this.descricao = descricao;


        }


        // Opções de serialização como campo readonly 
        private  static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = false, // Mantém compacto
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals
        } ;


 

        // METÓDOS:


        public static string GetJogosFromDB(string? busca = null)
        {
            var jogos = new List<Jogos>();
            try
            {
                
                MySqlConnection Conn = new MySqlConnection(DB.connectstring);//Connection abre a conexão com o BD (open é o metodo que faz isso)
                Conn.Open();
                
                using MySqlCommand command = new MySqlCommand("SELECT * FROM Jogos", Conn); //Command inicializa a query passada como 1º parametro na "IDE" do SQL 
                using MySqlDataReader reader = command.ExecuteReader();// executa a query do MySqlCommand e retorna um objeto (Reader) contendo o resultado

                
               
                while (reader.Read()) // Read passa para a proxima linha, se tiver da true, caso nao, false
                {

                    // Aqui está alocando as informações de cada linha para um objeto na Lista List<Jogos>
                    jogos.Add(new Jogos(
                        reader.GetString(reader.GetOrdinal("nome")),
                        reader.GetDouble(reader.GetOrdinal("valor")),
                        reader.GetString(reader.GetOrdinal("descricao"))
                    ));
                }

                if (!string.IsNullOrEmpty(busca))
                {
                    return JsonSerializer.Serialize(
                        jogos.Where(j =>
                        j.nome.Contains(busca, (StringComparison)5))
                        .ToList(), _jsonOptions);
                }

                return JsonSerializer.Serialize(jogos, _jsonOptions);

            }
            catch (Exception e)
            {
                Console.WriteLine($"ERRO: {e.Message}");
                throw;
            }
        }

        //Execpostpedido
        // metodo para o jogador fazer um pedido de compra x
        static public string Execpostpedido(HttpListenerRequest request)
        {

            using StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding);

            Console.WriteLine($"{reader.ReadToEnd()}");

            return "";

        }

        static public string Execpostgame(Jogos body)
        {

            try
            {

                using (MySqlConnection Conn = new MySqlConnection(DB.connectstring))
                {

                    Conn.Open();

                    using (MySqlCommand command = new MySqlCommand($"INSERT INTO JOGOS (nome,valor,descricao) values (@nome, @valor, @descricao)", Conn))
                    {
                        // Interessante colocar o resultado int  (que vem do metodo) em uma var para que controle melhor o sucesso

                        command.Parameters.AddWithValue("@nome", body.nome);
                        command.Parameters.AddWithValue("@valor", body.valor);
                        command.Parameters.AddWithValue("@descricao", body.descricao);
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