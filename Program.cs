using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text;



class Program{

    static string[] arrei = new string [2];


    static void Main(){
        Server server = new Server();
        server.runServer("http://localhost:8080/");
    }
}




class Server{
    HttpListener listener;
    HttpListenerContext context;
    HttpListenerRequest request;
    HttpListenerResponse response;


    



    public void runServer(string porta){

        // Instanciando, inserindo os prefixes e iniciando servidor
        listener = new HttpListener();
        listener.Prefixes.Add(porta);
        listener.Start();


        while(true){
            // aguardando a requisição e coletando a response e request
            context = listener.GetContext();
            System.Console.WriteLine("requisição chegou");
            System.Console.WriteLine(context.User);
            request = context.Request;
            response = context.Response;

            if(request.HttpMethod == "get"){
                System.Console.WriteLine("foi executado um GET");
            }
            
            string responseString = JsonSerializer.Serialize(new {message = "Ola mundo"});
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

        
        }
        


    }

}