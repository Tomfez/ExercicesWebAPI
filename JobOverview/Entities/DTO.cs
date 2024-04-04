namespace JobOverview.Entities
{
    public class FormRelease
    {
        public short Numero { get; set; }
        public float NumeroVersion { get; set; }
        public string CodeLogiciel { get; set; } = string.Empty;
        public DateOnly DatePubli { get; set; }
        public IFormFile? Notes { get; set; }
    }
}
