using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Sanus.Localization
{
    public class Localization : Prism.Mvvm.BindableBase
    {
        public static Localization Current { get; private set; }

        static Localization()
        {
            Current = new Localization();
        }

        public string this[string key] { get { return Resources.LocalTranslation.ResourceManager.GetString(key, new CultureInfo(Helpers.Settings.LanguageSettings)); } }

        internal void Refresh()
        {
            RaisePropertyChanged("");
            RaisePropertyChanged(".");
        }
    }

    public static class L
    {
        public static string Localize(string text)
        {
            return LocalizeInternal(text);
        }

        public static string Localize(string text, CultureInfo cultureInfo)
        {
            return LocalizeInternal(text, cultureInfo);
        }

        public static string Localize(string text, params object[] args)
        {
            return string.Format(LocalizeInternal(text), args);
        }

        public static string LocalizeWithThreeDots(string text, params object[] args)
        {
            var localizedText = Localize(text, args);
            return CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ? "..." + localizedText : localizedText + "...";
        }

        public static string LocalizeWithParantheses(string text, object valueWithinParentheses, params object[] args)
        {
            var localizedText = Localize(text);
            return CultureInfo.CurrentCulture.TextInfo.IsRightToLeft
                ? " (" + valueWithinParentheses + ")" + localizedText
                : localizedText + " (" + valueWithinParentheses + ")";
        }

        private static string LocalizeInternal(string text, CultureInfo cultureInfo = null)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return Resources.LocalTranslation.ResourceManager.GetString(text, cultureInfo ?? new CultureInfo(Helpers.Settings.LanguageSettings));
        }
    }
}
