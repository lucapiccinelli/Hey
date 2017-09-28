using System.Linq;
using Hey.Core;
using Hey.Soardi.Model;

namespace Hey.Soardi
{
    public class NoteVeicoloMessageProvider : IMessageProvider
    {
        private readonly int _idNota;

        public NoteVeicoloMessageProvider(int idNota)
        {
            _idNota = idNota;
        }

        public string GetText()
        {
            using (CarrozzeriaDataContext dataContext = new CarrozzeriaDataContext())
            {
                Veicoli_Note nota = GetNota(dataContext);
                return nota.Nota;
            }
        }

        public string GetAbstract()
        {
            using (CarrozzeriaDataContext dataContext = new CarrozzeriaDataContext())
            {
                Veicoli_Note nota = GetNota(dataContext);
                return $"Promemoria della pratica numero {nota.Veicoli.numPreventivo}";
            }
        }

        private Veicoli_Note GetNota(CarrozzeriaDataContext dataContext)
        {
            return dataContext.Veicoli_Notes.Single(note => note.IdNota == _idNota);
        }
    }
}