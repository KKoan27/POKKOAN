using System.Text.Encodings.Web;
using System.Text.Json;
using jogos;
using MySql.Data.MySqlClient;

namespace GetJogos
{
    class getJogos
    {
        private string connectstring = "Server=localhost;Port=3306;Database=pokkoan;Uid=root;Pwd=12345;";
        public string query = "SELECT * FROM Jogos";
        
        // Opções de serialização como campo readonly
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = false, // Mantém compacto
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals
        };

        public List<Jogos> GetJogosFromDB(string? busca = null)
        {
            var jogos = new List<Jogos>();

            try
            {
                using var Conn = new MySqlConnection(connectstring);
                Conn.Open();

                using var command = new MySqlCommand(query, Conn);
                using var reader = command.ExecuteReader();

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
                    // Filtra case-insensitive e retorna todos os matches
                    return jogos.Where(j => 
                        j.nome.Contains(busca, StringComparison.OrdinalIgnoreCase))
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
    }
}