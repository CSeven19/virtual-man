# coding=UTF-8
from socket import *
import json

# 客服端
def client():
    HOST = '127.0.0.1'
    PORT = 21567
    BUFSIZE = 1024
    ADDR = (HOST, PORT)

    while True:
        tcpCliSock = socket(AF_INET, SOCK_STREAM)
        tcpCliSock.connect(ADDR)
        data = input('>')
        if not data:
            break
        tcpCliSock.send(('%s\r\n' % data).encode('utf-8'))
        data = json.loads(tcpCliSock.recv(BUFSIZE).decode('utf-8'))
        if not data:
            break
        print(data)
        tcpCliSock.close()


if __name__ == '__main__':
    client()
