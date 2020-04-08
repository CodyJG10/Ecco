using Ecco.Mobile.Controls;
using Ecco.Mobile.ViewModels;
using Ecco.Mobile.ViewModels.Auth;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Ecco.Mobile.Converters
{
    /// <summary>
    /// This class have methods to convert the Boolean values to color objects. 
    /// This is needed to validate in the Entry controls. If the validation is failed, it will return the color code of error, otherwise it will be transparent.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ErrorValidationColorConverter : IValueConverter
    {
        /// <summary>
        /// Identifies the simple and gradient login pages.
        /// </summary>
        public string PageVariantParameter { get; set; }

        /// <summary>
        /// This method is used to convert the bool to color.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the color.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var emailEntry = parameter as BorderlessEntry;

            if (!(emailEntry?.BindingContext is LoginViewModelBase bindingContext))
            {
                return Color.Transparent;
            }

            var isFocused = (bool)value;
            bindingContext.IsInvalidEmail = !isFocused && !CheckValidEmail(bindingContext.Email);

            if (isFocused)
            {
                return Color.FromRgba(255, 255, 255, 0.6);
            }

            return bindingContext.IsInvalidEmail ? Color.FromHex("#FF4A4A") : Color.Transparent;
        }

        /// <summary>
        /// This method is used to convert the color to bool.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the string.</returns>        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        /// <summary>
        /// Validates the email.
        /// </summary>
        /// <param name="email">Gets the email.</param>
        /// <returns>Returns the boolean value.</returns>
        private static bool CheckValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }

            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return regex.IsMatch(email) && !email.EndsWith(".");
        }
    }
}