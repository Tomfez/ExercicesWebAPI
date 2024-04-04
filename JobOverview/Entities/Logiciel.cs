namespace JobOverview.Entities
{
    public class Logiciel
    {
        public string Code { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string CodeFiliere { get; set; } = string.Empty;
        public virtual List<Version> Versions { get; set; } = new();
        public virtual List<Module> Modules { get; set; } = new();
    }
}
