using Aga.Controls.Tree;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using JsObj = System.Collections.Generic.Dictionary<string, object>;

namespace QtSpy.UI
{
    public partial class FormMain : Form
    {
        const string TitlePrefix = "QtSpy";
        static readonly uint CurrentPid = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;
        static Cursor Crosshair = new Cursor(new MemoryStream(Properties.Resources.cursor1));

        bool ShowHidden = false;
        bool Searching = false;
        Size SavedSize;
        QtProcess LastProcess;
        IntPtr LastWindow = IntPtr.Zero;
        Rectangle LastRect = Rectangle.Empty;

        public FormMain()
        {
            InitializeComponent();
            Text = TitlePrefix;
        }

        void InvertLast()
        {
            if (LastWindow != IntPtr.Zero) {
                if (LastRect.IsEmpty)
                    UiTools.InvertWindow(LastWindow, ShowHidden);
                else
                    UiTools.InvertScreenRect(LastWindow, LastRect);
            }
        }

        private void picturePicker_MouseDown(object sender, MouseEventArgs e)
        {
            InvertLast();
            LastWindow = IntPtr.Zero;
            LastRect = Rectangle.Empty;
            Cursor.Current = Crosshair;
            picturePicker.Image = Properties.Resources.bitmap2;
            if (checkMinimize.Checked) {
                SavedSize = ClientSize;
                ClientSize = new Size(ClientSize.Width, panel1.Top);
            }
            Searching = true;
        }

        private void picturePicker_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Searching)
                return;
            var w = Winapi.WindowFromPoint(Cursor.Position);

            uint pid;
            Winapi.GetWindowThreadProcessId(w, out pid);
            if (pid == CurrentPid)
                return;

            var json = GetWidgetData(w, false);
            var rect = GetWidgetRect(json);

