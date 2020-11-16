from operator import itemgetter
import cv2
import numpy as np
import base64
from AutoHouse.PostRequest import PostImage2BaiduAPI
import math

RANDOM_FURNITURE = ['sofa', 'chair', 'table', 'cabinet']


# 读取图片
def ReadImg(img):
    # img = cv2.imread('img/houseTemplate1.jpg', 1)
    img = cv2.imread(img, 1)
    # cv2.imshow('src', img)
    return img


# 高斯滤波
def GausBlur(src):
    dst = cv2.GaussianBlur(src, (5, 5), 1.5)
    # dst = cv2.GaussianBlur(src, (3, 3), 0)
    # cv2.imshow('GausBlur', dst)
    return dst


# Canny滤波
def CannyBlur(src):
    canny = cv2.Canny(src, 0, 255)
    # cv2.imshow('GausBlur', canny)
    return canny


# 轮廓拟合
def GetContoursPositions(open_img):
    result = []
    contours, hierarchy = cv2.findContours(open_img, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
    for cnt in contours:
        # cnt = contours[0]
        if cnt is not None:
            rect = cv2.minAreaRect(cnt)
            box = cv2.boxPoints(rect)
            box = np.int0(box)
            temp = []
            if box[1].tolist()[0] != box[2].tolist()[0]:
                continue
            temp.append(box[2].tolist())
            temp.append(box[0].tolist())
            result.append(temp)

            # # 图像轮廓及中心点坐标绘制
            # cv2.drawContours(src, [box], 0, (0, 0, 255), 3)
            # M = cv2.moments(cnt)
            # if M['m00'] != 0:
            #     center_x = int(M['m10'] / M['m00'])
            #     center_y = int(M['m01'] / M['m00'])
            # cv2.circle(src, (center_x, center_y), 7, 128, -1)
            # str1 = '(' + str(center_x) + ',' + str(center_y) + ')'
            # cv2.putText(src, str1, (center_x - 50, center_y + 40), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 255, 0), 2,
            #             cv2.LINE_AA)
            # cv2.imshow('show', src)
    return result


# 百度ai单图片识别(或者设计师于对应单图片标记数字，利用数字识别类别)
def RecognizeIMG(img, positions):
    if positions is not None and len(positions) != 0:
        dic = {}
        count = 0
        for position in positions:
            if math.fabs(position[0][0] - position[1][0]) < 10 or math.fabs(
                            position[0][1] - position[1][1]) < 10:  # 过小一般没用
                continue
            single_img = img[position[0][0]:position[0][1], position[1][0]:position[1][1]]
            # json_loads = PostImage2BaiduAPI(Base64It(single_img))
            # if json_loads['error_code'] is None:
            # img_classfy = json_loads['result']['furnitrue']['name']
            img_classfy = RANDOM_FURNITURE[np.random.randint(0, 4)]  # 测试用，实际需根据百度接口返回处理
            dic[img_classfy] = position
            count += 1
        result = {}
        startx = 0
        endx = 0
        for index,temptuple in enumerate(sorted(dic.items(),  key=lambda x: x[1][0][0])):
            if index==0:
                startx = temptuple[1][0][0]
            if index==len(dic)-1:
                endx = temptuple[1][1][0]
            result[temptuple[0]]=temptuple[1]
        result["width"] = endx-startx
        starty = 0
        endy = 0
        for index,temptuple in enumerate(sorted(dic.items(),  key=lambda x: x[1][1][1])):
            if index==0:
                starty = temptuple[1][0][1]
            if index==len(dic)-1:
                endy = temptuple[1][1][1]
        result["height"] = endy - starty
        return result


def Base64It(img):
    with open(img, 'rb') as f:
        base64_data = base64.b64encode(f.read())
        return base64_data.decode()


def CreateImgData(img):
    src = ReadImg(img)
    gaus_img = GausBlur(src)
    canny_img = CannyBlur(gaus_img)
    return RecognizeIMG(src, GetContoursPositions(canny_img))


if __name__ == '__main__':
    img = 'img/house9.jpg'
    src = ReadImg(img)
    gaus_img = GausBlur(src)
    canny_img = CannyBlur(gaus_img)
    RecognizeIMG(src, GetContoursPositions(canny_img))
    cv2.waitKey(0)
