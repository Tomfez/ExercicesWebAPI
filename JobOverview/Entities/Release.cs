namespace JobOverview.Entities
{
    public class Release
    {
        public short Numero { get; set; }
        public float NumeroVersion { get; set; }
        public string CodeLogiciel { get; set; } = string.Empty;
        public DateOnly DatePubli { get; set; }
    }
}
