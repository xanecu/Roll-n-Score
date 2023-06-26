using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Diagnostics;

namespace março30
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window//classe criada pelo sistema
    {
        DispatcherTimer T = new DispatcherTimer();
        System.Diagnostics.Stopwatch cronometro = 
                   new System.Diagnostics.Stopwatch();
        JogoDoisDados j; //declaro variavel objeto<
        public MainWindow()
        {
            InitializeComponent();
            j = new JogoDoisDados("Ze Maria", 5000);//instancio a variavel
            //4 - O registo do evento
            j.OnFalencia += J_OnFalencia;
            j.OnPremio += J_OnPremio;
            //j.cumprimenta("Bom dia");
            j[0] = 5;
            j[1] = 3;
            Ver(j);
            T.Interval = new TimeSpan(0,0,1);//hr,min,seg
            T.Tick += T_Tick;
            T.Start();
            cronometro.Restart();

            int bonus = 0;
            j.Montante += bonus.acrescenta(300);
        }

        //handler
        private void T_Tick(object sender, EventArgs e)
        {
            this.relogio.Content = DateTime.Now.ToString();
            if(cronometro.ElapsedMilliseconds >3000)
            {
                T.Stop();
                cronometro.Stop();

                this.rolar.Background = Brushes.Red;
                //this.rolar.IsEnabled = false; serveria para bloquear apos os >3000 milisegundos, mas n apareceria a cor vermelha
            }
        }


        //Event handler, metodo q reage ao evento
        private void J_OnPremio(object sender, OnPremio_Args args)
        {//String builder, como se fosse array de strings
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            JogoDoisDados s = (JogoDoisDados)sender;//unboxing
            int premio = args.aposta * 2 * s.Dado1;
            s.Montante += premio;
            sb.Append(s.Jogador + "\n");
            sb.Append("Parabéns Ganhou " + premio.ToString() + "\n");
            sb.Append("O prémio saiu em " + args.quando.ToString());
            this.visor.Text = sb.ToString();
        }

        //5 - Declarar e implementar o event handler (tratador do evento)
        private void J_OnFalencia(object sender, EventArgs args)
        {
            //MessageBox.Show("Falência");
            this.visor.Text = "Acabou o Jogo - Falência: " + 
                ((JogoDoisDados)sender).Jogador + "  saia!!!!" ;
        }

        protected void Ver(JogoDoisDados jog)
        {
            this.txtjogador.Text = jog.Jogador;
            this.txtmontante.Text = jog.Montante.ToString();
            this.img1.Source = new BitmapImage(new Uri("dados/" 
                + jog.Dado1.ToString() + ".png", UriKind.Relative ));
            this.img2.Source = new BitmapImage(new Uri("dados/"
               + jog.Dado2.ToString() + ".png", UriKind.Relative));

        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.visor.Text = "";
                int aposta;
                if(int.TryParse(txtaposta.Text, out aposta))
                {
                    j.Jogar(aposta);
                    Ver(j);
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        protected void ver()
        {
            if (j != null)
            {
                this.txtmontante.Text = j.Montante.ToString();
                this.txtjogador.Text = j.Jogador;
            }
        }

        private void btnovo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtjogador.Text))
                {
                    j = new JogoDoisDados();

                    Ver(j);
                }
                else
                {
                    j = new JogoDoisDados(txtjogador.Text, int.Parse(txtmontante.Text));
                    Ver(j);
                }
                j.OnFalencia += J_OnFalencia;
                j.OnPremio += J_OnPremio;
                //j.OnPremio -= J_OnPremio; Desligar o evento

                ver();
            }
            catch (Exception)
            {
                MessageBox.Show("Erro");

            }


        }


        private void Rolarfichas_Click(object sender, RoutedEventArgs e)
        {
            j.Jogar(new FICHAS[] { FICHAS.RED, FICHAS.RED, FICHAS.GOLD, FICHAS.SILVER });
            Ver(j);
        }

        private void Comprarfichas_Click(object sender, RoutedEventArgs e)
        {
            j.ComprarFichas(new FICHAS[] { FICHAS.SILVER, FICHAS.GOLD });
            Ver(j);
        }
    }
}
