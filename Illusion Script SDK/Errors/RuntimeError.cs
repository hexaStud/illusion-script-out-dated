using System.IO;

namespace IllusionScript.SDK.Errors
{
    public class RuntimeError : Error
    {
        public Context Context;

        public RuntimeError(string details, Context context, Position startPos, Position endPos) : base("Runtime Error",
            details, startPos, endPos)
        {
            Context = context;
        }

        private string GenerateTraceback()
        {
            var result = "\n";
            var pos = StartPos;
            var context = Context;

            while (context != default(Context))
            {
                if (pos != default(Position))
                    result =
                        $"File {Path.Join(pos.Filepath, pos.FileName)}, line {pos.Ln + 1}, {context.DisplayName}\n" +
                        result;
                else
                    result = "File [Native code] <index function call>\n" + result;

                pos = context.ParentEntryPos != default(Position) ? context.ParentEntryPos : default;
                context = Context.Parent != default(Context) ? context.Parent : default;
            }

            return $"Traceback (most recent call last)\n{result}";
        }

        public override string ToString()
        {
            var result = GenerateTraceback();
            result += $"{Name}: {Details}\n";
            // result += StringWithArrows();
            return result;
        }
    }
}