using System.Collections.Generic;
using System.Text;

namespace QtSpy
{
    static class Ext
    {
        public static T TryGet<T>(this Dictionary<string, object> dict, string key)
        {
            object result;
            if (dict == null || !dict.TryGetValue(key, out result))
                return default(T);
            return (T)result;
        }

        public static void TryRemove(this Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key))
                dict.Remove(key);
        }

        public static string ToLiteral(this string input)
        {
            StringBuilder literal = new StringBuilder(input.Length + 2);
            literal.Append("\"");
            foreach (var c in input) {
                switch (c) {
                    case '\"': literal.Append("\\\""); break;
                    case '\\': literal.Append(@"\\"); break;
                    case '\0': literal.Append(@"\0"); break;
                    case '\a': literal.Append(@"\a"); break;
                    case '\b': literal.Append(@"\b"); break;
                    case '\f': literal.Append(@"\f"); break;
                    case '\n': literal.Append(@"\n"); break;
                    case '\r': literal.Append(@"\r"); break;
                    case '\t': literal.Append(@"\t"); break;
                    case '\v': literal.Append(@"\v"); break;
                    default:
                        // ASCII printable character
                        if (c >= 0x20) {
                            literal.Append(c);
                            // As UTF16 escaped character
                        } else {
                            literal.Append(@"\u");
                            literal.Append(((int)c).ToString("x4"));
                        }
                        break;
                }
            }
            literal.Append("\"");
            return literal.ToString();
        }
    }
}
