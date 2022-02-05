using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes.Assets
{
    public struct IfCase
    {
        public Node Condition;
        public Node Statements;
        public bool Bool;

        public string __bundle__()
        {
            return "{" +
                   $"\"condition\": {Condition.__bundle__()}, \"statements\": {Statements.__bundle__()}, \"bool\": {Bool.ToString().ToLower()}" +
                   "}";
        }

        public IfCase __unbundle__(Json json)
        {
            Condition = Node.ConvertNode(json.Get("condition"));
            Statements = Node.ConvertNode(json.Get("statements"));
            Bool = json.GetAsBool("bool");

            return this;
        }
    }
}