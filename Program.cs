using System.Text.Encodings.Web; // Para JavaScriptEncoder
using System.Net;
using System.Text.Json;
using System.Text;
using GetJogos;
using Mysqlx;
using Org.BouncyCastle.Crypto.Engines;
using PostJogosNamespace;
using jogos;
using Org.BouncyCastle.Security;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Cmp;



class Program
{



    static void Main()
    {
        Server server = new Server();
        server.runServer("http://localhost:8080/");
    }
}




class Server
{

    public string? requestBody;
    public string? responseBody;
    HttpListener? listener;
    HttpListenerContext? context;
    HttpListenerRequest? request;
    HttpListenerResponse? response;

    public Jogos BodyPOST; 


    getJogos connsql = new getJogos();

    public string? busca;
    // Adicione esta propriedade/field na sua classe
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = false,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        // Adicione esta linha para números decimais:
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.WriteAsString,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };






    public async Task runServer(string porta)
    {



        // Instanciando, inserindo os prefixes e iniciando servidor
        listener = new HttpListener();
        listener.Prefixes.Add(porta);
        listener.Start();


        while (true)
        {
            // aguardando a requisição e coletando a response e request
            context = listener.GetContext();
            System.Console.WriteLine("requisição chegou");
            System.Console.WriteLine(context.User);
            request = context.Request;
            response = context.Response;
            response.ContentType = "application/json; charset=utf-8";
            response.ContentEncoding = Encoding.UTF8;

            // Adicionando Headers de CORS
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS, ");
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type");



 //  Switch controladores das rotas, necessario manipular o fluxo de acordo com qual item está querendo modificar


            switch (request.HttpMethod)


            {
                case "GET":
                    Console.WriteLine("foi executado um GET");
                    var busca = request.QueryString["busca"];
                    responseBody = connsql.GetJogosAsJson(busca);
                    break;

                case "POST":

                    using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {


                        try
                        {

                            string json = await reader.ReadToEndAsync();
                            Jogos JsonSerializado = JsonSerializer.Deserialize<Jogos>(json);


                            if (!(JsonSerializado == null))
                            {
                                // instanciando o objeto jogos para uma variavel
                               BodyPOST = new Jogos(
                                    JsonSerializado.nome,
                                    JsonSerializado.valor,
                                    JsonSerializado.descricao
                                    );

                                // Jogando esta variavel como parametro para o metodo estatico
                                responseBody =  PostJogos.exec(BodyPOST);
                            }
                            else
                            {
                                throw new Exception("Algum campo ta nulo");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("DEU ERRO!!! \n {0}, \n Messagem:{1}", e, e.Message);
                        }



                    }

                    break;



                case "DELETE":
                    responseBody = "";
                    break;


                case "OPTIONS":

                    responseBody = "";
                       response.StatusCode = 200;
                    break;
            }

        


            byte[] buffer = Encoding.UTF8.GetBytes(responseBody);
            response.ContentLength64 = buffer.Length;
            using (Stream output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }




        }



    }

} 