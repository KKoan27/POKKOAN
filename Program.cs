using System;
using System.Text.Encodings.Web; // Para JavaScriptEncoder
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text;
using System.IO;
using GetJogos;
using jogos;

class Program
{

    static string[] arrei = new string[2];


    static void Main()
    {
        Server server = new Server();
        server.runServer("http://localhost:8080/");
    }
}




class Server
{

    public string requestBody;
    public string responseBody;
    HttpListener listener;
    HttpListenerContext context;
    HttpListenerRequest request;
    HttpListenerResponse response;

    getJogos connsql = new getJogos();

    public string busca;
    // Adicione esta propriedade/field na sua classe
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = false,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        // Adicione esta linha para números decimais:
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.WriteAsString,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };






    public void runServer(string porta)
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
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type");



            if (request.HttpMethod == "GET")
            {
                Console.WriteLine("foi executado um GET");
                var busca = request.QueryString["busca"];
                responseBody = connsql.GetJogosAsJson(busca);
            }
            else if (request.HttpMethod == "POST")
            {


                using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {



                }

                responseBody = "deu POST po, nada pra retornar";
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