using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace QtSpy.UI
{
    public class MyNode : QtObject<MyNode>
    {
        public MyTreeModel Model { get; internal set; }

        public void SetParent(MyNode newParent)
        {
            parent = new WeakReference(newParent);
        }

        public string Description { get { return base.GetDescription().Replace("&", "&&"); } }

        // NOTE: inherited widgets must appear BEFORE their super-classes
        static List<Tuple<string, Image>> icons = new[] {
            "QCheckBox",
            "QLabel",
            "QLineEdit",
            "QTextEdit",
            "QRadioButton",
            "QComboBox",
            "QPushButton",
            "QToolButton",
            "QTabBar",
            "QTableView",
            "QAbstractItemView",
            "QFrame",
            "QWebView",
            "QAction",
        }.Select(k => Tuple.Create(k, (Image)(Bitmap)Properties.Resources.ResourceManager.GetObject(k))).ToList();

        Image icon;
        public Image Icon
        {
            get
            {
                if (icon == null) {
                    var super = GetSuperClasses();
                    foreach (var i in icons) {
                        if (super.Contains(i.Item1)) {
                            icon = i.Item2;
                            break;
                        }
                    }
                    if (icon == null)
                        icon = Properties.Resources.QWidget;
                }
                return icon;
            }
        }
    }
}
