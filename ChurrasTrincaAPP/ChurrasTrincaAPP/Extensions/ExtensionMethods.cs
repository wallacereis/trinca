using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using ChurrasTrincaAPP.Models;

namespace ChurrasTrincaAPP.Extension
{
    public static class ExtensionMethods
    {
        public static string RemoveNonNumbers(this string texto)
        {
            var regex = new Regex(@"[^\d]");
            return regex.Replace(texto, "");
        }

        public static int DefaultInt(this object value)
        {
            if (string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return 0;
            }
            return Convert.ToInt32(value);
        }

        public static string DefaultString(this object value)
        {
            if (string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return string.Empty;
            }
            return Convert.ToString(value).Trim();
        }

        public static decimal DefaultDecimal(this object value)
        {
            var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            numberFormat.NumberDecimalSeparator = ",";

            if (string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return 0;
            }
            return Convert.ToDecimal(value);
        }

        public static DateTime DefaultDateTime(this object value)
        {
            if (string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return new DateTime(1900, 1, 1);
            }
            return Convert.ToDateTime(value);
        }

        public static bool IsCreditCardInfoValid(this string expiryDate)
        {
            //var cardCheck = new Regex(@"^(1298|1267|4512|4567|8901|8933)([\-\s]?[0-9]{4}){3}$");
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");

            //if (!cardCheck.IsMatch(cardNo)) // <1>check card number is valid
            //    return false;
            //if (!cvvCheck.IsMatch(cvv)) // <2>check cvv is valid as "999"
            //    return false;

            var dateParts = expiryDate.Split('/'); //expiry date in from MM/yyyy            
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch("20" + dateParts[1])) // <3 - 6>
                return false; // ^ check date format is valid as "MM/yyyy"

            var year = int.Parse("20" + dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            //check expiry greater than today & within next 6 years <7, 8>>
            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(14));
        }

        public static string FormatErrorMessage<T>(this Response<T> model)
        {
            if (model.message is not null)
            {
                return model.message;
            }
            else
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrEmpty(model.exception))
                    sb.AppendLine(model.exception);

                if (model.error.errors.Count > 0)
                {
                    model.error.errors.ForEach(item =>
                    {
                        sb.AppendLine(item);
                    });
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Substitui o caractere "," por "." em valores decimais.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatStringToDecimalPoint(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.Replace(".", "").Replace(",", ".");
        }

        public static string FormatDecimalToString(this decimal value)
        {
            return string.Format("{0:N2}", value);
        }

        public static decimal ConvertStringToDecimal(this string value)
        {
            var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            numberFormat.NumberDecimalSeparator = ",";
            return Convert.ToDecimal(value.Replace(".", ","), numberFormat);
        }

        public static string FirstCharToUpper(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Substring(0, 1).ToUpper() + string.Join("", value.Substring(1, value.Length - 1));

            return null;
        }
    }
}
