using System;

namespace Elsa.entities.cache
{
    public static class CacheKeys
    {
        // System info will be collect 1 time per day by request to get infor from Controller of API
        // API also has memory cached and will be fill 1 time per day. 
        // -> request from user will be responsed from controller of Website, not Controller of API

        public static string SystemInfo { get { return "_SystemInfo"; } }

        public static string Users { get { return "_Users"; } }
        
    }
}
