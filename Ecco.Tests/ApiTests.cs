using System;
using Ecco.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ecco.Tests
{
    [TestClass]
    public class ApiTests
    {
        private DatabaseManager _db;

        [TestMethod]
        public async void TestLogin()
        {
            string username = "codyjg10@gmail.com";
            string password = "Airplane10";
            string url = "https://localhost:44355";
            _db = new DatabaseManager();
            bool loginResult = await _db.Login(username, password);
            string message = loginResult ? "Login Succeeded" : "Login Failed";
            Assert.IsTrue(loginResult, message);
        }

        [TestMethod]
        public async void TestCards()
        {
            var cards = await _db.GetCards();
            Assert.IsNotNull(cards);
        }
    }
}