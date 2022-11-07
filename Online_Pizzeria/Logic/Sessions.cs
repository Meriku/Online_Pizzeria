namespace Online_Pizzeria.Logic
{ 
    public static class Sessions
    {
        private static List<string> ActiveSessions;

        public static bool Login(IConfiguration configuration, string loggin, string pass, out string sessionId)
        {
            ActiveSessions ??= new List<string>();

            sessionId = "";
            var _defaultLogin = configuration.GetValue<string>("Admin:Login");
            var _defaultPass = configuration.GetValue<string>("Admin:Pass");
            if (_defaultLogin.Equals(loggin) && _defaultPass.Equals(pass))
            {
                sessionId = Guid.NewGuid().ToString();
                ActiveSessions.Add(sessionId);
                return true;
            }
            else
            {            
                return false;
            }
        }

        public static bool CheckSessionId(string? sessionId)
        {
            ActiveSessions ??= new List<string>();

            return sessionId != null && ActiveSessions.Contains(sessionId);
        }

    }
}
