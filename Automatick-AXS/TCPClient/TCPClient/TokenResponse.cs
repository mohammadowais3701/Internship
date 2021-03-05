using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TCPClient
{
    public class TokenResponse
    {
        private String error;
        private String token;
        private String guid;

        public TokenResponse(String error, String token, String guid)
        {
            this.error = error;
            this.token = token;
            this.guid = guid;
        }

        public TokenResponse()
        {
            // TODO: Complete member initialization
        }

        public String Error
        {
            get { return error; }
            set { error = value; }
        }

        public String Token
        {
            get { return token; }
            set { token = value; }
        }

        public String Guid
        {
            get { return guid; }
            set { guid = value; }
        }
    }
}
