﻿namespace JobOverview.Entities
{
    public class Travail
    {
        public DateOnly DateTravail { get; set; }
        public int IdTache { get; set; }
        public decimal Heures { get; set; }
        public float TauxProductivite { get; set; }
    }
}
