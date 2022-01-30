using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK
{
    public class Position
    {
        public int Idx;
        public int Ln;
        public int Col;
        public string FileName;
        public string Filepath;
        public string FileText;

        public Position(int idx, int ln, int col, string fileName, string filepath, string fileText)
        {
            Idx = idx;
            Ln = ln;
            Col = col;
            FileName = fileName;
            Filepath = filepath;
            FileText = fileText;
        }

        public Position Advance(char currentChar)
        {
            Idx++;
            Col++;

            if (currentChar == '\n')
            {
                Col = 0;
                Ln++;
            }

            return this;
        }

        public Position Advance()
        {
            Idx++;
            Col++;

            return this;
        }

        public Position Copy()
        {
            return new Position(Idx, Ln, Col, FileName, Filepath, FileText);
        }

        public string __bundle__()
        {
            return "{" +
                   $"\"idx\": {Idx}, \"ln\": {Ln}, \"col\": {Col}, \"fileName\": \"{FileName}\", \"filepath\": \"{Filepath.Replace("\\", "\\\\")}\"" +
                   "}";
        }

        public static Position Empty()
        {
            return new Position(0, 0, 0, "", "", "");
        }

        public static Position Convert(Json json)
        {
            Position empty = Empty();
            empty.Idx = json.GetAsInt("idx");
            empty.Ln = json.GetAsInt("ln");
            empty.Col = json.GetAsInt("col");
            empty.FileName = json.GetAsText("fileName");
            empty.Filepath = json.GetAsText("filepath");
            return empty;
        }
    }
}