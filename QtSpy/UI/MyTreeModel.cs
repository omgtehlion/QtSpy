using Aga.Controls.Tree;
using System;
using System.Collections.Generic;

namespace QtSpy.UI
{
    public class MyTreeModel : ITreeModel
    {
        public MyNode Root { get; private set; }

        public static MyTreeModel FromNode(MyNode node)
        {
            var result = new MyTreeModel();
            result.Root.Children.Add(node);
            node.SetParent(result.Root);
            result.Accept(node);
            return result;
        }

        void Accept(MyNode node)
        {
            node.Model = this;
            foreach (var c in node.Children)
                Accept(c);
        }

        public MyTreeModel()
        {
            Root = new MyNode();
            Root.Model = this;
        }

        public TreePath GetPath(MyNode node)
        {
            if (node == Root)
                return TreePath.Empty;
            var stack = new Stack<object>();
            while (node.Parent != null) {
                stack.Push(node);
                node = (MyNode)node.Parent;
            }
            return new TreePath(stack.ToArray());
        }

        public MyNode FindNode(TreePath path)
        {
            return path.IsEmpty() ? Root : path.LastNode as MyNode;
        }

        #region ITreeModel Members

        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            var node = FindNode(treePath);
            if (node != null)
                return node.Children;
            return new object[0];
        }

        public bool IsLeaf(TreePath treePath)
        {
            var node = FindNode(treePath);
            if (node == null)
                throw new ArgumentException("Node not found", "treePath");
            return node.Children.Count == 0;
        }

        public event EventHandler<TreeModelEventArgs> NodesChanged;
        internal void OnNodesChanged(TreeModelEventArgs args)
        {
            if (NodesChanged != null)
                NodesChanged(this, args);
        }
        internal void OnNodeChanged(MyNode node)
        {
            if (NodesChanged != null) {
                var parent = node.Parent;
                TreeModelEventArgs args = new TreeModelEventArgs(GetPath(parent), new int[] { parent.Children.IndexOf(node) }, new object[] { node });
                NodesChanged(this, args);
            }

        }

        public event EventHandler<TreePathEventArgs> StructureChanged;
        public void OnStructureChanged(TreePathEventArgs args)
        {
            if (StructureChanged != null)
                StructureChanged(this, args);
        }

        public event EventHandler<TreeModelEventArgs> NodesInserted;
        internal void OnNodeInserted(MyNode parent, int index, MyNode node)
        {
            if (NodesInserted != null) {
                TreeModelEventArgs args = new TreeModelEventArgs(GetPath(parent), new int[] { index }, new object[] { node });
                NodesInserted(this, args);
            }

        }

        public event EventHandler<TreeModelEventArgs> NodesRemoved;
        internal void OnNodeRemoved(MyNode parent, int index, MyNode node)
        {
            if (NodesRemoved != null) {
                TreeModelEventArgs args = new TreeModelEventArgs(GetPath(parent), new int[] { index }, new object[] { node });
                NodesRemoved(this, args);
            }
        }

        #endregion
    }
}
