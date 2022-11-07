namespace Online_Pizzeria.Logic
{ 
    public static class Sessions
    {
        private static List<Session> ActiveSessions;

        public static bool Login(IConfiguration configuration, string loggin, string pass, out string sessionId)
        {
            ActiveSessions ??= new List<Session>();

            sessionId = "";
            var _defaultLogin = configuration.GetValue<string>("Admin:Login");
            var _defaultPass = configuration.GetValue<string>("Admin:Pass");
            if (_defaultLogin.Equals(loggin) && _defaultPass.Equals(pass))
            {
                sessionId = Guid.NewGuid().ToString();
                ActiveSessions.Add(new Session(sessionId));
                return true;
            }
            else
            {            
                return false;
            }
        }

        public static bool CheckSessionId(string? sessionId)
        {
            ActiveSessions ??= new List<Session>();
            if (sessionId == null)
            { return false; }

            var session = ActiveSessions.FirstOrDefault(s => s.CheckSessionId(sessionId));
            if (session == null)
            { return false; }

            if (session.CheckSessionTime())
            {
                return true;
            }
            else
            {
                ActiveSessions.Remove(session);
                return false;
            }
        }
    }

    public class Session 
    { 
        private string _sessionId;
        private DateTime _createTime;
        public Session(string sessionId)
        {
            _sessionId = sessionId;
            _createTime = DateTime.Now;
        }
        public bool CheckSessionId(string session)
        {
            return _sessionId.Equals(session);
        }
        public bool CheckSessionTime()
        {
            return _createTime.AddMinutes(5) >= DateTime.Now;
        }
    }
}
