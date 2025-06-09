import socket
import cv2
import numpy as np

def send_image(image):
    # JPEGエンコード
    _, encoded_img = cv2.imencode('.jpg', image)
    data = encoded_img.tobytes()

    # ソケット送信
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect(('localhost', 5001))  # WPF側の受信サーバーに接続
        s.sendall(len(data).to_bytes(4, 'big'))  # 先頭にサイズを送る
        s.sendall(data)
