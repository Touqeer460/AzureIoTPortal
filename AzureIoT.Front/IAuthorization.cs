using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureIoT.FrontEnd
{
    public class AzureAuthorization : IAuthorization
    {
        public static Authorization AuthToken
        {
            get;
            private set;
        }
        public Authorization Authorize()
        {
            throw new NotImplementedException();
        }
    }
    public interface IAuthorization
    {
        Authorization Authorize();
    }

    public class User
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

    public class Authorization
    {
        public bool Authorized { get; set; }
        public string AuthToken { get; set; }
    }
}