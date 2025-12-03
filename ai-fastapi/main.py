from fastapi import FastAPI, UploadFile, File, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import JSONResponse
import shutil
import os
from models import load_models

app = FastAPI()

# Load models at startup
model1, model2, model3 = load_models()

# Allow your frontend origin
origins = [
    "http://localhost:5173",  # Vite (if you're using it)
    "http://localhost:3000",  # React default
    "http://127.0.0.1:5173",  # Alt form
]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.post("/predict/")
async def predict_image(file: UploadFile = File(...)):
    try:
        temp_file_path = f"temp_{file.filename}"
        with open(temp_file_path, "wb") as buffer:
            shutil.copyfileobj(file.file, buffer)

        from my_model import Integrated_Model
        # Pass the pre-loaded models to the function
        result, confidence = Integrated_Model(temp_file_path, model1, model2, model3)
        os.remove(temp_file_path)

        return JSONResponse(content={
            "DiagnosisResult": result,
            "ConfidenceScore": confidence
        })

    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

@app.post("/upload-image/")
async def upload_image(file: UploadFile = File(...)):
    try:
        # Define the upload directory relative to the project root
        upload_dir = "../frontend/src/assets/uploadedImages/"
        os.makedirs(upload_dir, exist_ok=True)  # Create directory if it doesn't exist
        
        # Define the full path for the file
        file_path = os.path.join(upload_dir, file.filename)
        
        # Save the file
        with open(file_path, "wb") as buffer:
            shutil.copyfileobj(file.file, buffer)
        
        # Return the URL (adjust based on how your frontend serves assets)
        file_url = f"/assets/uploadedImages/{file.filename}"
        return JSONResponse(content={"url": file_url})
    
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Failed to upload image: {str(e)}")

@app.get("/check-model/")
def check_model():
    path = "models/dataset1.h5"
    return {"exists": os.path.exists(path)}