using System;
using System.Collections.Generic;
using System.Globalization;

namespace weblayer.transportador.core
{
    public static class DateHelper
    {

        /// <summary>
        /// Verifica se a string passada pode ser convertida para data
        /// </summary>
        public static bool CheckDate(String date)
        {
            DateTime Temp;


            if (DateTime.TryParse(date, out Temp) == true)
                return true;
            else
                return false;
        }

        public static string getMesPorExtenso(int mes)
        {
            // Obtém o nome do mês por extenso
            string mesExtenso = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(mes).ToLower();
            // retorna o nome do mês com a primeira letra em maiúscula
            return char.ToUpper(mesExtenso[0]) + mesExtenso.Substring(1);
        }

        public static int GetDaysInMonth(int year, int month)
        {
            DateTime dt1 = new DateTime(year, month, 1);
            DateTime dt2 = dt1.AddMonths(1);
            TimeSpan ts = dt2 - dt1;

            return (int)ts.TotalDays;
        }

        public static DateTime RetornaDiaUtil(DateTime database, int nr_dias, bool foradia, string oper)
        {
            int dt_valida;

            DateTime dt_return = database;

            dt_valida = 0;


            if (foradia)
            {
                if (oper == "+")
                    dt_return = database.AddDays(1);

                if (oper == "-")
                    dt_return = database.AddDays(-1);

            }

            while (nr_dias > dt_valida)
            {
                if (DiaUtil(dt_return))
                    dt_valida = dt_valida + 1;


                if (nr_dias != dt_valida)
                {
                    if (oper == "+")
                        dt_return = dt_return.AddDays(1);

                    if (oper == "-")
                        dt_return = dt_return.AddDays(-1);

                }

            }

            return dt_return;

        }

        private static bool DiaUtil(DateTime data)
        {
            //Caso seja fim de semana, retorna false.
            if ((data.DayOfWeek == DayOfWeek.Sunday) || (data.DayOfWeek == DayOfWeek.Saturday))
            {
                return false;
            }

            //Demais feriados
            //("01/01") -  confraternizacao
            if (data.ToString("dd/MM") == "01/01")
                return false;

            //("21/04") -  tiradentes
            if (data.ToString("dd/MM") == "21/04")
                return false;

            //("01/05") -  dia do trabalho
            if (data.ToString("dd/MM") == "01/05")
                return false;


            //("07/09") -  independencia
            if (data.ToString("dd/MM") == "07/09")
                return false;


            //("12/10") -  Nossa senhora
            if (data.ToString("dd/MM") == "12/10")
                return false;


            //("02/11") -  finados
            if (data.ToString("dd/MM") == "02/11")
                return false;


            //("15/11") -  republica
            if (data.ToString("dd/MM") == "15/11")
                return false;


            //("25/12") -  Natal
            if (data.ToString("dd/MM") == "25/12")
                return false;


            return true;

        }

        //Recebe um valor inteiro em minutos e devolve uma string no formato hora [HH:mm]
        public static string fMinToHour(int n_QtdMinutos)
        {
            string strReturn = "00:00";
            int n_Horas = 0;
            int n_Minutos = 0;

            if (n_QtdMinutos > 0)
            {
                n_Horas = (n_QtdMinutos / 60);
                n_Minutos = (n_QtdMinutos % 60);

                if (n_Horas < 10)
                    strReturn = "0";

                strReturn = strReturn + n_Horas.ToString() + ":";

                if (n_Minutos < 10)
                    strReturn = strReturn + "0";

                strReturn = strReturn + n_Minutos.ToString();

            }

            return strReturn;
        }


