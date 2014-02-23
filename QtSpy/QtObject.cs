using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QtSpy
{
    public sealed class QtObject : QtObject<QtObject>
    {
        public static QtObject ParseJson(Dictionary<string, object> json, out QtObject tagged)
        {
            return ParseJson<QtObject>(json, out tagged);
        }

        public static U ParseJson<U>(Dictionary<string, object> json, out U tagged) where U : QtObject<U>, new()
        {
            var taggedArr = new U[1];
            var result = QtObject<U>.ParseJson(json, null, taggedArr);
            tagged = taggedArr[0];
            return result;
        }
    }

    public class QtObject<T> where T : class
    {
        public Dictionary<string, object> Json { get; private set; }
        public string Class { get; private set; }
        public string Name { get; private set; }
        public int? WinId { get; private set; }
        public int Ptr { get; private set; }
        public List<T> Children { get; private set; }
        public bool Visible { get; set; }

        protected WeakReference parent = new WeakReference(null);
        public T Parent { get { return parent.IsAlive ? parent.Target as T : null; } }

        public QtObject()
        {
            Children = new List<T>();
        }

        internal static U ParseJson<U>(Dictionary<string, object> json, U parent, U[] tagged) where U : QtObject<U>, new()
        {
            var node = new U {
                parent = new WeakReference(parent),
                Json = json,
                Class = json.TryGet<string>("class"),
                Name = json.TryGet<string>("name"),
                WinId = json.TryGet<int?>("winId"),
                Ptr = json.TryGet<int>("ptr"),
            };

            var visible = json.TryGet<bool?>("visible");
            node.Visible = visible.HasValue && visible.Value;

            if (json.TryGet<object>("__tagged") != null)
                tagged[0] = node;
            json.TryRemove("__tagged");

            var children = json.TryGet<object[]>("__children");
            if (children != null) {
                node.Children = children.Select(c => ParseJson((Dictionary<string, object>)c, node, tagged)).ToList();
            } else {
                node.Children = new List<U>();
            }
            json.TryRemove("__children");

            return node;
        }

        public string GetDescription()
        {
            var result = new StringBuilder();
            result.Append(Class);

            if (!string.IsNullOrEmpty(Name)) {
                result.Append(' ');
                result.Append(Name.ToLiteral());
            }

            var title = Json.TryGet<string>("title") ?? Json.TryGet<string>("windowTitle");
            if (title != null) {
                result.Append(", title=");
                result.Append(title.ToLiteral());
            }

            var text = Json.TryGet<string>("text") ?? Json.TryGet<string>("plainText");
            if (text != null) {
                result.Append(", text=");
                result.Append(text.ToLiteral());
            }

            if (WinId.HasValue) {
                result.Append(", hWnd=");
                result.Append(WinId.Value.ToString("X8"));
            }

            return result.ToString();
        }

        public string[] GetSuperClasses()
        {
            var super = Json.TryGet<object[]>("super");
            if (super == null)
                return new string[0];
            return super.Cast<string>().ToArray();
        }
    }
}
