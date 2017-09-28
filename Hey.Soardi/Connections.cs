using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hey.Soardi
{
    static class Connections
    {
        public static readonly string Carrozzeria;

        static Connections()
        {
            Carrozzeria = ConfigurationManager.ConnectionStrings["Carrozzeria2017"].ConnectionString;
        }
    }
}