        /// <summary>
        /// Returns the first date of the specified year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfYear(int year)
        {
            return new DateTime(year, 1, 1, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the last date of the year provided at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfYear(int year)
        {
            return new DateTime(year, 12, DateTime.DaysInMonth(year, 12), 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the start date of last year with a time of 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfLastYear()
        {
            return GetStartOfYear(DateTime.Now.Year - 1);
        }

        /// <summary>
        /// Returns the last date of last year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfLastYear()
        {
            return GetEndOfYear(DateTime.Now.Year - 1);
        }

        /// <summary>
        /// Returns the first date of the current year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfCurrentYear()
        {
            return GetStartOfYear(DateTime.Now.Year);
        }

        /// <summary>
        /// Returns the last date of the current year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfCurrentYear()
        {
            return GetEndOfYear(DateTime.Now.Year);
        }

        /// <summary>
        /// Returns the first date of next year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfNextYear()
        {
            return GetStartOfYear(DateTime.Now.Year + 1);
        }

        /// <summary>
        /// Returns the last date of next year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfNextYear()
        {
            return GetEndOfYear(DateTime.Now.Year + 1);
        }

        /// <summary>
        /// Returns the number of whole years between two dates
        /// <para>oldDate is subtracted from newDate and the difference returned</para>
        /// </summary>
        public static int GetYearsBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            double years = days / 365;
            return (int)years;
        }

        /// <summary>
        /// Returns the number of whole and fractional years between two dates
        /// <para>The oldDate is subtracted from the new date and the difference returned</para>
        /// </summary>
        public static double GetTotalYearsBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            double years = days / 365;
            return years;
        }

        /// <summary>wl
        /// Returns the first date of the month and year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfMonth(int month, int year)
        {
            return new DateTime(year, month, 1, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the last date of the month and year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfMonth(int month, int year)
        {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the first date of last month's month and year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfLastMonth()
        {
            if (DateTime.Now.Month == 1)
            {
                return GetStartOfMonth(12, DateTime.Now.Year - 1);
            }
            else
            {
                return GetStartOfMonth(DateTime.Now.Month - 1, DateTime.Now.Year);
            }
        }

        /// <summary>
        /// Returns the last date of last month's month and year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfLastMonth()
        {
            if (DateTime.Now.Month == 1)
            {
                return GetEndOfMonth(12, DateTime.Now.Year - 1);
            }
            else
            {
                return GetEndOfMonth(DateTime.Now.Month - 1, DateTime.Now.Year);
            }
        }

        /// <summary>
        /// Returns the first date of current month and year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfCurrentMonth()
        {
            return GetStartOfMonth(DateTime.Now.Month, DateTime.Now.Year);
        }

        /// <summary>
        /// Returns the last date of current month and year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfCurrentMonth()
        {
            return GetEndOfMonth(DateTime.Now.Month, DateTime.Now.Year);
        }

        /// <summary>
        /// Returns the first date of previus month's month and year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfPreviustMonth()
        {
            DateTime data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

            return GetStartOfMonth(data.Month, data.Year);
        }

        public static DateTime GetStartOfPreviustMonth(int Year, int Month)
        {
            DateTime data = new DateTime(Year, Month, 1).AddDays(-1);

            return GetStartOfMonth(data.Month, data.Year);
        }

        /// <summary>
        /// Returns the first date of next month's month and year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfNextMonth()
        {
            if (DateTime.Now.Month == 12)
            {
                return GetStartOfMonth(1, DateTime.Now.Year + 1);
            }
            else
            {
                return GetStartOfMonth(DateTime.Now.Month + 1, DateTime.Now.Year);
            }
        }

        /// <summary>
        /// Returns the last date of next month's month and year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfNextMonth()
        {
            if (DateTime.Now.Month == 12)
            {
                return GetEndOfMonth(1, DateTime.Now.Year + 1);
            }
            else
            {
                return GetEndOfMonth(DateTime.Now.Month + 1, DateTime.Now.Year);
            }
        }

        /// <summary>
        /// Returns the number of whole months between two dates
        /// <para>An average of 30 days per month is used to determine the number of months between the two dates provided.</para>
        /// </summary>
        public static int GetMonthsBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            double months = days / 30;
            return (int)months;
        }

        /// <summary>
        /// Returns number of whole and fractional months between two dates
        /// <para>An average of 30 days per month is used to determine the number of months between the two dates provided.</para>
        /// </summary>
        public static double GetTotalMonthsBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            return days / 30;
        }

        /// <summary>
        /// Returns the first date of the week at 00:00:00:000 using the defualt start of week
        /// </summary>
        public static DateTime GetStartOfWeek(DateTime date)
        {
            int daysToSubtract = (int)date.DayOfWeek;
            DateTime tempDate = date.Subtract(TimeSpan.FromDays(daysToSubtract));
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the last date of the week at 23:59:59:999 using the defualt start of week
        /// </summary>
        public static DateTime GetEndOfWeek(DateTime date)
        {
            DateTime tempDate = GetStartOfWeek(date).AddDays(6);
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the first date of last week at 00:00:00:000 using the defualt start of week
        /// </summary>
        public static DateTime GetStartOfLastWeek()
        {
            int daysToSubtract = (int)DateTime.Now.DayOfWeek + 7;
            DateTime tempDate = DateTime.Now.Subtract(TimeSpan.FromDays(daysToSubtract));
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the last date of last week at 23:59:59:999 using the defualt start of week
        /// </summary>
        public static DateTime GetEndOfLastWeek()
        {
            DateTime tempDate = GetStartOfLastWeek().AddDays(6);
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the first date of the current week at 00:00:00:000 using the defualt start of week
        /// </summary>
        public static DateTime GetStartOfCurrentWeek()
        {
            return GetStartOfWeek(DateTime.Now);
        }

        /// <summary>
        /// Returns the last date of the current week at 23:59:59:999 using the defualt start of week
        /// </summary>
        public static DateTime GetEndOfCurrentWeek()
        {
            return GetEndOfWeek(DateTime.Now);
        }

        /// <summary>
        /// Returns the first date of next week at 00:00:00:000 using the defualt start of week
        /// </summary>
        public static DateTime GetStartOfNextWeek()
        {
            DateTime tempDate = GetEndOfCurrentWeek().AddDays(1);
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the last date of next week at 23:59:59:999 using the defualt start of week
        /// </summary>
        public static DateTime GetEndOfNextWeek()
        {
            DateTime tempDate = GetStartOfNextWeek().AddDays(6);
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the whole number of weeks between two dates
        /// <para>The oldDate is subtracted from the new date and the difference returned</para>
        /// </summary>
        public static int GetWeeksBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            double weeks = days / 7;
            return (int)weeks;
        }

        /// <summary>
        /// Returns the number whole and fractional weeks between two dates
        /// <para>The oldDate is subtracted from the new date and the difference returned</para>
        /// </summary>
        public static double GetTotalWeeksBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            return days / 7;
        }

        /// <summary>
        /// Returns the week of the year where the first week is at least fours long.
        /// <para>The week starts on Sunday</para>
        /// </summary>
        public static int GetWeekOfCurrentYear()
        {
            return GetWeekOfCurrentYear(CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns the current week of the year where the first week is at least fours days long
        /// </summary>
        public static int GetWeekOfCurrentYear(DayOfWeek firstDayOfWeek)
        {
            return GetWeekOfCurrentYear(CalendarWeekRule.FirstFourDayWeek, firstDayOfWeek);
        }

        /// <summary>
        /// Returns the current week of the year where the week starts on Sunday
        /// </summary>
        public static int GetWeekOfCurrentYear(CalendarWeekRule calendarWeekRule)
        {
            return GetWeekOfCurrentYear(calendarWeekRule, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns the current week of the year
        /// </summary>
        public static int GetWeekOfCurrentYear(CalendarWeekRule calendarWeekRule, DayOfWeek firstDayOfWeek)
        {
            return GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, firstDayOfWeek);
        }

        /// <summary>
        /// Returns the week of the year where the first week is at least fours long.
        /// <para>The week starts on Sunday</para>
        /// </summary>
        public static int GetWeekOfYear(DateTime date)
        {
            return GetWeekOfYear(date, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns the week of the year where the first week is at least fours days long
        /// </summary>
        public static int GetWeekOfYear(DateTime date, DayOfWeek firstDayOfWeek)
        {
            return GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, firstDayOfWeek);
        }

        /// <summary>
        /// Returns the week of the year where the week starts on Sunday
        /// </summary>
        public static int GetWeekOfYear(DateTime date, CalendarWeekRule calendarWeekRule)
        {
            return GetWeekOfYear(date, calendarWeekRule, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns the week of the year
        /// </summary>
        public static int GetWeekOfYear(DateTime date, CalendarWeekRule calendarWeekRule, DayOfWeek firstDayOfWeek)
        {
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            return calendar.GetWeekOfYear(date, calendarWeekRule, firstDayOfWeek);
        }

        /// <summary>
        /// Returns the date with a time of 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the date with a time of 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the whole number of days between two dates
        /// <para>The oldDate is subtracted from the new date and the difference returned</para>
        /// </summary>
        public static int GetDaysBetweenDates(DateTime oldDate, DateTime newDate)
        {
            TimeSpan ts = (newDate - oldDate);
            return ts.Days;
        }

        /// <summary>
        /// Returns the whole and fractional number of days between two dates
        /// <para>The oldDate is subtracted from the new date and the difference returned</para>
        /// </summary>
        public static double GetTotalDaysBetweenDates(DateTime oldDate, DateTime newDate)
        {
            TimeSpan ts = (newDate - oldDate);
            return ts.TotalDays;
        }

        /// <summary>
        /// Returns the name of the specified month using the Current Culture
        /// </summary>
        public static string GetMonthName(int month)
        {
            return DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
        }

        /// <summary>
        /// Returns the name of the specified DayOfWeek using the current culture
        /// </summary>
        public static string GetDayName(DayOfWeek dayOfWeek)
        {
            return DateTimeFormatInfo.CurrentInfo.GetDayName(dayOfWeek);
        }

        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        /// <summary>
        /// Returns a long string representation of the specified date and time using the ToLongDateString() and ToLongTimeString()
        /// <para>The string is formatted as "Date, Time"</para>
        /// </summary>
        public static string GetLongDateTimeString(DateTime date)
        {
            return string.Format("{0}, {1}", date.ToLongDateString(), date.ToLongTimeString());
        }

        /// <summary>
        /// Returns a short string representation of the specified date and time using the ToShortDateString() and ToShortTimeString()
        /// <para>The string is formatted as "Date, Time"</para>
        /// </summary>
        public static string GetShortDateTimeString(DateTime date)
        {
            return string.Format("{0}, {1}", date.ToShortDateString(), date.ToShortTimeString());
        }

        /// <summary>
        /// Returns whether the specified date is a Saturday or Sunday
        /// </summary>
        public static bool IsWeekend(DateTime dateTime)
        {
            return (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday);
        }

        /// <summary>
        /// Indicates whether the specified date is the same year and month as the current date
        /// </summary>
        public static bool IsCurrentMonthAndYear(DateTime date)
        {
            return IsMonthAndYearEqual(DateTime.Now, date);
        }

        /// <summary>
        /// Indicates whether the specified year and month is the same as the current month and year
        /// </summary>
        public static bool IsCurrentMonthAndYear(int year, int month)
        {
            return IsCurrentMonthAndYear(new DateTime(year, month, 1));
        }

        /// <summary>
        /// Indicates whether the specified date is the same as the month and year after the current month
        /// </summary>
        public static bool IsNextMonthAndYear(DateTime date)
        {
            return IsMonthAndYearEqual(DateTime.Now, date);
        }

        /// <summary>
        /// Inficates whether the specified month and year is the same as the month and year after the current month
        /// </summary>
        public static bool IsNextMonthAndYear(int year, int month)
        {
            return IsNextMonthAndYear(new DateTime(year, month, 1));
        }

        /// <summary>
        /// Indicates whether the specified date is the same as the month and year before the current month
        /// </summary>
        public static bool IsPreviousMonthAndYear(DateTime date)
        {
            return IsMonthAndYearEqual(DateTime.Now, date);
        }

        /// <summary>
        /// Indicates whether the specified month and year is the same as the month and year before the current month
        /// </summary>
        public static bool IsPreviousMonthAndYear(int year, int month)
        {
            return IsPreviousMonthAndYear(new DateTime(year, month, 1));
        }

        /// <summary>
        /// Indicates whether the specified dates have the same month and year
        /// </summary>
        public static bool IsMonthAndYearEqual(DateTime dateOne, DateTime dateTwo)
        {
            if (dateOne.Year == dateTwo.Year)
            {
                if (dateOne.Month == dateTwo.Month)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a list of all the dates between two dates
        /// <para>The begin date must be before the end date</para>
        /// </summary>
        public static List<DateTime> GetDatesBetweenDates(DateTime beginDate, DateTime endDate)
        {
            List<DateTime> list = new List<DateTime>();
            if (endDate < beginDate)
            {
                return list;
            }

            int days = GetDaysBetweenDates(beginDate, endDate);
            for (int i = 0; i < days; i++)
            {
                DateTime currentDate = beginDate.AddDays(i);
                list.Add(currentDate);
            }

            return list;
        }

        /// <summary>
        /// Returns a list of all the Sundays remaining in the current year
        /// <para>The begin date must be before the end date</para>
        /// </summary>
        public static List<DateTime> GetSundaysThroughCurrentYear()
        {
            return GetDatesBetweenDates(DateTime.Today, GetStartOfCurrentYear(), DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns a list of Sundays between two dates
        /// <para>The begin date must be before the end date</para>
        /// </summary>
        public static List<DateTime> GetSundaysBetweenDates(DateTime beginDate, DateTime endDate)
        {
            return GetDatesBetweenDates(beginDate, endDate, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns a list of Saturdays remaining in the current year
        /// </summary>
        public static List<DateTime> GetSaturdaysThroughCurrentYear()
        {
            return GetDatesBetweenDates(DateTime.Today, GetStartOfCurrentYear(), DayOfWeek.Saturday);
        }

        /// <summary>
        /// Returns a list of Saturdays between two dates
        /// <para>The begin date must be before the end date</para>
        /// </summary>
        public static List<DateTime> GetSaturdaysBetweenDates(DateTime beginDate, DateTime endDate)
        {
            return GetDatesBetweenDates(beginDate, endDate, DayOfWeek.Saturday);
        }

        /// <summary>
        /// Returns the number dates between two dates and of the specified day of the week
        /// <para>The begin date must be before the end date</para>
        /// </summary>
        public static List<DateTime> GetDatesBetweenDates(DateTime beginDate, DateTime endDate, DayOfWeek dayOfWeek)
        {
            List<DateTime> list = new List<DateTime>();
            if (endDate < beginDate)
            {
                return list;
            }

            int days = GetDaysBetweenDates(beginDate, endDate);
            for (int i = 0; i < days; i++)
            {
                DateTime currentDate = beginDate.AddDays(i);
                if (currentDate.DayOfWeek == dayOfWeek)
                {
                    list.Add(currentDate);
                }
            }

            return list;
        }



    }

}
