class ObjectRepo
{
public:
    ObjectRepo()
    {
        //Real_qt_removeObject = (qt_removeObject_t) InjectDetour(qt_removeObject, GetProcAddress(GetModuleHandleW(L"QtCore4.dll"), "qt_removeObject"));
        // qt_removeObject seems to be inlined into QObject::~QObject, so we better hook this
        auto dtorPtr = GetProcAddress(GetModuleHandleW(L"QtCore4.dll"), "??1QObject@@UAE@XZ");
        if (dtorPtr) {
            auto dtorFake = &ObjectRepo::QObject_dtor;
            ::InjectDetour((LPVOID&) dtorFake, dtorPtr, (LPVOID*) &Real_QObject_dtor);
        }
    }

    bool IsAlive(const QObject& obj)
    {
        return true;
    }

    void Remove(const QObject& obj)
    {
        //OutputDebugStringW(L"QObject::~QObject()\n");
    }

    void Add(const QObject& obj)
    {
    }

    static ObjectRepo* Instance()
    {
        return instance;
    }

    static void Init()
    {
        assert(!instance);
        instance = new ObjectRepo();
    }

private:
    static ObjectRepo* instance;
    static void(__thiscall *Real_QObject_dtor)(QObject* this_);
    void QObject_dtor()
    {
        // WARNING: `this` points to QObject!
        auto obj = reinterpret_cast<QObject*>(this);
        if (instance)
            instance->Remove(*obj);
        Real_QObject_dtor(obj);
    }
}; // class ObjectRepo

decltype(ObjectRepo::Real_QObject_dtor) ObjectRepo::Real_QObject_dtor = NULL;
ObjectRepo* ObjectRepo::instance = NULL;
