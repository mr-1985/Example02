﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TopLearn.Core.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value).ToString() +
                   "/" + pc.GetMonth(value).ToString("00") +
                   "/" + pc.GetDayOfMonth(value).ToString("00");
        }
    }
}
