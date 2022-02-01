using IllusionScript.SDK.Errors;
using IllusionScript.SDK.Nodes;

namespace IllusionScript.SDK
{
    public class ParserResult
    {
        public int AdvanceCount;
        public Error Error;
        public bool IsHeader;
        public int LastRegisteredAdvanceCount;
        public Node Node;
        public int ToReverseCount;

        public ParserResult()
        {
            IsHeader = true;
            AdvanceCount = 0;
            LastRegisteredAdvanceCount = 0;
            ToReverseCount = 0;
        }

        public void RegisterAdvancement()
        {
            AdvanceCount++;
            LastRegisteredAdvanceCount++;
        }

        public Node Register(ParserResult res)
        {
            LastRegisteredAdvanceCount = res.AdvanceCount;
            AdvanceCount += res.AdvanceCount;
            if (res.Error != default(Error))
                Error = res.Error;
            else
                CheckHeader(res);

            return res.Node;
        }

        public Node TryRegister(ParserResult res)
        {
            if (res.Error != default(Error))
            {
                ToReverseCount = res.AdvanceCount;
                return default;
            }

            return Register(res);
        }

        public ParserResult Success(Node node)
        {
            CheckHeader(node);
            Node = node;
            return this;
        }

        public ParserResult Failure(Error err)
        {
            if (Error == default(Error) || AdvanceCount == 0) Error = err;

            return this;
        }

        private void CheckHeader(ParserResult res)
        {
            CheckHeader(res.Node);
        }

        private void CheckHeader(Node node)
        {
            var type = node.GetType();
            if (type.IsSubclassOf(typeof(HeaderNode)) && !IsHeader)
                Error = new InvalidSyntaxError(
                    "Header function and fields can only used on top of the file", node.StartPos, node.EndPos);

            if (!type.IsSubclassOf(typeof(HeaderNode)) &&
                type != typeof(UnaryOpNode) &&
                type != typeof(BinOpNode) &&
                type != typeof(NumberNode) &&
                type != typeof(StringNode) &&
                type != typeof(ListNode) &&
                type != typeof(VarAccessNode) &&
                IsHeader
               )
                IsHeader = false;
        }
    }
}