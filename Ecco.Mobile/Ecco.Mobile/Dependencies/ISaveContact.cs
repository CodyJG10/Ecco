using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.Dependencies
{
    public interface ISaveContact
    {
        void SaveContact(string name, string number, string email);
    }
}