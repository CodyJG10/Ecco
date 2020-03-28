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
        Task<UserData> GetUserData();

        Task<bool> CreateCard(Card card);
        Task<IEnumerable<Card>> GetCards();

        Task<IEnumerable<Connection>> GetMyConnections(Guid id);
        Task<IEnumerable<Connection>> GetMyPendingConnections(Guid id);
        Task<bool> CreateConnection(Guid id, Guid toId);
        Task<bool> AcceptConnection(Connection connection);
        Task<bool> DeleteConnection(Connection connection);
    }
}