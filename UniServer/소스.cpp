//유니서버

//-------------------------------------------------------------
//	[  UDPServer.cpp ]
//-------------------------------------------------------------





#include <stdlib.h>	
#include <stdio.h>
#include"wbsock.h"


#define SERVER_PORT 9000

//네트워크에서 오는 로그정보를 받는함수
void logmessage(const char* info, const char* msg)
{
	printf("[%s] %s", info, msg);
}


//네트워크에서 오는 메시지 정보를 받는 함수
//1함수정의
void shortmessage(const char* ip, int port,  char* msg, int size)
{
	msg[size] = '\0';
	printf("[UDP/%s:%d] %s\n", ip,port,msg);

}
int main(int argc, char* argv[])
{
	//2.함수주소를 전달
	wbsock_init(logmessage, shortmessage);

	int sock = wbsock_createsocket(SERVER_PORT);


	wbsock_run(sock);

	wbsock_closesocket(sock);

	wbsock_exit();



	return 0;
}
