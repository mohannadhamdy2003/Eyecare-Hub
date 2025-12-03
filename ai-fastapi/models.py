from tensorflow import keras

def load_models():
    model1 = keras.models.load_model(r'models\model-1.h5')
    model2 = keras.models.load_model(r'models\model-2.h5')
    model3 = keras.models.load_model(r'models\model-3.h5')
    return model1, model2, model3