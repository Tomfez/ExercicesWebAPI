namespace JobOverview.Entities
{
    public class Metier
    {
        public string Code { get; set; } = string.Empty;
        public string Titre { get; set; } = string.Empty;
        public string CodeService { get; set; } = string.Empty;
        public virtual List<Activite> Activites { get; set; } = new();
    }
}
