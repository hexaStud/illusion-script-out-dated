namespace IllusionScript.SDK.Extensions
{
    public static class String
    {
        public static string Repeat(this string str, int repeat)
        {
            var newStr = "";
            for (var i = 0; i < repeat; i++) newStr += str;

            return newStr;
        }
    }
}