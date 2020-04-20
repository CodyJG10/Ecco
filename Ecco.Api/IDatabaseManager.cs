using Ecco.Entities;
using Ecco.Entities.Company;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecco.Api
{
    public interface IDatabaseManager
    {

        #region Identity

        void SetUrl(string url);
        Task<bool> Login(string username, string password);
        Task<bool> Register(string username, string email, string password, string confirmPassword);
        Task<UserData> GetUserData();
        Task<UserData> GetUserData(Guid id);
        Task<UserData> GetUserData(string profileName);
        Task<bool> UserExists(string userName);
        void ForgotPassword(string email);
        Task<UserData> GetUserDataByEmail(string email);

        #endregion

        #region Cards

        Task<bool> CreateCard(Card card);
        Task<IEnumerable<Card>> GetCards();
        Task<Card> GetCard(int id);
        Task<IEnumerable<Card>> GetMyCards(string id);
        Task<bool> EditCard(Card card);
        Task<bool> DeleteCard(Card card);

        #endregion

        #region Connections

        Task<IEnumerable<Connection>> GetMyConnections(Guid id);
        Task<IEnumerable<Connection>> GetMyPendingConnections(Guid id);
        Task<bool> CreateConnection(Guid id, Guid toId, int cardId);
        Task<bool> AcceptConnection(Connection connection);
        Task<bool> DeleteConnection(Connection connection);
        Task<Connection> GetConnection(int id);

        #endregion

        #region Templates

        Task<List<Template>> GetTemplates();
        Task<Template> GetTemplate(int id);

        #endregion

        #region Company

        Task<bool> CreateCompany(Company copmany);
        Task<bool> InviteEmployee(EmployeeInvitation invitation);
        Task<Company> GetCompany(int id);
        Task<Company> GetMyOwnedCompany(Guid id);
        Task<List<Company>> GetMyEmployers(string userId);
        Task<bool> LeaveCompany(Company company, Guid userId);
        Task<bool> AcceptEmployeeInvitation(Guid userId, int companyId);
        Task<bool> DenyEmployeeInvitation(Guid userId, int companyId);
        Task<List<EmployeeInvitation>> GetMyPendingEmployeeInvites(Guid userId);

        #endregion
    }
}