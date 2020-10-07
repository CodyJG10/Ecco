using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Api.Util
{
    public static class UserToContainer
    {
        public static string EmailToContainer(string email)
        {
            return  "username-" + email.Replace("@", "-").Replace(".", "-");
        }
    }
}
