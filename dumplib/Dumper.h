﻿#include <map>
#include <QtGui>
#include <QtWebKit>
#include "ObjectRepo.h"
#include "rapidjson/document.h"

class Dumper
{
public:
    void ProcessCmd(uint8_t* buff, DWORD len, HANDLE output)
    {
        rapidjson::Document doc;
        PipeStream outStream(output);
        MyWriter writer(outStream);
        writer.StartObject();

        buff[len] = '\0';
        if (doc.ParseInsitu<0>(reinterpret_cast<char*> (buff)).HasParseError()) {
            writer.String("error").String("Cannot parse command");
        } else {
            const Value& cmd = doc["cmd"];
            auto cmdLen = cmd.GetStringLength();
            auto cmdData = cmd.GetString();

#define IS_CMD(cmd) (cmdLen == (_countof(cmd) - 1) && ::memcmp(cmdData, cmd, _countof(cmd) - 1) == 0)

            if (IS_CMD("atPoint")) {
                dumpAtPoint(writer, doc["hWnd"].GetUint(), doc["x"].GetInt(), doc["y"].GetInt());
            } else if (IS_CMD("treeAtPoint")) {
                dumpTreeAtPoint(writer, doc["hWnd"].GetUint(), doc["x"].GetInt(), doc["y"].GetInt(), doc["onlyWidgets"].GetBool());
            } else if (IS_CMD("setVisible")) {
                setVisible(writer, doc["ptr"].GetUint(), doc["visible"].GetBool());
            } else if (IS_CMD("dumpTable")) {
                dumpTable(writer, doc["ptr"].GetUint());
            } else if (IS_CMD("execJs")) {
                execJs(writer, doc["ptr"].GetUint(), doc["code"].GetString());
            } else {
                writer.String("error").String("Unknown command");
            }

#undef IS_CMD

        }

        writer.EndObject();
        outStream.Put('\n');
    }

    typedef void(*dumper_t)(MyWriter& writer, const QObject& obj);

private:
    static std::map<std::string, dumper_t> dumpers;

    void dumpRect(MyWriter& writer, const QRect& rect)
    {
        writer.StartObject();
        writer.String("x").Int(rect.x());
        writer.String("y").Int(rect.y());
        writer.String("w").Int(rect.width());
        writer.String("h").Int(rect.height());
        writer.EndObject();
    }

    void dumpPoint(MyWriter& writer, const QPoint& pt)
    {
        writer.StartObject();
        writer.String("x").Int(pt.x());
        writer.String("y").Int(pt.y());
        writer.EndObject();
    }

    void dumpClasses(MyWriter& writer, const QObject& object)
    {
        auto metaObj = object.metaObject();
        writer.String("class").String(metaObj->className());
        writer.String("super").StartArray();
        while (metaObj) {
            writer.String(metaObj->className());
            metaObj = metaObj->superClass();
        }
        writer.EndArray();
    }

    void dumpSpecific(MyWriter& writer, const QObject& object)
    {
        auto metaObj = object.metaObject();

        while (metaObj) {
            auto cn = metaObj->className();
            auto dumper = dumpers[cn];
            if (dumper)
                dumper(writer, object);
            metaObj = metaObj->superClass();
        }

    }

    bool isActionType(const QObject *obj)
    {
        auto metaObj = obj->metaObject();
        return metaObj && (QString(metaObj->className()) == QString("QAction"));
    }

    void dumpNodeData(MyWriter& writer, const QObject& object)
    {
        ObjectRepo::Instance()->Add(object);
        writer.String("ptr").Int(reinterpret_cast<int>(&object));

        writer.String("name").String(object.objectName().toUtf8().data());
        dumpClasses(writer, object);

        if (object.isWidgetType()) {
            auto w = static_cast<const QWidget*>(&object);
            //result["accName"] = w->accessibleName().toUtf8().data(); // this fails sometimes

            writer.String("attr").StartArray();
            for (int i = 0; i < Qt::WA_AttributeCount; i++)
                if (w->testAttribute(static_cast<Qt::WidgetAttribute>(i)))
                    writer.Int(i);
            writer.EndArray();

            auto winId = w->internalWinId();
            if (winId)
                writer.String("winId").Int(reinterpret_cast<int>(winId));

            if (w->isWindow())
                writer.String("windowTitle").String(w->windowTitle().toUtf8().data());

            dumpRect(writer.String("bounds"), w->frameGeometry());

            if (w->isVisible())
                writer.String("visible").Bool(true);

            dumpSpecific(writer, object);
        }
        else if (isActionType(&object)){
            auto act = static_cast<const QAction*>(&object);
            writer.String("text").String(act->text().toUtf8().data());
            if (act->isVisible()){
                writer.String("visible").Bool(act->isVisible());
            }
        }

    }

