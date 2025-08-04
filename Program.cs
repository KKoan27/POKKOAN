using System.Text.Encodings.Web; // Para JavaScriptEncoder
using System.Net;
using System.Text.Json;
using System.Text;
using PostJogosNamespace;
using jogos;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Specialized;
using Google.Protobuf.WellKnownTypes;



class Program
{     async static Task Main()
    {
        Server server = new Server();
        await server.runServer("http://localhost:8080/");


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

    public string busca = "";


    public Jogos? jogo;

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
            HttpListenerContext context = await listener.GetContextAsync();
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



            //  Switch controladores das rotas, necessario manipular o fluxo de acordo com qual item está querendo manipular


            switch (request.HttpMethod)


            {
                case "GET":
                    Console.WriteLine("foi executado um GET");

                    if (request.QueryString["busca"] == null || request.QueryString["busca"] == "")
                    {
                        responseBody = Jogos.GetJogosFromDB();
                    }
                    else
                    {
                        responseBody = Jogos.GetJogosFromDB(busca);
                    }
                     
                    break;

                case "POST":

                    if (request.RawUrl.IsNullOrEmpty())
                    {
                        using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                        {
                            try
                            {

                                string json = await reader.ReadToEndAsync();
                                Jogos? JsonSerializado = JsonSerializer.Deserialize<Jogos>(json);


                                if (!(JsonSerializado == null))
                                {
                                    // instanciando o objeto jogos para uma variavel
                                    jogo = new Jogos(
                                         JsonSerializado.nome,
                                         JsonSerializado.valor,
                                         JsonSerializado.descricao
                                         );

                                    // Jogando esta variavel como parametro para o metodo 
                                    responseBody = jogo.Execpostgame(jogo);
                                }
                                else
                                {

                                    throw new Exception("Algum campo ta nulo");

                                }
                            }
                            catch (Exception e)
                            {

                                responseBody = "Erro ao processar o JSON";

                                Console.WriteLine("DEU ERRO!!! \n {0}, \n Messagem:{1}", e, e.Message);
                            }



                        }
                    }
                    else
                    {
                        jogo.Execpostpedido(request);

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
            _ = ProcessRequestAsync(response, responseBody);

        }



    }
    
    static async Task ProcessRequestAsync(HttpListenerResponse response, string responseBody)
{

    
    var buffer = Encoding.UTF8.GetBytes(responseBody);
    response.ContentLength64 = buffer.Length;
    await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
    response.OutputStream.Close();
}

} 