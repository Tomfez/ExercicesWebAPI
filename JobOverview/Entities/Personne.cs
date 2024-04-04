namespace JobOverview.Entities
{
    public class Personne
    {
        public string Pseudo { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public float TauxProductivite { get; set; }
        public string CodeEquipe { get; set; } = string.Empty;
        public string CodeMetier { get; set; } = string.Empty;
        public string? Manager { get; set; }
        public virtual Metier? Metier { get; set; }
    }
}
