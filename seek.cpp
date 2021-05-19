#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <fstream>
#include <cstring>
#include <windows.h>
using namespace std;

typedef struct
{
    BITMAPFILEHEADER Header;
    BITMAPINFOHEADER Info;
    unsigned char* Pixels;
} BITMAPDATA;

void LoadBmp(const char* path, BITMAPDATA* Data)
{
    std::ifstream hFile(path, std::ios::in | std::ios::binary);

    if (hFile.is_open())
    {
        hFile.read((char*)&Data->Header, sizeof(Data->Header));
        hFile.read((char*)&Data->Info, sizeof(Data->Info));
        hFile.seekg(Data->Header.bfOffBits, std::ios::beg);

        Data->Pixels = new unsigned char[Data->Info.biSizeImage];
        hFile.read((char*)Data->Pixels, Data->Info.biSizeImage);
        hFile.close();
    }
}


int cheak(int i, int j, int w, int h) {
    if (i < 0 || i >= h || j < 0 || j >= w) return 0;
    return 1;
}

void Cut(BITMAPDATA* Data)
{
    FILE* fp = fopen("cat2.bmp", "wb");

    unsigned int width = std::abs(Data->Info.biWidth);
    unsigned int height = std::abs(Data->Info.biHeight);

    cout << width;

    width /= 3;

    Data->Info.biSizeImage = width * height * 3;
    Data->Header.bfSize = width * height * 3 + 54;
    Data->Info.biWidth = width;

    fwrite(&Data->Header, sizeof(BITMAPFILEHEADER), 1, fp);
    fwrite(&Data->Info, sizeof(BITMAPINFOHEADER), 1, fp);


    int** a, * b;
    int i, j;

    a = (int**)malloc(sizeof(int*) * height);
    b = (int*)malloc(sizeof(int*) * height * width);

    for (i = 0; i < height; i++)
        a[i] = b + i * width;
    for (i = 0; i < height; i++)
        for (j = 0; j < width; j++)
            a[i][j] = 1;


    int top = 0, bot = width * 3, left = 0, right = width;
    int top2 = 0, bot2 = height, left2 = 0, right2 = width * 3;

    for (i = top; i < bot; i++)
        for (j = left; j < right; j++)
            a[i][j] = 2;

    int count = 0;

    for (i = top2; i < bot2; i++) {
        for (j = left2; j < right2; j++) {
            if (cheak(i, j, width, height) == 1) {
                fwrite(&Data->Pixels[count], 3, 1, fp);
            }
            count += 3;
        }
    }
}

void Hap(BITMAPDATA* Data , BITMAPDATA* Data1, BITMAPDATA* Data2, BITMAPDATA* Data3 , BITMAPDATA* Data4)
{
    FILE* fp = fopen("11.bmp", "wb");

    unsigned int width =Data1->Info.biWidth + Data2->Info.biWidth + Data3->Info.biWidth + Data4->Info.biWidth;
    unsigned int height = Data1->Info.biHeight+ Data2->Info.biHeight + Data3->Info.biHeight+ Data4->Info.biHeight;

    cout << width <<endl;
    cout << height;

    Data->Header.bfSize = Data1->Header.bfSize + Data2->Header.bfSize + Data3->Header.bfSize + Data4->Header.bfSize;
    Data->Info.biHeight = height;
    Data->Info.biWidth = width;


 
    fwrite(&Data->Header, sizeof(BITMAPFILEHEADER), 1, fp);
    fwrite(&Data->Info, sizeof(BITMAPINFOHEADER), 1, fp);


 
    
}

int main()
{
    BITMAPDATA Data ,Data1, Data2, Data3, Data4;
    LoadBmp("cat.bmp", &Data1);
    LoadBmp("cat.bmp", &Data2);
    LoadBmp("cat.bmp", &Data3);
    LoadBmp("cat.bmp", &Data4);



    Hap(&Data, &Data1, &Data2, &Data3, &Data4);
    delete[] Data.Pixels;
    delete[] Data1.Pixels;
    delete[] Data2.Pixels;
    delete[] Data3.Pixels;
    delete[] Data4.Pixels;


    return 0;
}