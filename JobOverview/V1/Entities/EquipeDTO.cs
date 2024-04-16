namespace JobOverview.V1.Entities
{
    public class EquipeDTO
    {
        public string Code { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string CodeService { get; set; } = string.Empty;
        public string CodeFiliere { get; set; } = string.Empty;

        // Propriétés de navigation
        public virtual List<PersonneDTO> Personnes { get; set; } = new();
        public virtual JobOverview.Entities.Service Service { get; set; } = new();
    }
}
