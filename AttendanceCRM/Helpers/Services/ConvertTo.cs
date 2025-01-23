using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;

namespace AttendanceCRM.Helpers.Services
{
    public static class ConvertTo
    {
        #region Methods/Functions

        #region Common type conversion

        public static string String(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    return Convert.ToString(readField, CultureInfo.InvariantCulture);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ToStringTrim(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    return readField.ToString().Trim();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static double ToDouble(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    if (readField.ToString().Trim().Length == 0)
                    {
                        return 0.0;
                    }
                    else
                    {
                        return Convert.ToDouble(readField, CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    return 0.0;
                }
            }
            else
            {
                return 0.0;
            }
        }

        public static decimal ToDecimal(this object readField, int decimalPlace = -1)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    if (readField.ToString().Trim().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        decimal x;
                        if (decimal.TryParse(readField.ToString(), out x))
                        {
                            x = decimal.Round(x, decimalPlace == -1 ? 2 : decimalPlace);
                            return x;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static string ToNullableDecimalString(this decimal? readField, int decimalPoints = -1)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    return readField.ToDecimal().ToDecimalString(decimalPoints);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ToDecimalString(this decimal readField, int decimalPoints = -1)
        {
            return Convert.ToString(Math.Round(Convert.ToDecimal(readField), decimalPoints == -1 ? 2 : decimalPoints));
        }

        public static bool ToBoolean(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    string stringReadField = Convert.ToString(readField, CultureInfo.InvariantCulture);

                    if (stringReadField == "1")
                    {
                        return true;
                    }

                    bool x;
                    if (bool.TryParse(stringReadField, out x))
                    {
                        return x;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public static bool ToBooleanCustom(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    string stringReadField = Convert.ToString(readField, CultureInfo.InvariantCulture);

                    if (stringReadField.ToLower() == "on")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public static bool? ToBoolNull(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    string stringReadField = Convert.ToString(readField, CultureInfo.InvariantCulture);

                    if (stringReadField == "1")
                    {
                        return true;
                    }

                    bool x;
                    if (bool.TryParse(stringReadField, out x))
                    {
                        return x;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public static int ToInteger(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    if (readField.ToString().Trim().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        int toReturn;
                        if (int.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out toReturn))
                        {
                            return toReturn;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }


        public static long ToLong(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    if (readField.ToString().Trim().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        long toReturn;
                        if (long.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out toReturn))
                        {
                            return toReturn;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }


        public static short ToShort(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    if (readField.ToString().Trim().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        short toReturn = 0;
                        if (short.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out toReturn))
                        {
                            return toReturn;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region Date Time

        public static DateTime? ToNullableDate(this object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    DateTime dateReturn;
                    if (DateTime.TryParse(Convert.ToString(readField, CultureInfo.CurrentCulture), out dateReturn))
                    {
                        return Convert.ToDateTime(readField, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        public static DateTime ToDate(this object readField)
        {
            return Convert.ToDateTime(readField, CultureInfo.CurrentCulture);
        }

        public static string ToDateString(this object readField, string dateFormat)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(DBNull))
                {
                    if (!string.IsNullOrEmpty(dateFormat))
                    {
                        return Convert.ToDateTime(readField, CultureInfo.CurrentCulture).ToString(dateFormat, CultureInfo.InvariantCulture);
                    }

                    return Convert.ToDateTime(readField, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
                }
            }

            return DateTime.MinValue.ToString(CultureInfo.CurrentCulture);
        }


        public static string ToDateString(this DateTime? value, string dateFormat)
        {
            return value.ToDateString(dateFormat, string.Empty);
        }


        public static string ToDateString(this DateTime? value, string dateFormat, string nullvalue)
        {
            if (value != null && value.HasValue)
            {
                return value.Value.ToDateString(dateFormat);
            }
            else
            {
                return nullvalue;
            }
        }

        public static string ToSystemDate(this DateTime value)
        {
            return value.ToDateString("dd/MM/yyyy");
        }

        public static string ToSystemDate(this DateTime? value)
        {
            string q = string.Empty;
            if (value.HasValue)
            {
                q = value.Value.ToSystemDate();
            }

            return q;
        }

        public static string ToSystemDateTime(this DateTime value)
        {
            return value.ToDateString("dd/MM/yyyy hh:mm tt");
        }

        public static string ToSystemDateTime(this DateTime? value)
        {
            string q = string.Empty;
            if (value.HasValue)
            {
                q = value.Value.ToSystemDateTime();
            }

            return q;
        }

        public static string ToTimeString(this DateTime readField)
        {
            return readField.ToString("hh:mm tt");
        }

        public static string ToTimeString(this TimeSpan? value, string timeFormat)
        {
            return value.ToTimeString(timeFormat, string.Empty);
        }

        public static string ToTimeString(this TimeSpan? value, string timeFormat, string nullvalue)
        {
            if (value != null && value.HasValue)
            {
                return value.Value.ToString(timeFormat);
            }
            else
            {
                return nullvalue;
            }
        }

        #endregion

        public static object ToDBNullValue(this string value)
        {
            if (value == null | string.IsNullOrEmpty(value))
            {
                return DBNull.Value;
            }

            return value;
        }

        public static object ToDBNull(this object value)
        {
            if (value != null)
            {
                return value;
            }

            return DBNull.Value;
        }

        public static bool IsVoid(string checkString)
        {
            if (checkString == null)
            {
                return true;
            }
            else
            {
                return checkString == string.Empty || checkString.Length == 0 || checkString.Trim() == string.Empty;
            }
        }

        public static string GetPropertyNameInString<T, TProp>(this Expression<Func<T, TProp>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                return null;
            }

            return memberExpression.Member.Name;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            ////Get all the properties
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                ////Defining type of data column gives proper data table
                var type = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;
                ////Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    ////inserting property values to datatable rows
                    values[i] = props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }

            ////put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                var type = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;
                table.Columns.Add(prop.Name, type);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }

                table.Rows.Add(values);
            }

            return table;
        }

        public static DataTable ToReportDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                var type = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;
                //set Dispaly Name
                table.Columns.Add(prop.DisplayName, type);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    PropertyDescriptor prop = props[i];
                    values[i] = props[i].GetValue(item);
                }

                table.Rows.Add(values);
            }

            //Remove NotMapped attribute 
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];

                if (prop.Attributes.OfType<NotMappedAttribute>().Any())
                {
                    table.Columns.Remove(prop.DisplayName);
                }
            }

            return table;
        }

        public static string ImageToBase64(string imageURL)
        {
            if (!string.IsNullOrEmpty(imageURL))
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(imageURL);
                    return "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                }
            }

            return string.Empty;
        }

        public static string ThousandSeparator(this decimal? value)
        {
            return value.ToDecimal().ToString("N");
        }

        public static string ConvertMinutestoHours(double minutes)
        {
            var time = TimeSpan.FromMinutes(minutes);
            return string.Format("{0}:{1:00}", (int)time.TotalHours, time.Minutes);
        }

        public static TimeSpan ConvertHourstoTimeSpan(string hours)
        {
            return new TimeSpan(hours.Split(':')[0].ToInteger(), hours.Split(':')[1].ToInteger(), 0);
        }

        private static DataTable FromXMLStringToDataTable(this string xmlString)
        {
            DataSet ds = new DataSet();
            using (StringReader stringReader = new StringReader(xmlString))
            {
                ds = new DataSet();
                ds.ReadXml(stringReader);
            }

            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }

        #endregion
    }
}
