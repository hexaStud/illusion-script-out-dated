namespace IllusionScript.SDK.Extensions
{
    public static class String
    {
        public static string Repeat(this string str, int repeat)
        {
            string newStr = "";
            for (int i = 0; i < repeat; i++)
            {
                newStr += str;
            }

            return newStr;
        }
    }
}