using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Utilities for the LyricsFinder.
    /// </summary>
    internal static class Utility
    {

        /// <summary>
        /// Lyrics result text.
        /// </summary>
        /// <returns>Transformed text.</returns>
        public static string ResultText(
            this LyricResultEnum lyricsResult)
        {
            var ret = new StringBuilder(lyricsResult.ToString());

            // Convert CamelCase to normal sentence, e.g. "Camel case"
            for (int i = 0; i < ret.Length; i++)
            {
                var tst = ret[i].ToString(CultureInfo.InvariantCulture).ToUpperInvariant()[0];

                if ((i > 0) && (ret[i] == tst))
                {
                    ret[i] = ret[i].ToString(CultureInfo.InvariantCulture).ToLowerInvariant()[0];
                    ret.Insert(i, ' ');
                }
            }

            return ret.ToString();
        }

    }

}
