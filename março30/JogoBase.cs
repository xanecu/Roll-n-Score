using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace março30
{
    //Enum: lista de ctes do tipo int
    public enum FICHAS : int //variavel (tem de ser discreta)
    {//lista de ctes
        RED = 10, //se nao desse valor seria 0,1,2...
        GREEN = 20,
        BLUE = 30,
        SILVER = 50,
        GOLD = 100

    }
    //delegate:apontador de metodos/os eventos sao metodos void
    //delegate tem o mesmo nivel duma classe
    //Passos para implementar o evento:

    //1 - Declarar o delegate para tipificar o evento
    public delegate void OnFalencia_Handler(Object sender, EventArgs args);

    //interface permite criar regras para eventos, delegates ou propriedades
    //
    interface IJogo
    {
        event OnFalencia_Handler OnFalencia;
        event OnPremio_Handler OnPremio;

        //dentro posso por propriedades
        //interface pode levar assinaturas de properties,
        //nao pode levar variaveis,
        //assinaturas de metodos e de delegates
        int Dado1 { get; set; }
        int Dado2 { get; set; }
        //overloading, defenir duas vezes um metodo com msm nome
        //mas com assinatura diferente, prohibido
        void Jogar(int aposta);
        void Jogar(FICHAS[] fichas);

    }
    public abstract class JogoBase //declaramos public para q a mainwindow tenho acesso ao nosso jgo
    {
        //metodo abstact é como o interface, nao tem corpo
        //a consequencia de por abstract numa classe é q a classe tem de ser tmb abstract
        //a consequencia tambem é q nao pode ser instanciada
        //por isso é q deu erro, por estar a instanciar uma classe derivada
        public abstract void ComprarFichas(FICHAS[] fichas);


        public const int MAXAPOSTA = 500;
        //coloca cte em maiuscula para diferenciar

        //Overloading : defenir 2  ou + métodos com o mesmo nome na msm classe mas lista de parametros diferentes
        public JogoBase()//construtor1, permitem inicializaçao
        {
            this._jogador = "Desconhhecido";
            this._montante = 0;
        }

        //Override = a classe derivada ter a possibility de reescrever
        //metodos herdados - esmagando os metodos

        //so ha dois metodos que podem ser override, os abstratos e os virtuais
        //abstracto e virtual sao as clausulas que permitem override


        public virtual void cumprimenta(String msg)
        {
            System.Windows.MessageBox.Show(msg);
        }

        public JogoBase(String jogador, int montante)//construtor2
        {
            this._jogador = jogador;
            this.Montante = montante;

        }

        protected String _jogador; //campo privado, scope visivel so na classe onde e criado e na derivada
        //mas nao é visivel para o cliente

        public String Jogador //property de leitura ja q tiramos o set
        //o cliente n pode atualizar o nome do jogador
        {//property com metodo "get"
            get { return _jogador; }
            //set { this._jogador = value; }
        }

        protected int _montante; //campo privado/protegido

        public int Montante //property que so ira atualizar se o valor for igual ou maior q zero
        {
            get { return _montante; }//logica para acesso de leitura

            set
            {
                if (value >= 0) this._montante = value;
            }
            //é atraves do set que o cliente atualiza o campo
            //logica para acesso a escritura
        }
    }

    //Isto é classe de argumento
    public class OnPremio_Args:EventArgs
    {
        public int aposta;
        public DateTime quando;
        public OnPremio_Args(int ap)
        {
            this.aposta = ap;
            quando = DateTime.Now;
        }
    }

    //Para disparar evento Onpremio/1.Fazer o delegate
    public delegate void OnPremio_Handler(object sender, OnPremio_Args args);


    //classe derivada da classe mae "JogoBase" e implementa o interface IJogo
    public class JogoDoisDados : JogoBase, IJogo
    {

        public override void ComprarFichas(FICHAS[] fichas)
        {
            foreach (FICHAS f in fichas) Montante += (int)f;
        }


        public override void cumprimenta(string msg)
        {
            //base.cumprimenta(msg);
            String m = "Srº " + this.Jogador + ": " + msg;
            System.Windows.MessageBox.Show(m);
        }


        //especializamos introduzindo um array privado de 2 inteiros
        int[] _dados = new int[2] { 1, 2 };


        //dois construtores que unicamente chamam a classe mae
        //"JogoDoisDados
        public JogoDoisDados() : base()//construtor1
        {

        }
        public JogoDoisDados(String jogador, int montante) : base(jogador, montante)//construtor2
        {   //gera inicialmente random dados ao abrir a App
            Random r = new Random();
            Dado1 = r.Next(1, 7);
            Dado2 = r.Next(1, 7);
        }

        //property indice (i)
        public int this[int i]
        {
            get
            {
                switch (i)
                {
                    case (0):
                        return _dados[0];
                    case (1):
                        return _dados[1];
                    default:
                        throw new Exception("Dado invalido");


                }
            }
            
            set
            {
                if (value >= 1 && value <= 6)
                {
                    switch (i)
                    {
                        case (0):
                            _dados[0] = value;
                            break;
                        case (1):
                            _dados[1] = value;
                            break;
                        default:
                            throw new Exception("Dado invalido");
                    }
                }
            }
        }

        //2 propriedades que vem do interface q vao aplicar regras
        //em cada um dos dados
        public int Dado1
        {
            get { return _dados[0]; }//1ªposiçao do array;
            set { if (value >= 1 && value <= 6) this._dados[0] = value; }
        }
        public int Dado2
        {
            get { return _dados[1]; }//2ª posiçao do array
            set { if (value >= 1 && value <= 6) this._dados[1] = value; }
        }

        //2 - Declarar o evento
        public event OnFalencia_Handler OnFalencia;
        //um evento é um delegate, apontador de metodos
        //o botao tem definido um delegate que a assinatura do metodo vai reagir no cliente
        //e esse metodo tem o nome de event handler

        //event onpremio/2.Declarar o evento na classe
        public event OnPremio_Handler OnPremio;

        public void Jogar(int aposta)
        {            
            if (Montante >= aposta)
            {
                Montante -= aposta;
                //3 - Disparar o evento
                if (Montante == 0 && OnFalencia != null) OnFalencia(this, new EventArgs());
            }
            else if (Montante > 0)
            {
                aposta = Montante;
                Montante = 0;
                if (OnFalencia != null) OnFalencia(this, new EventArgs());
            }
            else aposta = 0;

            if (aposta > 0)
            {   //se tudo OK, num aleatorio entre 1 e 6 nos dados
                Random r = new Random();
                Dado1 = r.Next(1, 7);
                Dado2 = r.Next(1, 7);
                //3.disparar o evento onPremio
                if (Dado1 == Dado2)
                {
                    if (OnPremio != null) OnPremio(this,new OnPremio_Args(aposta));

                }
            }
        }

        public void Jogar(FICHAS[] fichas)
        {
            int aposta = 0;
            foreach (FICHAS f in fichas) aposta += (int)f;
            this.Jogar(aposta);
        }
    }

}


