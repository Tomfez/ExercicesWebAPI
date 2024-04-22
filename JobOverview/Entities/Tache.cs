namespace JobOverview.Entities
{
    public class Tache
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public decimal DureePrevue { get; set; }
        public decimal DureeRestante { get; set; }
        public string CodeActivite { get; set; } = string.Empty;
        public string Personne { get; set; } = string.Empty;
        public string CodeLogiciel { get; set; } = string.Empty;
        public string CodeModule { get; set; } = string.Empty;
        public float NumVersion { get;set; }
        public string? Description { get; set; }
        // Jeton d'accès concurrentiel
        public Guid Vers { get; set; }
        public List<Travail> Travaux { get; set; } = new();
    }
}
