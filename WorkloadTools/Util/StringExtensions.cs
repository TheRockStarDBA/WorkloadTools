﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkloadTools.Util
{
    public static class StringExtensions
    {
        public static string Right(this string value, int count)
        {
            //Check if the value is valid
            if (string.IsNullOrEmpty(value))
            {
                //Set valid empty string as string could be null
                value = string.Empty;
            }
            else if (value.Length > count)
            {
                //Make the string no longer than the max length
                value = value.Substring(value.Length - count, count);
            }

            //Return the string
            return value;
        }
    }
}
