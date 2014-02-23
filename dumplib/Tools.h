#define STRICT
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <inttypes.h>

#define BUFSIZE 4096

#include "rapidjson/writer.h"
namespace rapidjson {
    class PipeStream {
    private:
        HANDLE handle;
        char buff[BUFSIZE];
        int cnt;
    public:
        PipeStream(HANDLE h) : handle(h), cnt(0) {}
        void inline Put(char c) {
            buff[cnt++] = c;
            if (cnt == BUFSIZE)
                Flush();
        }
        void Flush() {
            DWORD written;
            ::WriteFile(handle, buff, cnt, &written, NULL);
            cnt = 0;
        }
        ~PipeStream() {
            if (cnt)
                Flush();
            ::FlushFileBuffers(handle);
        }
    };
} // namespace rapidjson
using namespace rapidjson;
typedef Writer<PipeStream> MyWriter;
