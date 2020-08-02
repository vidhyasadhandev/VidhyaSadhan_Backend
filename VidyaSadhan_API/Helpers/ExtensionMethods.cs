using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Helpers
{
    public static class ExtensionMethods
    {
        public static bool IsNullOrWhiteSpace(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return true;
            }
            return false;
        }
    }
}
