#include "Main.h"
#include <cassert>
#include <cstring>
#include <nn.h>
#include <survey.h>
#include <reqrep.h>
#include <stdio.h>

using namespace std;

typedef struct Functions
{
	void (*DataReceived)(char* buffer, int length);
	void (*WriteLine)(const char* msg);
} NanoFunctions;

typedef struct Ret
{
	char* buffer;
	int length;
};

int recvSocket;
int reqSocket;

EXPORT void StartEventClient(const char* url, NanoFunctions functions)
{
	recvSocket = nn_socket(AF_SP, NN_RESPONDENT);
	char t[28];
	sprintf_s(t, "%d", recvSocket);
	assert(recvSocket >= 0);
	assert(nn_connect(recvSocket, url) >= 0);
	functions.WriteLine("[INFO] Started event thread.");
	while (1)
	{
		char* buf = NULL;
		int bytes = nn_recv(recvSocket, &buf, NN_MSG, 0);
		if (bytes >= 0)
		{
			functions.DataReceived(buf, bytes);
			nn_freemsg(buf);
		}
	}
}

EXPORT void StartFuncClient(const char* url, NanoFunctions functions)
{
	reqSocket = nn_socket(AF_SP, NN_REQ);
	assert(reqSocket >= 0);
	assert(nn_connect(reqSocket, url) >= 0);
	functions.WriteLine("[INFO] Started API thread.");
}

EXPORT void SendFuncData(unsigned short* buf, int bufSize)
{
	int bytes = nn_send(reqSocket, buf, bufSize, 0);
}

EXPORT Ret RecvFuncData()
{
	while (1)
	{
		char* buf = NULL;
		char* retBuf = NULL;
		int bytes = nn_recv(reqSocket, &buf, NN_MSG, 0);
		if (bytes >= 0)
		{
			retBuf = buf;
			nn_freemsg(buf);
		}

		Ret retVal;
		retVal.buffer = retBuf;
		retVal.length = bytes;

		return retVal;
	}
}

EXPORT void SendEventData(const char* msg)
{
	char d[8];

	sprintf_s(d, "%s\0", msg);

	int size = strlen(d) + 1;

	int bytes = nn_send(recvSocket, d, size, 0);
	assert(bytes == size);
}

EXPORT int StopEventClient()
{
	return nn_shutdown(recvSocket, 0);
}

EXPORT int StopFuncClient()
{
	return nn_shutdown(reqSocket, 0);
}