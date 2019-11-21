using System;
using System.Collections.Generic;

namespace AZNPano.DBModel
{
    public class GehaltFaktorModel
    {
        //3 Instanzen mit 5 Elementen in der Liste
        public GehaltFaktorModel(List<(double Faktor, DateTime Start, DateTime End)> faktoren, ESchichtGehaltsfaktoren key)
        {
            Key = key;
        }

        public ESchichtGehaltsfaktoren Key 
        { 
            get;
        }

        public double GetFaktor(DateTime dateTime) {
            throw new NotImplementedException();
        }
    }

    public class GehaltFaktorModel2
    {

        //15 instanzen
        public GehaltFaktorModel2(double faktor, DateTime start, DateTime end, ESchichtGehaltsfaktoren key)
        {
            Faktor = faktor;
            Start = start;
            End = end;
            Key = key;
        }

        public double Faktor { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public ESchichtGehaltsfaktoren Key { get; }
    }
}