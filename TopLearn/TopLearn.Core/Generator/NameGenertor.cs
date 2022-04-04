using System;
using System.Collections.Generic;
using System.Text;

namespace TopLearn.Core.Generator
{
    public class NameGenertor
    {
        public static string GenerateUniqCode()
        {
            return Guid.NewGuid().ToString().Replace("-","");
        }
    }
}
