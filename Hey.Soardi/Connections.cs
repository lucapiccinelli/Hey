using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hey.Soardi
{
    public static class Connections
    {
        public static readonly string CarrozzeriaDarfo;
        public static readonly string CarrozzeriaCosta;
        public static readonly Dictionary<string, string> Strings;

        static Connections()
        {
            CarrozzeriaDarfo = ConfigurationManager.ConnectionStrings["Carrozzeria2017"].ConnectionString;
            CarrozzeriaCosta = ConfigurationManager.ConnectionStrings["CarrozzeriaCosta"].ConnectionString;
            Strings = new Dictionary<string, string>
            {
                {"Darfo", CarrozzeriaDarfo},
                {"Costa", CarrozzeriaCosta}
            };

            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                Strings.Add(connectionString.Name, connectionString.ConnectionString);
            }
        }
    }
}
