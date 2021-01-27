


#define _WINSOCK_DEPRECATED_NO_WARNINGS

#include<stdio.h>
#include"wbsock.h"

#define BUFSIZE 512
//4.함수의 주소를 담을 전역변수 선언
LOMESSAGE g_LogMessage = NULL;
SHORTMESSAGE g_ShortMessage = 0;

// 외부에서 접근하는 함수
int wbsock_init(LOMESSAGE logfun, SHORTMESSAGE smessage)
{
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
		return 0;
	else
	{
		//대입연산
		g_ShortMessage = smessage;
		g_LogMessage = logfun;
		return 1;
	}
}

SOCKET wbsock_createsocket(int port)
{

	// socket()
	SOCKET sock = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (sock == INVALID_SOCKET)
	{
		err_quit("socket()");
		return 0;
	}

	// bind()
	SOCKADDR_IN			serveraddr;
	ZeroMemory(&serveraddr, sizeof(serveraddr));
	serveraddr.sin_family = AF_INET;
	serveraddr.sin_port = htons(port);
	serveraddr.sin_addr.s_addr = htonl(INADDR_ANY);
	int retval = bind(sock, (SOCKADDR*)&serveraddr,
		sizeof(serveraddr));

	if (retval == SOCKET_ERROR)
	{
		err_quit("bind");
		return 0;
	}

	return sock;
}


void wbsock_run(SOCKET sock)

{	// 데이터 통신에 사용할 변수
	SOCKADDR_IN		clientaddr;
	int				addrlen;
	char			buf[BUFSIZE + 1];


	int retval;


	// 클라이언트와 데이터 통신
	while (1)
	{
		// 데이터 받기
		addrlen = sizeof(clientaddr);
		retval = recvfrom(sock, buf, BUFSIZE, 0,
			(SOCKADDR*)&clientaddr, &addrlen);

		//TCP는 SOCKET_ERROR, 0(상대방이 종료):이건 UDP가없다(상대방이없어서), 정상데이터
		if (retval == SOCKET_ERROR)
		{
			err_display("recvfrom");
			continue;
		}

		// 받은 데이터 출력
		buf[retval] = '\0';
		printf("[UDP/%s:%d] %s\n", inet_ntoa(clientaddr.sin_addr),
			ntohs(clientaddr.sin_port), buf);

		// 데이터 보내기
		retval = sendto(sock, buf, retval, 0,
			(SOCKADDR*)&clientaddr, sizeof(clientaddr));
		if (retval == SOCKET_ERROR)
		{
			err_display("sendto");
			continue;
		}
	}


}

void wbsock_closesocket(SOCKET sock)
{

	// closesocket()
	closesocket(sock);


}
void wbsock_exit()
{

	// 윈속 종료
	WSACleanup();
}

//wbsock내부용 함수
void err_quit(const char* msg)
{
	LPVOID lpMsgBuf;
	FormatMessage(
		FORMAT_MESSAGE_ALLOCATE_BUFFER |
		FORMAT_MESSAGE_FROM_SYSTEM,
		NULL, WSAGetLastError(),
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPTSTR)&lpMsgBuf, 0, NULL);
	MessageBox(NULL, (LPCTSTR)lpMsgBuf, msg, MB_ICONERROR);
	LocalFree(lpMsgBuf);
	exit(-1);
}




// 소켓 함수 오류 출력
void err_display(const char* msg)
{
	LPVOID lpMsgBuf;
	FormatMessage(
		FORMAT_MESSAGE_ALLOCATE_BUFFER |
		FORMAT_MESSAGE_FROM_SYSTEM,
		NULL, WSAGetLastError(),
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPTSTR)&lpMsgBuf, 0, NULL);
	//printf("[%s] %s", msg, (LPCTSTR)lpMsgBuf);

	g_LogMessage(msg, (LPTSTR)lpMsgBuf);


	LocalFree(lpMsgBuf);
}

