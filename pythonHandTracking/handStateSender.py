import cv2
import mediapipe as mp
import socket

# UDP設定
UDP_IP = "127.0.0.0" # your IP adress
UDP_PORT = 12345
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# MediaPipe設定
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(max_num_hands=2, min_detection_confidence=0.7)
mp_drawing = mp.solutions.drawing_utils

# カメラ入力
cap = cv2.VideoCapture(0)
cv2.namedWindow('Hand Game', cv2.WINDOW_NORMAL)

def get_hand_gesture(landmarks):
    # 指の開き具合でグー(1)/パー(2)/チョキ(3)を判定
    fingers = [mp_hands.HandLandmark.INDEX_FINGER_TIP,
               mp_hands.HandLandmark.MIDDLE_FINGER_TIP,
               mp_hands.HandLandmark.RING_FINGER_TIP,
               mp_hands.HandLandmark.PINKY_TIP]
    pips = [mp_hands.HandLandmark.INDEX_FINGER_PIP,
            mp_hands.HandLandmark.MIDDLE_FINGER_PIP,
            mp_hands.HandLandmark.RING_FINGER_PIP,
            mp_hands.HandLandmark.PINKY_PIP]

    # 指が伸びているかどうかを判定
    extended = [landmarks[tip].y < landmarks[pip].y for tip, pip in zip(fingers, pips)]

    if extended == [False, False, False, False]:
        return 1  # グー
    elif extended.count(True) >= 3:
        return 2  # パー
    elif extended == [True, True, False, False]:
        return 3  # チョキ
    else:
        return 1  # デフォルトでグー

while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        break

    frame = cv2.flip(frame, 1)
    rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    results = hands.process(rgb)
    frame = cv2.cvtColor(rgb, cv2.COLOR_RGB2BGR)

    hand_state = {"Left": "0", "Right": "0"}  # 初期値は未検出

    if results.multi_hand_landmarks and results.multi_handedness:
        for landmark, handed in zip(results.multi_hand_landmarks, results.multi_handedness):
            label = handed.classification[0].label  # 'Left' or 'Right'
            state = str(get_hand_gesture(landmark.landmark))
            hand_state[label] = state

            mp_drawing.draw_landmarks(frame, landmark, mp_hands.HAND_CONNECTIONS)

            cx = int(landmark.landmark[mp_hands.HandLandmark.MIDDLE_FINGER_MCP].x * frame.shape[1])
            cy = int(landmark.landmark[mp_hands.HandLandmark.MIDDLE_FINGER_MCP].y * frame.shape[0])

            # ラベル表示
            state_text = {
                "0": "None", "1": "Goo", "2": "Paa", "3": "Choki"
            }[state]
            cv2.putText(frame, f"{label}: {state_text}", (cx - 50, cy - 20),
                        cv2.FONT_HERSHEY_SIMPLEX, 0.9, (0, 255, 0), 2)

    # メッセージ構築 & UDP送信
    message = hand_state["Left"] + hand_state["Right"]
    sock.sendto(message.encode("utf-8"), (UDP_IP, UDP_PORT))

    # 表示
    cv2.putText(frame, f"Send: {message}", (10, 30),
                cv2.FONT_HERSHEY_SIMPLEX, 1.0, (255, 0, 0), 2)
    cv2.imshow("Hand Game", frame)

    if cv2.waitKey(1) & 0xFF == ord("q"):
        break

cap.release()
cv2.destroyAllWindows()
