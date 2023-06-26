using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace março30
{


    //Método Extensão: serve para acrescentar codigo a uma classe sealed // sealed: classe q n pode ser derivada,n tnho acesso ao codigo
    //obrigatorio q este seja "static"
    //1º argumento é "this"
    public static class contentor
    {
        public static int acrescenta(this int i, int delta)
        {
            return i + delta;
        }
    }
    class utils
    {
    }
}
