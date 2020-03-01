using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrologMVC
{
    public static class SessionData
    {
        public static string DataSession;
        private static List<string> listData = new List<string>() ;

        public static void setDataSession(string x)
        {
            DataSession = x;
        }
        public static string getDataSession()
        {
            return DataSession;
        }
        public static void setDataList(List<string> ts)
        {
            listData = ts;
        }
        public static List<string> getDatalist()
        {
            return listData;
        }
    }                                
}
