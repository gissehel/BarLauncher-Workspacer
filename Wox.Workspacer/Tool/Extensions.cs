using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wox.Workspacer.Tool
{
    public static class Extensions
    {
        public static string FormatWith(this string self, params object[] args) => string.Format(self, args);
    }
}