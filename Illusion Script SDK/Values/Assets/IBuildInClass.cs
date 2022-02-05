using System.Collections.Generic;

namespace IllusionScript.SDK.Values.Assets
{
    public interface IBuildInClass
    {
        public string Name { get; }
        public List<IBuildInMethod> Methods { get; }
        public List<IBuildInField> Fields { get; }
        
        public List<IBuildInMethod> StaticMethods { get; }
        public List<IBuildInField> StaticFields { get; }
    }
}