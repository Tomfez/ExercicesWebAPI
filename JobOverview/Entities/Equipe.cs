namespace JobOverview.Entities
{
    public class Equipe
    {
        public string Code { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string CodeService { get; set; } = string.Empty;
        public string CodeFiliere {  get; set; } = string.Empty;
        public virtual Service Service { get; set; } = null!;
        public virtual List<Personne> Personnes { get; set; } = new();
    }
}
