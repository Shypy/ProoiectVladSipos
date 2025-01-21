using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProoiectVladSipos.Converters
{
    public class EuroSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Adaugă simbolul € pentru afișare
            if (value is decimal decimalValue)
            {
                return $"{decimalValue:0.00} €";
            }
            if (value is double doubleValue)
            {
                return $"{doubleValue:0.00} €";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Elimină simbolul € pentru salvare
            if (value is string stringValue)
            {
                stringValue = stringValue.Replace("€", "").Trim();
                if (decimal.TryParse(stringValue, out var result))
                {
                    return result;
                }
            }
            return 0;
        }
    }
}
