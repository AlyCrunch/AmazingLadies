using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.AmazingLadies.Helpers
{
    public static class Utilities
    {
        public static Dictionary<string, string> StringToDictionary(string str)
        {
            var dict = str.Split(';')
                      .Select(s => s.Split('='))
                      .ToDictionary(a => a[0].Trim(), a => a[1].Trim());
            return dict;
        }

        public static string DictionaryToString(Dictionary<string, string> dic)
        {
            return string.Join(";", dic.Select(x => x.Key + "=" + x.Value).ToArray());
        }
    }
}
