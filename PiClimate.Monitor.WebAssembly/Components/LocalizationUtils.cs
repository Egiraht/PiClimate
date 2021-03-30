using System.Collections.Generic;
using System.Globalization;

namespace PiClimate.Monitor.WebAssembly.Components
{
  /// <summary>
  ///   The static class with various localization-related utility methods.
  /// </summary>
  public static class LocalizationUtils
  {
    /// <summary>
    ///   Defines the dictionary for converting short locale names into their default long names with a country code
    ///   specified.
    /// </summary>
    public static readonly Dictionary<string, string> DefaultLocaleNames = new()
    {
      {"af", "af-ZA"},
      {"am", "am-ET"},
      {"ar", "ar-SA"},
      {"arn", "arn-CL"},
      {"as", "as-IN"},
      {"az", "az-Latn-AZ"},
      {"ba", "ba-RU"},
      {"be", "be-BY"},
      {"bg", "bg-BG"},
      {"bn", "bn-IN"},
      {"bo", "bo-CN"},
      {"br", "br-FR"},
      {"bs", "bs-Cyrl-BA"},
      {"ca", "ca-ES"},
      {"co", "co-FR"},
      {"cs", "cs-CZ"},
      {"cy", "cy-GB"},
      {"da", "da-DK"},
      {"de", "de-DE"},
      {"dsb", "dsb-DE"},
      {"dv", "dv-MV"},
      {"el", "el-GR"},
      {"en", "en-US"},
      {"es", "es-ES"},
      {"et", "et-EE"},
      {"eu", "eu-ES"},
      {"fa", "fa-IR"},
      {"fi", "fi-FI"},
      {"fil", "fil-PH"},
      {"fo", "fo-FO"},
      {"fr", "fr-FR"},
      {"fy", "fy-NL"},
      {"ga", "ga-IE"},
      {"gd", "gd-GB"},
      {"gl", "gl-ES"},
      {"gsw", "gsw-FR"},
      {"gu", "gu-IN"},
      {"ha", "ha-Latn-NG"},
      {"he", "he-IL"},
      {"hi", "hi-IN"},
      {"hr", "hr-HR"},
      {"hsb", "hsb-DE"},
      {"hu", "hu-HU"},
      {"hy", "hy-AM"},
      {"id", "id-ID"},
      {"ig", "ig-NG"},
      {"ii", "ii-CN"},
      {"is", "is-IS"},
      {"it", "it-IT"},
      {"iu", "iu-Cans-CA"},
      {"ja", "ja-JP"},
      {"ka", "ka-GE"},
      {"kk", "kk-KZ"},
      {"kl", "kl-GL"},
      {"km", "km-KH"},
      {"kn", "kn-IN"},
      {"ko", "ko-KR"},
      {"kok", "kok-IN"},
      {"ky", "ky-KG"},
      {"lb", "lb-LU"},
      {"lo", "lo-LA"},
      {"lt", "lt-LT"},
      {"lv", "lv-LV"},
      {"mi", "mi-NZ"},
      {"mk", "mk-MK"},
      {"ml", "ml-IN"},
      {"mn", "mn-MN"},
      {"moh", "moh-CA"},
      {"mr", "mr-IN"},
      {"ms", "ms-MY"},
      {"mt", "mt-MT"},
      {"nb", "nb-NO"},
      {"ne", "ne-NP"},
      {"nl", "nl-NL"},
      {"nn", "nn-NO"},
      {"no", "no-NO"},
      {"nso", "nso-ZA"},
      {"oc", "oc-FR"},
      {"or", "or-IN"},
      {"pa", "pa-IN"},
      {"pl", "pl-PL"},
      {"prs", "prs-AF"},
      {"ps", "ps-AF"},
      {"pt", "pt-PT"},
      {"qut", "qut-GT"},
      {"quz", "quz-BO"},
      {"rm", "rm-CH"},
      {"ro", "ro-RO"},
      {"ru", "ru-RU"},
      {"rw", "rw-RW"},
      {"sa", "sa-IN"},
      {"sah", "sah-RU"},
      {"se", "se-SE"},
      {"si", "si-LK"},
      {"sk", "sk-SK"},
      {"sl", "sl-SI"},
      {"sma", "sma-NO"},
      {"smj", "smj-NO"},
      {"smn", "smn-FI"},
      {"sms", "sms-FI"},
      {"sq", "sq-AL"},
      {"sr", "sr-Cyrl-RS"},
      {"sv", "sv-SE"},
      {"sw", "sw-KE"},
      {"syr", "syr-SY"},
      {"ta", "ta-IN"},
      {"te", "te-IN"},
      {"tg", "tg-Cyrl-TJ"},
      {"th", "th-TH"},
      {"tk", "tk-TM"},
      {"tn", "tn-ZA"},
      {"tr", "tr-TR"},
      {"tt", "tt-RU"},
      {"tzm", "tzm-Latn-DZ"},
      {"ug", "ug-CN"},
      {"uk", "uk-UA"},
      {"ur", "ur-PK"},
      {"uz", "uz-Latn-UZ"},
      {"vi", "vi-VN"},
      {"wo", "wo-SN"},
      {"xh", "xh-ZA"},
      {"yo", "yo-NG"},
      {"zh", "zh-CN"},
      {"zu", "zu-ZA"}
    };

    /// <summary>
    ///   Gets the culture information object for the current browser.
    /// </summary>
    public static CultureInfo GetBrowserCultureInfo()
    {
      // Converting some short locale names to their long variants with explicit country codes (xx-XX).
      // This is necessary for UnitsNet library compatibility as it provides localizations only for locales with a
      // country code being specified.
      var locale = CultureInfo.CurrentUICulture;
      var localeName = locale.Name;

      try
      {
        if (DefaultLocaleNames.ContainsKey(localeName))
          localeName = DefaultLocaleNames[localeName];
        locale = new CultureInfo(localeName);
      }
      catch
      {
        // Ignore exceptions.
      }

      return locale;
    }
  }
}
