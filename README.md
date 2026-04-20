# 🤟 Talkable — AI-Powered Sign Language Communication Platform

Talkable is a real-time communication platform designed to bridge the gap between sign language users and spoken/written language users using AI-powered translation.

It enables live video calls where sign language gestures are converted into text (and optionally speech) using a machine learning model, making communication more accessible and inclusive.

> 🚧 This project is currently under active development as part of a graduation project.

---

## 🚀 Key Features

- 🎥 Real-time video communication using WebRTC signaling
- ⚡ SignalR-based live connection management (rooms, peers, ICE candidates)
- 🤖 AI-powered sign language recognition (gesture → text)
- 🔐 JWT authentication with role-based authorization
- 🧩 Clean layered backend architecture (Controllers / Services / Repositories)
- 🗄️ Entity Framework Core with relational database design
- 📡 RESTful API + real-time communication hybrid system

---

## 🧠 AI Integration

The system sends extracted gesture features to a Python-based ML API:

- Input: Hand/body feature vectors
- Output: Predicted text + confidence score
- Used in real-time during communication sessions

---

## 🏗️ Backend Architecture
Controllers → Services → Repositories → Database
### Core Modules:
- Auth System (JWT)
- Avatar / Animation System
- Real-time Call Hub (SignalR)
- AI Feature Processing Service

---

## 📡 Real-Time Communication Flow

1. User creates or joins a room
2. SignalR establishes connection
3. WebRTC offer/answer/ICE exchange happens via CallHub
4. Video stream starts between peers
5. AI service processes gesture features in real-time

---

## 🛠️ Tech Stack

- ASP.NET Core (.NET 9)
- SignalR
- Entity Framework Core
- JWT Authentication
- Python (AI inference API)
- WebRTC (peer-to-peer video communication)
- SQL Server (or configured DB)

---

## 📌 Project Status

🚧 Currently under active development  
Planned improvements:
- Enhanced AI accuracy and model optimization
- UI/UX improvements in Flutter frontend
- Multi-user group call support
- Performance optimization for real-time inference

---



## 🎯 Goal

To create an accessible communication bridge between sign language users and non-signers using real-time AI and low-latency communication systems.
