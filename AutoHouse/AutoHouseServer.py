import json
from werkzeug.serving import run_simple
from flask import Flask, request
from AutoHouse.OutlineRecognize import CreateImgData

app = Flask(__name__)


# 测试用
@app.route('/greeter', methods=['GET'])
def hello_world():
    return 'Hello World !'


# 获取自构建房屋设计json
@app.route('/getautohouse', methods=['POST'])
def get_autohouse():
    IMG = 'img/house9.jpg'
    return json.dumps(CreateImgData(IMG)).encode('utf-8')


if __name__ == '__main__':
    run_simple('localhost', 21567, app)
