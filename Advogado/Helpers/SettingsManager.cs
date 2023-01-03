using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Advogado.Helpers
{
    public class SettingsManager
    {
        private static NameValueCollection Settings => ConfigurationManager.AppSettings;
        public static string EmailHost => Settings["EmailHost"];
        public static string EmailSenha => Settings["EmailSenha"];
        public static int EmailPorta => int.Parse(Settings["EmailPorta"] ?? "587");
        public static string EmailRemetente => Settings["EmailRemetente"];
        public static bool EmailSeguro => Convert.ToBoolean(Settings["EmailSeguro"]);
    }
}