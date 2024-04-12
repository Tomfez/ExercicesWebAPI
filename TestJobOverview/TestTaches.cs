using JobOverview.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestJobOverview
{
    [TestClass]
    public class TestTaches
    {
        private IServiceTaches _serviceTaches = null!;
        private Tache _tache = null!;

        [TestInitialize]
        public void InitializeTests()
        {
            _serviceTaches = new ServiceTaches(new ContexteJobOverview(TestInit.ContextOptions));

            _tache = new Tache()
            {
                Id = 1,
                Titre = "Tache de test",
                DureePrevue = 2,
                DureeRestante = 2,
                CodeActivite = "DEV",
                Personne = "RBEAUMONT",
                CodeLogiciel = "GENOMICA",
                CodeModule = "SEQUENCAGE",
                NumVersion = 1,
                Description = "",
                Travaux = null!
            };
        }

        [DataTestMethod]
        [DataRow("RBEAUMONT", "GENOMICA", 1, 8)]
        [DataRow("MWEBER", null, 2, 6)]
        [DataRow(null, "ANATOMIA", null, 5)]
        public async Task ObtenirTaches(string? personne, string? logiciel, float? version, int nbLignes)
        {
            List<Tache>? taches = await _serviceTaches.GetTaches(personne, logiciel, version);

            Assert.AreEqual(nbLignes, taches.Count);
        }

        [TestMethod]
        public async Task ObtenirTache()
        {
            Tache? tache = await _serviceTaches.GetTache(2);

            Assert.IsNotNull(tache);
            Assert.AreEqual("RBEAUMONT", tache.Personne);
        }

        [TestMethod]
        public async Task ObtenirPersonne()
        {
            Personne? personne = await _serviceTaches.GetPersonne("RBEAUMONT");

            Assert.IsNotNull(personne);
            Assert.AreEqual("BNORMAND", personne.Manager);
        }

        [TestMethod]
        public async Task AjouterTache()
        {
            var tache = new Tache()
            {
                Titre = "Tache ajoutée en test",
                DureePrevue = 4,
                DureeRestante = 2,
                CodeActivite = "DEV",
                Personne = "RBEAUMONT",
                CodeLogiciel = "ANATOMIA",
                CodeModule = "MICRO",
                NumVersion = 6,
                Description = "tache qui vient du test",
                Travaux = null!
            };

            await _serviceTaches.PutPostTache(tache);

            Assert.IsTrue(tache.Id > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationRulesException))]
        public async Task AjouterTachePersonneInconnue()
        {
            _tache.Personne = "TOTO";
            await _serviceTaches.PostTache(_tache);
        }

        [DataTestMethod]
        [DataRow("RBEAUMONT", "GENOMICA", 1, 8)]
        [DataRow("MWEBER", null, 6, 6)]
        [DataRow(null, "ANATOMIA", null, 6)]
        public async Task SupprimerTache(string? personne, string? logiciel, float? version, int expected)
        {
            int nbSupr = await _serviceTaches.DeleteTaches(personne, logiciel, version);

            Assert.AreEqual(0, nbSupr);
        }
    }
}