    // this crap is needed to access protected d_ptr
    struct QObject_f : QObject
    {
        const QObjectData* getDPtr() { return this->d_ptr.data(); }
    };

    bool isOk(const QObject& object)
    {
        auto objPtr = (QObject*) &object;
        if (objPtr != nullptr) {
            auto objData = reinterpret_cast<QObject_f*>(objPtr)->getDPtr();
            return objData && objData->q_ptr == objPtr;
        }
        return false;
    }


    void dumpRecursive(MyWriter& writer, const QObject& object, const QObject& tagged, bool onlyWidgets, bool wrapObject)
    {
        if (isOk(object) && (!onlyWidgets || object.isWidgetType() || isActionType(&object))) {
            if (wrapObject)
                writer.StartObject();
            dumpNodeData(writer, object);

            if (&object == &tagged)
                writer.String("__tagged").Bool(true);

            auto children = object.children();

            if (object.isWidgetType()){
                const QWidget *w = static_cast<const QWidget *>(&object);
                auto acts = w->actions();
                for (int i = 0; i < acts.count(); i++){
                    if (!children.contains(acts[i])){
                        children.push_back(acts[i]);
                    }
                }
            }

            if (!children.isEmpty()) {
                writer.String("__children").StartArray();
                for (int i = 0; i < children.size(); ++i)
                    dumpRecursive(writer, *children.at(i), tagged, onlyWidgets, true);
                writer.EndArray();
            }

            if (wrapObject)
                writer.EndObject();
        }
    }

    void dumpAllTrees(MyWriter& writer)
    {
        //auto list = QApplication::topLevelWidgets();
        //for (auto i = list.begin(); i != list.end(); ++i) {
        //    auto obj = dumpRecursive(**i, **i);
        //    obj["screenOffset"] = dumpPoint((*i)->mapToGlobal(QPoint(0, 0)));
        //}
    }

    void dumpAtPoint(MyWriter& writer, DWORD hWnd, int x, int y)
    {
        auto wnd = QWidget::find(reinterpret_cast<WId>(hWnd));
        if (!wnd) {
            writer.String("error").String("Cant find window");
            return;
        }
        auto pt = wnd->mapFromGlobal(QPoint(x, y));
        auto widget = wnd->childAt(pt);
        if (!widget)
            widget = wnd;
        dumpNodeData(writer, *widget);
        dumpPoint(writer.String("screenOffset"), widget->mapToGlobal(QPoint(0, 0)));
    }

    void dumpTreeAtPoint(MyWriter& writer, DWORD hWnd, int x, int y, bool onlyWidgets)
    {
        ObjectRepo::Instance()->Lock();

        auto wnd = QWidget::find(reinterpret_cast<WId>(hWnd));
        if (!wnd) {
            writer.String("error").String("Cant find window");
        } else {
            // find root
            wnd = wnd->window();
            auto pt = wnd->mapFromGlobal(QPoint(x, y));
            auto tagged = wnd->childAt(pt);
            if (!tagged)
                tagged = wnd;
            dumpRecursive(writer, *wnd, *tagged, onlyWidgets, false);
            dumpPoint(writer.String("screenOffset"), wnd->mapToGlobal(QPoint(0, 0)));
        }

        ObjectRepo::Instance()->Unlock();
    }

    void setVisible(MyWriter& writer, DWORD ptr, bool visible)
    {
        ObjectRepo::Instance()->Lock();

        auto widget = reinterpret_cast<QWidget*>(ptr);
        if (!isOk(*widget) || !ObjectRepo::Instance()->IsAlive(*widget)) {
            writer.String("error").String("Dead or unknown object");
        } else {
            widget->setVisible(visible);
            dumpNodeData(writer, *widget);
            dumpPoint(writer.String("screenOffset"), widget->mapToGlobal(QPoint(0, 0)));
        }

        ObjectRepo::Instance()->Unlock();
    }

