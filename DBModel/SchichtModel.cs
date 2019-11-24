using System;
using System.Collections.Generic;

namespace AZNPano.DBModel
{
    public class SchichtModel
    {
        private readonly EMonat Monat;
        private readonly int Jahr;
        List<(ESchichtTyp Typ, ESchichtBereich Bereich, DateTime Beginn, DateTime Ende)> Schicht = new List<(ESchichtTyp Typ, ESchichtBereich Bereich, DateTime Beginn, DateTime Ende)>();
        
        public SchichtModel(List<(ESchichtTyp Typ, ESchichtBereich Bereich, DateTime Beginn, DateTime Ende)> faktoren, EMonat Monat, int Jahr)
        {
            this.Monat = Monat;
            this.Jahr = Jahr;
            this.Schicht = faktoren;
        }
        public void AddSchichtWithInfos(ESchichtTyp Typ, ESchichtBereich Bereich, DateTime Beginn, DateTime Ende){
            Schicht.Add((Typ, Bereich, Beginn, Ende));
        }
    }
}