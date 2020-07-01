using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Helpers
{
    public class VSException: Exception
    {
        public VSException(): base()
        {

        }

        public VSException(string message) : base(message) { }

        public VSException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }

        public int Status { get; set; } = 500;

        public object Value { get; set; }

    }
}
