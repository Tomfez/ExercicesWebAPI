namespace JobOverview.Entities
{
    public class Module
    {
        public string Code { get; set; } = string.Empty;
        public string CodeLogiciel { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string? CodeModuleParent { get; set; }
        public string? CodeLogicielParent { get; set; }
        public virtual List<Module> SousModules { get; set; } = new();
    }
}
