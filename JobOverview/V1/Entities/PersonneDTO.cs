using JobOverview.Entities;

namespace JobOverview.V1.Entities
{
    public class PersonneDTO
    {
        public string Pseudo { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public decimal TauxProductivite { get; set; }

        public string CodeEquipe { get; set; } = string.Empty;
        public string CodeMetier { get; set; } = string.Empty;
        public string? Manager { get; set; }

        // Propriétés de navigation
        public virtual Metier Métier { get; set; } = new();
    }
}
