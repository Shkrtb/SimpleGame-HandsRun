# SimpleGame-HandsRun

A simple interactive game application using **hand tracking** powered by MediaPipe and Unity.

## 🎮 Demo
https://www.youtube.com/watch?v=xAmO9aADVv8

---

## 📝 Overview
本プロジェクトは、カメラから取得した手の動きを入力としてゲームを操作する、**非接触型インタラクションゲーム**です。

MediaPipeを用いて手のランドマーク（関節位置）をリアルタイムで検出し、その情報をUnityに連携することでゲーム操作を実現しています。

---

## 🚀 Features
- カメラ入力によるリアルタイム手検出
- 指や手の位置に基づく直感的な操作
- キーボード・マウス不要のゲーム体験
- Unityによる軽量なゲーム描画

---

## 🛠️ Tech Stack
- Unity (C#)
- MediaPipe (Hand Tracking)
- Webカメラ
- Python
- UDP通信

---

## ⚙️ System Architecture
[Camera Input]
→
[MediaPipe (Hand Landmark Detection)]
→
[Data Transfer (UDP Socket)]
→
[Unity (Game Logic & Rendering)]
