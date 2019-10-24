#include "Main.h"
#include <cassert>
#include <cstring>
#include <nn.h>
#include <survey.h>
#include <stdio.h>

using namespace std;

typedef struct Functions
{
	void (*DataReceived)(char* buffer, int length);
	void (*WriteLine)(const char* msg);
} NanoFunctions;

int socket;

EXPORT void StartClient(const char* url, NanoFunctions functions)
{
	socket = nn_socket(AF_SP, NN_RESPONDENT);
	char t[28];
	sprintf_s(t, "%d", socket);
	assert(socket >= 0);
	assert(nn_connect(socket, url) >= 0);
	functions.WriteLine("[INFO] Started listener thread.");
	while (1)
	{
		char* buf = NULL;
		int bytes = nn_recv(socket, &buf, NN_MSG, 0);
		if (bytes >= 0)
		{
			functions.DataReceived(buf, bytes);
			nn_freemsg(buf);
		}
	}
}

EXPORT void SendData(const char* msg)
{
	char d[8];

	sprintf_s(d, "%s\0", msg);

	int size = strlen(d) + 1;

	int bytes = nn_send(socket, d, size, 0);
	assert(bytes == size);
}

EXPORT int StopClient()
{
	return nn_shutdown(socket, 0);
}