using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
{
    public static class ValidationExt
    {
        public static bool vIsNull<T>(this T v)
        {
            return v == null;
        }
        public static bool vIsEmpty<T>(this T v)
        {
            if (v == null) {
                return false;
            } else if (v is string str) {
                return string.IsNullOrEmpty(str);
            } else if (v is ICollection<T> collection) {
                return collection.Count == 0;
            }
            return false;
        }
        public static bool vIsNotEmpty<T>(this T v)
        {
            return !v.vIsEmpty();
        }
        public static bool vIsDefault<T>(this T v)
        {
            return EqualityComparer<T>.Default.Equals(v, default(T));
        }
        public static bool vIsBetween(this int v, int lv, int rv)
        {
            return lv <= v && v <= rv;
        }
        public static bool vIsYYYYMMDD(this string v)
        {
            if (v.Length != 8) {
                return false;
            }
            return DateTime.TryParseExact(v, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