            if (LastWindow != w || LastRect != rect) {
                // unhilite old
                InvertLast();
                LastWindow = w;
                LastRect = rect;
                // hilite new
                InvertLast();
                // also update window title
                Text = TitlePrefix + string.Format(" [{0:X8}, {1}, {2}]", w, json.TryGet<string>("class"), json.TryGet<string>("text"));
            }
        }

        private void picturePicker_MouseUp(object sender, MouseEventArgs e)
        {
            InvertLast();
            Searching = false;
            if (checkMinimize.Checked)
                ClientSize = SavedSize;

            dataGridParams.Rows.Clear();

            var json = GetWidgetData(LastWindow, true);
            if (json != null) {
                MyNode tagged;
                var root = QtObject.ParseJson<MyNode>(json, out tagged);
                var model = MyTreeModel.FromNode(root);
                treeWidgets.Model = model;

                if (tagged != null) {
                    treeWidgets.SelectedNode = treeWidgets.FindNode(model.GetPath(tagged));
                    treeWidgets.EnsureVisible(treeWidgets.SelectedNode);
                    treeWidgets.Focus();
                }
            }

            LastWindow = IntPtr.Zero;
            picturePicker.Image = Properties.Resources.bitmap1;
        }

        private JsObj GetWidgetData(IntPtr w, bool getTree)
        {
            LastProcess = QtProcess.FromWindow(w);
            if (LastProcess == null)
                return null;
            var pt = Cursor.Position;
            return LastProcess.ExecCommand(new {
                cmd = getTree ? "treeAtPoint" : "atPoint",
                hWnd = (int)w,
                x = pt.X,
                y = pt.Y,
                onlyWidgets = checkOnlyWidgets.Checked
            });
        }

        private Rectangle ParseRect(JsObj json)
        {
            var bounds = json.TryGet<JsObj>("bounds");
            if (bounds != null)
                return new Rectangle((int)bounds["x"], (int)bounds["y"], (int)bounds["w"], (int)bounds["h"]);
            return Rectangle.Empty;
        }

        private Rectangle GetWidgetRect(JsObj json)
        {
            var err = json.TryGet<string>("error");
            if (err == null) {
                var bounds = json.TryGet<JsObj>("bounds");
                if (bounds != null) {
                    var rect = ParseRect(json);
                    var offset = json.TryGet<JsObj>("screenOffset");
                    rect.Location = new Point((int)offset["x"], (int)offset["y"]);
                    return rect;
                }
            }
            return Rectangle.Empty;
        }

        private void treeWidgets_SelectionChanged(object sender, EventArgs e)
        {
            var sNode = treeWidgets.SelectedNode;
            if (sNode == null)
                return;

            var node = (MyNode)sNode.Tag;

            dataGridParams.Rows.Clear();
            foreach (var kvp in node.Json) {
                string value = "";
                switch (kvp.Key) {
                    case "winId":
                    case "ptr":
                        value = ((int)kvp.Value).ToString("X8");
                        break;
                    case "attr":
                        value = string.Join(", ", ((object[])kvp.Value).Select(x => ((WidgetAttributes)(int)x).ToString()));
                        break;
                    default:
                        value = new JavaScriptSerializer().Serialize(kvp.Value);
                        break;
                }
                dataGridParams.Rows.Add(new[] { kvp.Key, value });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridParams.RowTemplate.Height = 18;
        }

        private void dataGridParams_DoubleClick(object sender, EventArgs e)
        {
            var sr = dataGridParams.SelectedRows;
            if (sr.Count == 0)
                return;
            var row = sr[0];
            if (row.Cells[0].Value as string == "class") {
                var clsName = ((MyNode)treeWidgets.SelectedNode.Tag).Class;
                if (clsName[0] == 'Q')
                    System.Diagnostics.Process.Start("http://qt-project.org/doc/qt-4.8/" + clsName.ToLowerInvariant() + ".html");
            }
        }

        private void treeWidgets_DrawControl(object sender, Aga.Controls.Tree.NodeControls.DrawEventArgs e)
        {
            var node = (MyNode)e.Node.Tag;
            if (!node.Visible)
                e.TextColor = e.Node.IsSelected ? Color.White : Color.Gray;
        }

        private void treeWidgets_NodeMouseClick(object sender, TreeNodeAdvMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
                var sNode = treeWidgets.SelectedNode;
                if (sNode == null)
                    return;

                var node = (MyNode)sNode.Tag;
                menuItemVisible.Checked = node.Visible;
                menuItemEnabled.Enabled = false;
                menuItemDump.Enabled = node.GetSuperClasses().Contains("QAbstractItemView");

                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void menuItemVisible_Click(object sender, EventArgs e)
        {
            var node = (MyNode)treeWidgets.SelectedNode.Tag;
            var newVisible = !menuItemVisible.Checked;
            LastProcess.ExecCommand(new {
                cmd = "setVisible",
                ptr = node.Ptr,
                visible = newVisible
            });
            node.Visible = newVisible;
            node.Model.OnNodeChanged(node);
        }

        private void menuItemDump_Click(object sender, EventArgs e)
        {
            var node = (MyNode)treeWidgets.SelectedNode.Tag;
            var json = LastProcess.ExecCommand(new {
                cmd = "dumpTable",
                ptr = node.Ptr,
            });
            var data = json.TryGet<object[]>("data").Cast<object[]>().ToArray();
            var tsv = string.Join("\r\n", data.Select(row => string.Join("\t", row.Select(s => TsvSafe((s as string) ?? "<null>")))));
            Clipboard.SetText(tsv);
            MessageBox.Show(this, "Content of the table has been copied to your clipboard.", "QtSpy");
        }

        private string TsvSafe(string s)
        {
            return s.Replace("\t", "\\t").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\0", "\\0");
        }

        //private Point GetClientOffset(Node node)
        //{
        //    var result = new Point();
        //    while (node != null && node.Parent != treeModel.Root) {
        //        var rect = ParseRect((JsObj)node.Tag);
        //        result.Offset(rect.Location);
        //        node = node.Parent;
        //    }
        //    return result;
        //}

        private void treeWidgets_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //InvertLast();
            //lastWindow = IntPtr.Zero;

            //var sNode = treeWidgets.SelectedNode;
            //if (sNode == null) {
            //    SelectedNode = null;
            //    return;
            //}

            //SelectedNode = (JsObj)((Node)sNode.Tag).Tag;

            //if (SelectedNode.TryGet<object>("invisible") != null)
            //    return;

            //lastWindow = (IntPtr)(int)((JsObj)(treeModel.Root.Nodes[0].Tag))["winId"];
            //var rect = ParseRect(SelectedNode);
            //rect.Location = new Window(lastWindow).ClientToScreen(GetClientOffset((Node)sNode.Tag));
            //lastRect = rect;
            //InvertLast();
        }

        private void treeWidgets_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) {
                treeWidgets_MouseDoubleClick(null, null);
            }
        }
    }
}
