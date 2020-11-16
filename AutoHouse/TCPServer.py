from socket import *
from AutoHouse.OutlineRecognize import CreateImgData
import json

# tcp服务端
def server():
    # jsonData = {
    #     'ssid': '123',
    #     'passwd': '456'
    # }
    IMG = 'img/house9.jpg'
    HOST = '127.0.0.1'
    PORT = 21567
    BUFSIZ = 1024
    ADDR = (HOST, PORT)

    tcpSerSock = socket(AF_INET, SOCK_STREAM)
    tcpSerSock.bind(ADDR)
    tcpSerSock.listen(5)

    while True:
        print('*'*5,'等待连接','*'*5)
        tcpCliSock, addr = tcpSerSock.accept()
        print('*'*5,'连接地址:', addr,'*'*5)
        while True:
            data = tcpCliSock.recv(BUFSIZ)
            if not data:
                break
            tcpCliSock.send(json.dumps(CreateImgData(IMG)).encode('utf-8'))
        tcpCliSock.close()


if __name__ == '__main__':
    server()
