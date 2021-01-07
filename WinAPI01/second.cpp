//skeleton 코드 
#pragma comment(linker, "/subsystem:windows")

#include <Windows.h>
#include <tchar.h>

#define WM_SENDPOINT WM_USER + 100
#define WM_SENDCOLOR WM_USER + 200

COLORREF g_color = RGB(0, 255, 0);
int g_width = 1;

LRESULT OnSendPoint(HWND hwnd, WPARAM wParam, LPARAM lParam)
{
	HDC hdc = GetDC(hwnd);

	HPEN pen = CreatePen(PS_SOLID, g_width, g_color);
	HPEN oldpen = (HPEN)SelectObject(hdc, pen);

	Rectangle(hdc, wParam, lParam, wParam + 50, lParam + 50);

	DeleteObject(SelectObject(hdc, oldpen));
	ReleaseDC(hwnd, hdc);
	return 0;
}

LRESULT OnSendColor(HWND hwnd, WPARAM wParam, LPARAM lParam)
{
	g_color = wParam;
	g_width = lParam;

	return 0;
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_SENDPOINT:
		return OnSendPoint(hwnd, wParam, lParam);
	case WM_SENDCOLOR:
		return OnSendColor(hwnd, wParam, lParam);
	case WM_CREATE:
		return 0;

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
		TEXT("Sample"), TEXT("second"),
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