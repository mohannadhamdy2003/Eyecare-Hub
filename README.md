# 🧿 EyeCare Hub

A smart AI‑powered eye‑disease detection and patient management system built using **.NET Web API**, **React**, **FastAPI**, and **Deep Learning models**.

### 🌐 **Live Demo**

**https://eyecare-hub-front-end.vercel.app/**

---

## 🚀 Overview

EyeCare Hub helps users upload fundus (eye) images to detect common eye diseases using AI models. Doctors and patients can easily manage reports, history, and appointments through a clean and responsive web interface.

---

## 🧩 Key Features

* 🔐 **Secure Authentication** (JWT)
* 🩺 **AI Disease Detection** (Diabetic Retinopathy, Glaucoma, Cataract)
* 📤 **Upload Fundus Images** for instant predictions
* 📊 **Detailed Medical Reports** saved in database
* 👨‍⚕️ **Doctor & Patient Dashboards**
* 🤝 **Frontend + .NET API + FastAPI AI Integration**
* 🗄️ **SQL Server Database**

---

## 🏗️ Tech Stack

### **Frontend**

* **React.js** – For building the user interface
* **JavaScript (ES6+)** – Main programming language
* **React Router** – Client-side navigation
* **Axios** – API communication
* **CSS / TailwindCSS** 
* **Vite ** – Frontend build & development tool
* **Node.js + npm** – Dependency management


### **Backend (.NET)**

* .NET Web API
* Entity Framework Core
* SQL Server

### **AI Service (FastAPI)**

* Python FastAPI
* TensorFlow / PyTorch (CNN models)
* Pillow, NumPy

---

## 🗂️ Project Structure

```
EyeCare-Hub/
│
├── backend-dotnet/      # .NET Web API
├── frontend-react/      # React frontend
├── ai-fastapi/          # Python FastAPI + AI models
└── docs/                # Architecture, diagrams, API docs
```

---

## 📡 API Flow

1. User uploads image → React
2. React sends to .NET API
3. .NET → forwards to FastAPI
4. FastAPI → runs AI model → returns prediction
5. .NET saves report
6. User views results

---

