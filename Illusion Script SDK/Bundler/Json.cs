using System;
using System.Collections.Generic;
using System.Linq;

namespace IllusionScript.SDK.Bundler
{
    public class Json
    {
        private readonly Dictionary<string, string> Objs;
        private bool IsArray;

        private Json(string str)
        {
            Objs = new Dictionary<string, string>();
            string[][] objs;
            if (str.StartsWith("{") && str.EndsWith("}"))
            {
                IsArray = false;
                objs = BuildObj(str);
            }
            else if (str.StartsWith("[") && str.EndsWith("]"))
            {
                IsArray = true;
                objs = BuildArray(str);
            }
            else
            {
                throw new Exception("Syntax Error");
            }

            foreach (string[] strings in objs)
            {
                Objs.Add(strings[0], strings[1]);
            }
        }

        private string[][] BuildObj(string str)
        {
            str = str.Substring(1, str.Length - 2);
            string[] objs = Split(str);

            List<string[]> json = new List<string[]>();

            foreach (string s in objs)
            {
                string obj = Trim(s);

                int splitInt = 0;
                bool openStr = false;
                char lastChar = default;
                string[] parts = new string[2];

                foreach (char c in obj)
                {
                    if (c == '"' && lastChar != '\\')
                    {
                        openStr = !openStr;
                    }

                    if (c == ':' && !openStr)
                    {
                        parts[0] = obj.Substring(0, splitInt);
                        parts[1] = obj.Substring(splitInt + 1);
                        if (parts[1].EndsWith(","))
                        {
                            parts[1] = parts[1].Substring(0, parts[1].Length - 1);
                        }

                        break;
                    }

                    splitInt++;
                    lastChar = c;
                }

                json.Add(parts);
            }

            return json.ToArray();
        }

        private string[][] BuildArray(string str)
        {
            str = str.Substring(1, str.Length - 2);
            string[] objs = Split(str);

            List<string[]> json = new List<string[]>();
            int index = 0;

            foreach (string s in objs)
            {
                string obj = Trim(s);
                if (obj.EndsWith(","))
                {
                    obj = obj.Substring(0, obj.Length - 1);
                }

                json.Add(new[] { '"' + index.ToString() + '"', obj });
                index++;
            }

            return json.ToArray();
        }

        private string[] Split(string str)
        {
            int bodyIndex = 0;
            List<string> objs = new List<string>();
            string obj = "";
            bool openStr = false;
            char lastChar = default;
            foreach (char c in str)
            {
                if (c == '"' && lastChar != '\\')
                {
                    openStr = !openStr;
                }

                if (c == '[' && !openStr || c == '{' && !openStr)
                {
                    bodyIndex++;
                }

                if (c == ']' && !openStr || c == '}' && !openStr)
                {
                    bodyIndex--;
                }

                if (c == ',' && !openStr && bodyIndex == 0)
                {
                    obj += c;
                    objs.Add(obj);
                    obj = "";
                    lastChar = c;
                    continue;
                }

                lastChar = c;
                obj += c;
            }

            if (obj != "")
            {
                objs.Add(obj);
            }

            return objs.ToArray();
        }

        private string Trim(string str)
        {
            string nStr = "";
            bool openStr = false;
            char lastChar = default;
            foreach (char c in str)
            {
                if (c == '"' && lastChar != '\\')
                {
                    openStr = !openStr;
                }

                if (c == ' ' && !openStr)
                {
                    lastChar = c;
                    continue;
                }

                lastChar = c;
                nStr += c;
            }

            return nStr;
        }

        public Json Get(string name, string alter)
        {
            if (!name.StartsWith('"') && !name.EndsWith('"'))
            {
                name = $"\"{name}\"";
            }

            if (Objs.ContainsKey(name))
            {
                string obj = Objs[name];
                if (obj.StartsWith('[') && obj.EndsWith(']') || obj.StartsWith("{") && obj.EndsWith("}"))
                {
                    return BuildByString(obj);
                }
                else
                {
                    return BuildByString(alter);
                }
            }
            else
            {
                return BuildByString(alter);
            }
        }

        public Json Get(string name)
        {
            return Get(name, "[]");
        }

        public string GetAsText(string name, string alter)
        {
            if (!name.StartsWith('"') && !name.EndsWith('"'))
            {
                name = $"\"{name}\"";
            }

            if (Objs.ContainsKey(name))
            {
                string obj = Objs[name];
                if (obj.StartsWith('"') && obj.EndsWith('"'))
                {
                    return obj.Substring(1, obj.Length - 2);
                }
                else
                {
                    return alter;
                }
            }
            else
            {
                return alter;
            }
        }

        public string GetAsText(string name)
        {
            return GetAsText(name, "");
        }

        public float GetAsFloat(string name, float alter)
        {
            if (!name.StartsWith('"') && !name.EndsWith('"'))
            {
                name = $"\"{name}\"";
            }

            if (Objs.ContainsKey(name))
            {
                string obj = Objs[name];
                if (obj.StartsWith('"') && obj.EndsWith('"'))
                {
                    return float.Parse(obj);
                }
                else
                {
                    return alter;
                }
            }
            else
            {
                return alter;
            }
        }

        public float GetAsFloat(string name)
        {
            return GetAsFloat(name, 0F);
        }

        public bool GetAsBool(string name, bool alter = false)
        {
            if (!name.StartsWith('"') && !name.EndsWith('"'))
            {
                name = $"\"{name}\"";
            }

            if (Objs.ContainsKey(name))
            {
                string obj = Objs[name];
                if (obj.StartsWith('"') && obj.EndsWith('"'))
                {
                    return alter;
                }

                if (obj != "true" && obj != "false")
                {
                    return alter;
                }

                return obj == "true";
            }
            else
            {
                return alter;
            }
        }


        public int GetAsInt(string name, int alter)
        {
            if (!name.StartsWith('"') && !name.EndsWith('"'))
            {
                name = $"\"{name}\"";
            }

            if (Objs.ContainsKey(name))
            {
                string obj = Objs[name];
                if (obj.StartsWith('"') && obj.EndsWith('"'))
                {
                    return int.Parse(obj);
                }
                else
                {
                    return alter;
                }
            }
            else
            {
                return alter;
            }
        }

        public int GetAsInt(string name)
        {
            return GetAsInt(name, 0);
        }

        public static Json BuildByString(string str)
        {
            return new Json(str);
        }

        public static bool IsList(Json json)
        {
            return json.IsArray;
        }

        public static int Length(Json json)
        {
            if (!json.IsArray)
            {
                throw new Exception("Cannot read json length of non array");
            }

            return json.Objs.Count;
        }

        public static Dictionary<string, string>.KeyCollection Keys(Json json)
        {
            if (json.IsArray)
            {
                throw new Exception("Array has no keys use length instate");
            }

            return json.Objs.Keys;
        }

        public static bool KeyExists(Json json, string key)
        {
            if (!key.StartsWith('"') && !key.EndsWith('"'))
            {
                key = $"\"{key}\"";
            }

            if (json.IsArray)
            {
                throw new Exception("Cannot read keys of array");
            }

            return json.Objs.ContainsKey(key);
        }
    }
}