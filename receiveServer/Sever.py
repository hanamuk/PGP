import socket
import sys
import os
s = socket.socket()
s.bind(("192.168.0.4",9999))
s.listen(10) # Acepta hasta 10 conexiones entrantes.


j=len(os.walk(r'D:\yolo\tranfered').__next__()[2])
while True:
    sc, address = s.accept()

    print(address)
    j=j+1
    f = open('./tranfered/'+ str(j)+".jpg",'wb') #open in binary
    while (True):
        # recibimos y escribimos en el fichero
        l = sc.recv(1024)
        f.write(l)

        if not l:
            break

    f.close()
    sc.close()
    print('copied the file.')

s.close()
