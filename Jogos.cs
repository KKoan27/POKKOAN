

using System;
using System.IO;
using System.Text;



namespace jogos{
class Jogos{
    

    private int id{get;set;}
    public string nome{ get;set;}
    public double valor {get; private set;} 

    public string descricao {get;set;}





    public Jogos(string nome, double valor, string descricao ){
        this.nome = nome;
        this.valor = valor;
        this.descricao = descricao;

    }

}
}