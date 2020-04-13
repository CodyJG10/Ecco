using System;
using System.Linq;
using Ecco.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ecco.Tests
{
    [TestClass]
    public class ApiTests
    {
        #region Fields
        private readonly DatabaseManager _db;
        private const string id = "baf2f30a-671b-4fa7-91c8-837cb4e1e770";
        #endregion

        #region Init
        
        public ApiTests()
        {
            _db = new DatabaseManager();
            _db.SetUrl("https://ecco-space.azurewebsites.net");
            Login();
        }

        private void Login()
        {
            string username = "codyjg10@gmail.com";
            string password = "Airplane10";
            _db.Login(username, password).GetAwaiter().GetResult();
        }

        #endregion

        #region Cards

        [TestMethod]
        public void GetCards()
        {
            var cards = _db.GetCards().GetAwaiter().GetResult();
            Assert.IsNotNull(cards, "Retrieved " + cards.ToList().Count + " cards.");
        }

        [TestMethod]
        public void GetMyCards()
        {
            var cards = _db.GetMyCards(id).GetAwaiter().GetResult();
            Assert.IsNotNull(cards, "Retrieved " + cards.ToList().Count + " cards.");
        }

        #endregion

        #region Connections

        [TestMethod]
        public void GetMyConnections()
        {
            var connections = _db.GetMyConnections(new Guid(id)).GetAwaiter().GetResult().ToList();
            Assert.IsNotNull(connections, "Retrieved " + connections.Count + " results");
        }

        [TestMethod]
        public void GetMyPendingConnections()
        {
            var connections = _db.GetMyPendingConnections(new Guid(id)).GetAwaiter().GetResult().ToList();
            Assert.IsNotNull(connections, "Retrieved " + connections.Count + " results");
        }

        #endregion

        #region Templates

        [TestMethod]
        public void GetTemplates()
        {
            var templates = _db.GetTemplates().GetAwaiter().GetResult();
            Assert.IsNotNull(templates, "Retrieved " + templates.Count + " results");
        }

        #endregion
    }
}