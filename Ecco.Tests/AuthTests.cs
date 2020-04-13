using Ecco.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecco.Tests
{
    [TestClass]
    public class AuthTests
    {
        private IDatabaseManager _db;

        public AuthTests()
        {
            _db = new DatabaseManager();
            _db.SetUrl("https://ecco-space.azurewebsites.net");
        }

        [TestMethod]
        public void LoginWithValidCredentials()
        {
            string username = "codyjg10@gmail.com";
            string password = "Airplane10";
            bool loginResult = _db.Login(username, password).GetAwaiter().GetResult();
            string message = loginResult ? "Login Succeeded" : "Login Failed";
            Assert.IsTrue(loginResult, message);
        }

        [TestMethod]
        public void LoginWithInvalidCredentials()
        {
            string username = "codyjg10@gmail.com";
            string password = "NotMyPassword";
            _db = new DatabaseManager();
            _db.SetUrl("https://ecco-space.azurewebsites.net");
            bool loginResult = _db.Login(username, password).GetAwaiter().GetResult();
            string message = loginResult ? "Login Succeeded" : "Login Failed";
            Assert.IsFalse(loginResult, message);
        }
    }
}
