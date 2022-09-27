using System;

namespace TargetConfiguration
{
    public static class Targets
    {
        //public const string WebServer = "http://Prototyping-ALB-610553215.ap-northeast-2.elb.amazonaws.com:50405/";
        public const string WebServer = "http://localhost:50405/";

        public const string BattleServerIP = "127.0.0.1";
        public const int BattleServerPort = 50404;

        public const uint DBPort = 3306;
        public const string DBServer = "127.0.0.1";
        public const string DBUser = "admin";
        public const string DBPassword = "dhfhak31!#";
        public const string DBCharset = "utf8";
        public const string Database = "accounts";
    }
}
