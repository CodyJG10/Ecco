using Ecco.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Ecco.Api
{
    public interface IStorageManager
    {
        #region Templates

        MemoryStream GetTemplate(string file);

        #endregion

        #region Cards

        MemoryStream GetCard(string username, string file);
        Task<Task> SaveCard(string cardTitle, Stream file, string username);

        #endregion
    }
}