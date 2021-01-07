//skeleton 코드 
#pragma comment(linker, "/subsystem:windows")

#include <Windows.h>
#include <tchar.h>

#define WM_SENDPOINT WM_USER + 100
#define WM_SENDCOLOR WM_USER + 200

LRESULT OnLButtonDown(HWND hwnd, WPARAM wParam, LPARAM lParam)
{
	POINTS pt = MAKEPOINTS(lParam);

	HWND htarget = FindWindow(0, TEXT("second"));
	if (htarget == 0)
	{
		MessageBox(hwnd, TEXT("없다"), TEXT("알림"), MB_OK);
		return 0;
	}

	SendMessage(htarget, WM_SENDPOINT, pt.x, pt.y);
	return 0;
}

LRESULT OnKeyDown(HWND hwnd, WPARAM wParam, LPARAM lParam)
{
	HWND htarget = FindWindow(0, TEXT("second"));
	if (htarget == 0)
	{
		MessageBox(hwnd, TEXT("없다"), TEXT("알림"), MB_OK);
		return 0;
	}

	if (wParam == 'R')
	{
		SendMessage(htarget, WM_SENDCOLOR, RGB(255, 0, 0), 3);
	}
	else if (wParam == 'G')
	{
		SendMessage(htarget, WM_SENDCOLOR, RGB(0, 255, 255), 5);
	}
	return 0;
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_CREATE:
		return 0;
	case WM_LBUTTONDOWN:  
		return OnLButtonDown(hwnd, wParam, lParam);
	case WM_KEYDOWN:
		return OnKeyDown(hwnd, wParam, lParam);

	case WM_DESTROY:
		PostQuitMessage(0);
		return 0;
	}
	return DefWindowProc(hwnd, msg, wParam, lParam);
}


int WINAPI _tWinMain(HINSTANCE hInst, HINSTANCE hPrev, LPTSTR lpcmd, int nShow)
{
	//1. 윈도우 클래스 생성
	WNDCLASS wc;
	wc.cbWndExtra = 0;
	wc.cbClsExtra = 0;
	wc.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
	wc.hCursor = LoadCursor(0, IDC_ARROW);
	wc.hIcon = LoadIcon(0, IDI_APPLICATION);
	wc.hInstance = hInst;
	wc.lpfnWndProc = WndProc;
	wc.lpszClassName = TEXT("Sample");
	wc.lpszMenuName = 0;
	wc.style = 0;

	//2. 생성한 윈도우 클래스를 등록
	RegisterClass(&wc);

	//3. 윈도우 생성 
	HWND hwnd = CreateWindowEx(0,	//WS_EX_TOPMOST
		TEXT("Sample"), TEXT("first"),
		WS_OVERLAPPEDWINDOW,	// & ~WS_THICKFRAME,
		CW_USEDEFAULT, 0, CW_USEDEFAULT, 0,
		0, 0,
		hInst,
		0);

	//4. 생성된 윈도우를 화면에 출력
	ShowWindow(hwnd, SW_SHOW);
	UpdateWindow(hwnd);

	//5. 메시지 루프
	MSG msg;
	while (GetMessage(&msg, 0, 0, 0))
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}
	return 0;
}