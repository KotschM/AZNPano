using System;
using System.Collections.Generic;

namespace AZNPano.DBModel
{
    public class GehaltModel
    {
        List<(double Betrag, DateTime Start, DateTime End)> Gehalt = new List<(double Betrag, DateTime Start, DateTime End)>();
        //3 Instanzen mit 5 Elementen in der Liste
        public GehaltModel(List<(double Betrag, DateTime Start, DateTime End)> faktoren)
        {
            this.Gehalt = faktoren;
        }

        public void AddBetragWithDuration(double Betrag, DateTime Start, DateTime End){
            Gehalt.Add((Betrag, Start, End));
        }

        public double GetBetrag(DateTime time){
            double res = 0.0;
            foreach (var item in Gehalt)
            {
                if ((item.Start <= time) && (time <= item.End))
                {
                    res = item.Betrag;
                }
            }
            return res;
        }
    }
}