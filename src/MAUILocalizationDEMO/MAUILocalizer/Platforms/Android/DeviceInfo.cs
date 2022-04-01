using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUILocalizer
{
  public static partial class DeviceInfoService
  {
    public static partial CultureInfo DeviceCulture()
    {
      var androidLocale = Java.Util.Locale.Default;
      var netLanguage = AndroidToDotnetLanguage(androidLocale?.ToString()?.Replace("_", "-") ?? string.Empty);
      // this gets called a lot - try/catch can be expensive so consider caching or something
      System.Globalization.CultureInfo ci = default!;
      try
      {
        ci = new System.Globalization.CultureInfo(netLanguage);
      }
      catch (CultureNotFoundException)
      {
        // iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
        // fallback to first characters, in this case "en"
        try
        {
          var fallback = ToDotnetFallbackLanguage(netLanguage);
          ci = new System.Globalization.CultureInfo(fallback);
        }
        catch (CultureNotFoundException)
        {
          // iOS language not valid .NET culture, falling back to English
          ci = new System.Globalization.CultureInfo("en");
        }
      }
      return ci;
    }

    private static string AndroidToDotnetLanguage(string? androidLanguage)
    {
      if(string.IsNullOrWhiteSpace(androidLanguage))
        return "en-us";

      var netLanguage = androidLanguage;
      //certain languages need to be converted to CultureInfo equivalent
      switch (androidLanguage)
      {
        case "ms-BN":   // "Malaysian (Brunei)" not supported .NET culture
        case "ms-MY":   // "Malaysian (Malaysia)" not supported .NET culture
        case "ms-SG":   // "Malaysian (Singapore)" not supported .NET culture
          netLanguage = "ms"; // closest supported
          break;
        case "in-ID":  // "Indonesian (Indonesia)" has different code in  .NET
          netLanguage = "id-ID"; // correct code for .NET
          break;
        case "gsw-CH":  // "Schwiizertüütsch (Swiss German)" not supported .NET culture
          netLanguage = "de-CH"; // closest supported
          break;
          // add more application-specific cases here (if required)
          // ONLY use cultures that have been tested and known to work
      }
      return netLanguage;
    }

    private static string ToDotnetFallbackLanguage(string languageCode)
    {
      string netLanguage = "en-US";

      switch (languageCode)
      {
        case "gsw":
          netLanguage = "de-CH"; // equivalent to German (Switzerland) for this app
          break;
          // add more application-specific cases here (if required)
          // ONLY use cultures that have been tested and known to work
      }
      return netLanguage;
    }
  }
}
