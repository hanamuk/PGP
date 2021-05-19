//#define _CRT_SECURE_NO_WARNINGS
//#include <stdio.h>
//#include <stdlib.h>
//
//typedef unsigned long DWORD;
//typedef unsigned short WORD;
//typedef unsigned long LONG;
//typedef unsigned char BYTE;
//
//int lc = 0;
//
//#pragma pack(push)  
//#pragma pack(1)     
//struct BITMAPFILEHEADER
//{
//	WORD	bfType;
//	DWORD	bfSize;
//	WORD	bfReserved1;
//	WORD	bfReserved2;
//	DWORD	bfOffBits;
//};
//#pragma pack(pop)   
//
//#pragma pack(push)  
//#pragma pack(1)     
//struct BITMAPINFOHEADER
//{
//	DWORD	biSize;
//	LONG	biWidth;
//	LONG	biHeight;
//	WORD	biPlanes;
//	WORD	biBitCount;
//	DWORD	biCompression;
//	DWORD	biSizeImage;  
//	LONG	biXPelsPerMeter;
//	LONG	biYPelsPerMeter;
//	DWORD	biClrUsed;  
//	DWORD	biClrImportant;
//
//};
//#pragma pack(pop)   
//
//#pragma pack(push) 
//struct RGBQUAD {
//	BYTE    rgbBlue;
//	BYTE    rgbGreen;
//	BYTE    rgbRed;
//	BYTE    rgbReserved;
//};
//#pragma pack(pop)   
//
//
//
//int cheak(int i, int j, int w, int h) {
//	if (i < 0 || i >= h || j < 0 || j >= w) return 0;
//	return 1;
//}
//
//int cut(unsigned char* bitmapData, BITMAPFILEHEADER* bitmapFileHeader, BITMAPINFOHEADER* bitmapInfoHeader, RGBQUAD* Palette) 
//{
//	FILE* fp = fopen("cat2.bmp", "wb");
//
//	int m, n;
//	LONG w = bitmapInfoHeader->biWidth;
//	LONG h = bitmapInfoHeader->biHeight;
//
//	n = w / 3;
//	m = h;
//
//	int** a, * b;
//	int i, j;
//
//	a = (int**)malloc(sizeof(int*) * h);
//	b = (int*)malloc(sizeof(int*) * h * w);
//
//	for (i = 0; i < h; i++)
//		a[i] = b + i * w;
//	for (i = 0; i < h; i++) 
//		for (j = 0; j < w; j++)
//			a[i][j] = 1;
//
//	int temp;
//	int top = 0, bot = 300, left = 0, right = 100;
//	int top2 = 0, bot2 = 300, left2 = 0, right2 = 300;
//
//
//
//
//	for (i = top; i < bot; i++)
//		for (j = left; j < right; j++)
//			a[i][j] = 2;
//
//
//	//chinh sua thong tin trong header, xuat header:
//	bitmapInfoHeader->biWidth = n;
//	bitmapInfoHeader->biHeight = m;
//	int index = -1; //khai bao o trong if thi o ben ngoai if se ko dung dc 
//	if (bitmapInfoHeader->biBitCount >= 16) { // neu la anh >=16 bit
//		if (bitmapInfoHeader->biSizeImage != 0) {
//			if ((n * 3) % 4 == 0)
//			{
//				bitmapInfoHeader->biSizeImage = n * m * 3;   //90000
//			}
//			else { bitmapInfoHeader->biSizeImage = (n * 3 + (4 - ((n * 3) % 4))) * m; }
//		}
//		if ((n * 3) % 4 == 0)
//		{
//			bitmapFileHeader->bfSize = n * m * 3 + 54;  //90054
//		}
//		else { bitmapFileHeader->bfSize = (n * 3 + (4 - ((n * 3) % 4))) * m + 54; }
//		fwrite(bitmapFileHeader, sizeof(BITMAPFILEHEADER), 1, fp);
//		fwrite(bitmapInfoHeader, sizeof(BITMAPINFOHEADER), 1, fp);
//	}
//
//	int count = 0;
//	int padd;
//	int r = 0;
//	int lc;
//
//	if (bitmapInfoHeader->biBitCount >= 16) {
//		for (i = top2; i < bot2; i++) {
//			lc = 0;
//			for (j = left2; j < right2; j++) {
//				if (cheak(i, j, w, h) == 1) {  
//					if (a[i][j] == 2) {
//						fwrite(&bitmapData[count], 3, 1, fp);
//						lc = 1;
//					}
//					count += 3;
//				}
//				else {
//					if ((left <= j && j < right) || (top <= i && i < bot) ||
//						(left2 < 0 && right2 >= w && top2 < 0 && bot2 >= h)) {
//						fwrite(&r, 3, 1, fp);
//						lc = 1;
//					}
//				}
//			}
//			if (0 <= i && i < h) {
//				if ((w * 3) % 4 == 0) { padd = 0; }
//				else {
//					padd = 4 - ((w * 3) % 4);
//					count = count + padd;
//				}
//			}
//			if (lc == 1) {
//				if ((n * 3) % 4 == 0) { padd = 0; }
//				else {
//					padd = 4 - ((n * 3) % 4);
//					fwrite(&r, 1, padd, fp);
//				}
//			}
//		}
//	}
//	free(b);
//	free(a);
//	fclose(fp);
//	return 0;
//}
//
//unsigned char* LoadBitmapFile(const char* filename, BITMAPFILEHEADER* bitmapFileHeader, BITMAPINFOHEADER* bitmapInfoHeader, RGBQUAD*& Palette) {
//	FILE* filePtr;
//	filePtr = fopen(filename, "rb");
//	if (filePtr == NULL)
//		return NULL;
//
//	fread(bitmapFileHeader, sizeof(BITMAPFILEHEADER), 1, filePtr);
//	if (bitmapFileHeader->bfType != 0x4D42) // hoac la 19778
//	{
//		fclose(filePtr);
//		return NULL;
//	}
//
//	fread(bitmapInfoHeader, sizeof(BITMAPINFOHEADER), 1, filePtr);
//	if (bitmapInfoHeader->biCompression != 0) {
//		printf("Ma nguon nay khong ho tro cho file anh bi nen\n");
//		lc = 1;
//		return NULL;
//	}
//
//	LONG w = bitmapInfoHeader->biWidth;
//	LONG h = bitmapInfoHeader->biHeight;
//	DWORD Size_Padd;
//	unsigned char* bitmapImage;
//	//16 24 32 bit: 
//	if (bitmapInfoHeader->biBitCount >= 16) {
//		if ((w * 3) % 4 == 0) { Size_Padd = w * h * 3; }
//		else { Size_Padd = (w * 3 + (4 - ((w * 3) % 4))) * h; }
//		//Size_Padd: kich thuoc co tinh ca padding 
//	}
//	else { // 1 4 8 bit:
//		if (w % 4 == 0) { Size_Padd = w * h; }
//		else { Size_Padd = (w + (4 - (w % 4))) * h; }
//		DWORD mau;
//		if (bitmapInfoHeader->biClrUsed == 0) { mau = 256; }
//		else { mau = bitmapInfoHeader->biClrUsed; }
//		Palette = (RGBQUAD*)malloc(sizeof(RGBQUAD) * mau + 1); //tao du ra 1 chut lat co viec can xai
//		fread(Palette, sizeof(RGBQUAD), mau, filePtr);
//	}
//	fseek(filePtr, bitmapFileHeader->bfOffBits, SEEK_SET);
//	bitmapImage = (unsigned char*)malloc(Size_Padd);
//	if (!bitmapImage) //neu ko du bo nho de cung cap (==NULL)
//	{					//((not) bitmapImage)
//		free(bitmapImage);
//		printf("Khong du bo nho de load anh nay");
//		lc = 1;
//		fclose(filePtr);
//		return NULL;
//	}
//	fread(bitmapImage, Size_Padd, 1, filePtr);
//	if (bitmapImage == NULL)
//	{
//		fclose(filePtr);
//		return NULL;
//	}
//	fclose(filePtr);
//	return bitmapImage;
//}
//
//int main() {
//	unsigned char* bitmapData;
//	BITMAPFILEHEADER bitmapFileHeader;
//	BITMAPINFOHEADER bitmapInfoHeader;
//	RGBQUAD* Palette;
//
//	bitmapData = LoadBitmapFile("cat.bmp", &bitmapFileHeader, &bitmapInfoHeader, Palette);
//	cut(bitmapData, &bitmapFileHeader, &bitmapInfoHeader, Palette);
//
//	return 0;
//}