//���ϼ���

//-------------------------------------------------------------
//	[  UDPServer.cpp ]
//-------------------------------------------------------------





#include <stdlib.h>	
#include <stdio.h>
#include"wbsock.h"


#define SERVER_PORT 9000

//��Ʈ��ũ���� ���� �α������� �޴��Լ�
void logmessage(const char* info, const char* msg)
{
	printf("[%s] %s", info, msg);
}


//��Ʈ��ũ���� ���� �޽��� ������ �޴� �Լ�
//1�Լ�����
void shortmessage(const char* ip, int port,  char* msg, int size)
{
	msg[size] = '\0';
	printf("[UDP/%s:%d] %s\n", ip,port,msg);

}
int main(int argc, char* argv[])
{
	//2.�Լ��ּҸ� ����
	wbsock_init(logmessage, shortmessage);

	int sock = wbsock_createsocket(SERVER_PORT);


	wbsock_run(sock);

	wbsock_closesocket(sock);

	wbsock_exit();



	return 0;
}
