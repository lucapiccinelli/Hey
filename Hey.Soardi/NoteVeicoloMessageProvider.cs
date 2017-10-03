using System;
using System.Linq;
using System.Text.RegularExpressions;
using Hey.Core;
using Hey.Soardi.Model;

namespace Hey.Soardi
{
    public class NoteVeicoloMessageProvider : IMessageProvider
    {
        private readonly int _idNota;
        private readonly string _connection;

        public NoteVeicoloMessageProvider(int idNota)
            : this(idNota, string.Empty)
        {
        }

        public NoteVeicoloMessageProvider(int idNota, string connection)
        {
            _idNota = idNota;
            _connection = connection;
        }

        public string GetText()
        {
            using (CarrozzeriaDataContext dataContext = GetDataContext())
            {
                Veicoli_Note nota = GetNota(dataContext);
                return nota.Nota;
            }
        }

        public string GetAbstract()
        {
            using (CarrozzeriaDataContext dataContext = GetDataContext())
            {
                Veicoli_Note nota = GetNota(dataContext);
                return $"Promemoria della pratica numero {nota.Veicoli.numPreventivo}";
            }
        }

        private CarrozzeriaDataContext GetDataContext()
        {
            return _connection == String.Empty
                ? new CarrozzeriaDataContext()
                : new CarrozzeriaDataContext(_connection);
        }

        private Veicoli_Note GetNota(CarrozzeriaDataContext dataContext)
        {
            return dataContext.Veicoli_Notes.Single(note => note.IdNota == _idNota);
        }
    }
}