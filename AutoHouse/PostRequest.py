import urllib.request
import json

def PostImage2BaiduAPI(img):
    url = "https://aip.baidubce.com/rest/2.0/image-classify/v2/dish"
    data = {
        'access_token':'24.f9ba9c5241b67688bb4adbed8bc91dec.2592000.1485570332.282335-8574074',
        'image': img,
        'scenes':'advanced_general'
    }
    headers = {'Content-Type': 'application/json;charset=utf-8'}
    request = urllib.request.Request(url=url, headers=headers, data=json.dumps(data).encode('utf-8'))
    response = urllib.request.urlopen(request).read().decode('utf-8')
    return json.loads(response)