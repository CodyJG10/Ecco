using Ecco.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Api
{
    public interface IDatabaseManager
    {
        void SetUrl(string url);
        Task<bool> Login(string username, string password);
        Task<RegisterResult> Register(string username, string password, string confirmPassword);
        Task<IEnumerable<Card>> GetCards();
        Task<IEnumerable<Connection>> GetConnections(Guid id);
        Task<UserData> GetUserData();
    }
}