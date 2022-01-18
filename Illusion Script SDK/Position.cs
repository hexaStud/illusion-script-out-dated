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

        public static Position Empty()
        {
            return new Position(0, 0, 0, "", "", "");
        }
    }
}