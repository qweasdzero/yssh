namespace StarForce
{
    public static class StringExtension
    {
        /// <summary>
        /// 返回字符串的子串。
        /// 例如：(1,2,3) startChar: ‘(’  endChar: ‘)’
        /// 返回值:1,2,3
        /// </summary>
        /// <param name="string">字符串。</param>
        /// <param name="startChar">开始字符。</param>
        /// <param name="endChar">结束字符。</param>
        /// <returns>子串。</returns>
        public static string SubString(this string @string, char startChar, char endChar)
        {
            if (string.IsNullOrEmpty(@string))
            {
                return @string;
            }

            int startIndex = @string.IndexOf(startChar);
            startIndex++;
            int endIndex = @string.LastIndexOf(endChar);
            int lenght = endIndex - startIndex;
            return @string.Substring(startIndex, lenght);
        }
    }
}