    void execJs(MyWriter& writer, DWORD ptr, const char* code)
    {
        ObjectRepo::Instance()->Lock();

        auto widget = reinterpret_cast<QWidget*>(ptr);
        if (!isOk(*widget) || !ObjectRepo::Instance()->IsAlive(*widget)) {
            writer.String("error").String("Dead or unknown object");
        } else {
            if (!widget->inherits("QWebView")) {
                writer.String("error").String("Widget is not QWebView");
            } else {
                auto webView = reinterpret_cast<QWebView*>(widget);
                auto page = webView->page();
                if (!page || !page->currentFrame()) {
                    writer.String("error").String("No page in web view");
                } else {
                    // evaluateJavaScript can be called only from UI thread
                    QVariant retVal;
                    QMetaObject::invokeMethod(
                        page->currentFrame(),
                        "evaluateJavaScript",
                        Qt::BlockingQueuedConnection,
                        QReturnArgument<QVariant>("QVariant", retVal),
                        QArgument<QString>("QString", QString::fromUtf8(code))
                        );
                    writer.String("data").String(retVal.toString().toUtf8().data());
                }
            }
        }

        ObjectRepo::Instance()->Unlock();
    }

    void dumpTable(MyWriter& writer, DWORD ptr)
    {
        ObjectRepo::Instance()->Lock();

        auto widget = reinterpret_cast<QAbstractItemView*>(ptr);
        if (!isOk(*widget) || !ObjectRepo::Instance()->IsAlive(*widget)) {
            writer.String("error").String("Dead or unknown object");
        } else {
            auto model = widget->model();
            if (!model) {
                writer.String("error").String("No model");
            } else {
                int rowCount = model->rowCount();
                int columnCount = model->columnCount();
                writer.String("data").StartArray();
                for (int r = 0; r < rowCount; r++) {
                    writer.StartArray();
                    for (int c = 0; c < columnCount; c++) {
                        auto data = model->data(model->index(r, c));
                        writer.String(data.toString().toUtf8().data());
                    }
                    writer.EndArray();
                }
                writer.EndArray();
            }
        }

        ObjectRepo::Instance()->Unlock();
    }
}; // class Dumper

#define DUMPER_STUB(name, body) {                   \
#name, [](MyWriter& writer, const QObject& obj) {   \
    auto w = static_cast<const name*>(&obj);        \
    body;                                           \
}                                                   \
}

std::map<std::string, Dumper::dumper_t> Dumper::dumpers = {
    DUMPER_STUB(QLabel, {
        writer.String("text").String(w->text().toUtf8().data());
    }),
    DUMPER_STUB(QAbstractButton, {
        writer.String("text").String(w->text().toUtf8().data());
        writer.String("checked").Bool(w->isChecked());
    }),
    DUMPER_STUB(QLineEdit, {
        writer.String("text").String(w->text().toUtf8().data());
    }),
    DUMPER_STUB(QComboBox, {
        writer.String("text").String(w->currentText().toUtf8().data());
    }),
    DUMPER_STUB(QAbstractSlider, {
        writer.String("value").Int(w->value());
    }),
    DUMPER_STUB(QGroupBox, {
        writer.String("title").String(w->title().toUtf8().data());
    }),
    DUMPER_STUB(QTextEdit, {
        auto acceptRich = w->acceptRichText();
        writer.String("acceptRichText").Bool(acceptRich);
        writer.String("plainText").String(w->toPlainText().toUtf8().data());
        if (acceptRich)
            writer.String("html").String(w->toHtml().toUtf8().data());
    }),
    DUMPER_STUB(QWebView, {
        writer.String("url").String(w->url().toString().toUtf8().data());
    }),
    DUMPER_STUB(QAbstractItemView, {
        auto model = w->model();
        if (model) {
            writer.String("columnCount").Int(model->columnCount());
            writer.String("rowCount").Int(model->rowCount());
        }
    }),
};

#undef DUMPER_STUB
