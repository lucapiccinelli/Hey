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
                Veicoli_Note nota = dataContext.Veicoli_Notes.Single(note => note.IdNota == _idNota);
                return nota.Nota;
            }
        }
    }
}