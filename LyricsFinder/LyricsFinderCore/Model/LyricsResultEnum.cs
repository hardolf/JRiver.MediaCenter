using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Enumeration of the lyric services results.
    /// </summary>
    [Flags]
    public enum LyricResultEnum
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        NotProcessedYet = 0,
        Found = 1,
        NotFound = 2,
        SkippedOldLyrics = 4,
        ManuallyEdited = 8,
        Error = 16,
        Processing = 32,
        Canceled = 64
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }



    /// <summary>
    /// Extension methods for the <see cref="LyricResultEnum"/>.
    /// </summary>
    public static class LyricResultEnumExtensions
    {

        /// <summary>
        /// Lyrics result text.
        /// </summary>
        /// <returns>Transformed text.</returns>
        public static string ResultText(this LyricResultEnum lyricsResult)
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


        /// <summary>
        /// Converts a string to a matching <see cref="LyricResultEnum"/> value.
        /// </summary>
        /// <param name="resultText">The result text.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">resultText</exception>
        /// <exception cref="ArgumentException"></exception>
        public static LyricResultEnum ToLyricResultEnum(this string resultText)
        {
            if (resultText == null) throw new ArgumentNullException(nameof(resultText));

            resultText = resultText.Replace(" ", string.Empty);

            foreach (var lr in (LyricResultEnum[])Enum.GetValues(typeof(LyricResultEnum)))
            {
                if (lr.ToString().Equals(resultText, StringComparison.InvariantCultureIgnoreCase))
                    return lr;
            }

            throw new ArgumentException($"The string value does not match any LyricResultEnum values: {resultText}.");
        }

    }

}
