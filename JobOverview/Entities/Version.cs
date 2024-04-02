namespace JobOverview.Entities
{
    public class Version
    {
        public float Numero { get; set; }
        public short Millesime { get; set; }
        public string CodeLogiciel { get; set; } = string.Empty;
        public DateOnly DateOuverture { get; set; }
        public DateOnly DateSortiePrevue { get; set; }
        public DateOnly? DateSortieReelle { get; set; }
        public virtual List<Release> Releases { get; set; } = new();
    }
}
