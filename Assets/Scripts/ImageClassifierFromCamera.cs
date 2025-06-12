using UnityEngine;
using UnityEngine.UI;
using Unity.InferenceEngine;
using TMPro;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private RawImage webcamImage; // Image to display the camera feed
    [SerializeField] private TextMeshProUGUI resultText; // Text to display the classification result
    [SerializeField] ModelAsset modelAssets; // Model assets containing the ONNX model and labels
    [SerializeField] private int targetWidth = 224; // Target width for the input image
    [SerializeField] private int targetHeight = 224; // Target height for the input image
    [SerializeField] private int inputChannels = 3; // Number of input channels (e.g., RGB)
    private WebCamTexture webcamTexture; // Texture de la webcam
    private Model runtimeModel; // Modèle d'exécution
    private Worker worker; // Worker pour l'inférence
    Tensor<float> inputTensor; // Tenseur d'entrée pour le modèle
    Texture2D inputTexture; // Texture d'entrée pour le modèle
    Texture2D resizedTexture; // Texture redimensionnée pour le modèle


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialisation de la webcam
        webcamTexture = new WebCamTexture();
        webcamImage.texture = webcamTexture;
        webcamTexture.Play();

        // Initialiser les textures
        inputTexture = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGBA32, false);
        resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);

        // Initialiser le tenseur d'entrée
        inputTensor = new Tensor<float>(new TensorShape(1, targetWidth, targetHeight, inputChannels));

        // Charger le modèle
        runtimeModel = ModelLoader.Load(modelAssets);

        // Initialiser le worker pour l'inférence
        worker = new Worker(runtimeModel, BackendType.GPUCompute);
    }

    // Update is called once per frame
    void Update()
    {
        // Si la webcam a mis à jour l'image, traiter l'image
        if (webcamTexture.didUpdateThisFrame)
        {
            // Mettre à jour les pixels de la texture d'entrée
            inputTexture.SetPixels(webcamTexture.GetPixels());
            inputTexture.Apply();

            // Inverser la texture horizontalement (effet miroir)
            inputTexture = TextureUtils.FlipTextureHorizontally(inputTexture);

            // Recadrer et redimensionner la texture
            resizedTexture = TextureUtils.CropAndResizeTexture(inputTexture, targetWidth, targetHeight);

            // Mettre à jour les pixels de la texture redimensionnée
            webcamImage.texture = resizedTexture;

            // Convertir la texture en tenseur avec transformation
            TextureConverter.ToTensor(resizedTexture, inputTensor, new TextureTransform().SetTensorLayout(TensorLayout.NHWC));

            // Initialiser le tenseur de sortie
            Tensor<float> outputTensor = null;

            try
            {
                // Effectuer une prédiction
                worker.Schedule(inputTensor);
                outputTensor = worker.PeekOutput() as Tensor<float>;

                // Interpréter les résultats
                int predictedClass = GetPredictedClass(outputTensor);

                // Gérer la prédiction (afficher le texte correspondant à la classe prédite)
                HandlePrediction(predictedClass);
            }
            finally
            {
                // Libérer les ressources si nécessaire
            }
        }
    }


    private int GetPredictedClass(Tensor<float> outputTensor)
    {
        float[] outputData = outputTensor.DownloadToArray();
        Debug.Log("Output data: " + string.Join(", ", outputData));

        int predictedClass = -1;
        float maxConfidence = 0.05f; // Seuil de confiance minimum

        for (int i = 0; i < outputData.Length; i++)
        {
            if (outputData[i] > maxConfidence)
            {
                predictedClass = i;
                maxConfidence = outputData[i];
            }
        }

        return predictedClass;
    }

    private void HandlePrediction(int predictedClass)
    {
        Vector3 movement = Vector3.zero;

        switch (predictedClass)
        {
            case 0: // Gauche
                resultText.text = "Pose détectée: Gauche";
                movement = Vector3.left;
                break;
            case 1: // Droite
                resultText.text = "Pose détectée: Droite";
                movement = Vector3.right;
                break;
            case 2: // Haut
                resultText.text = "Pose détectée: Haut";
                movement = Vector3.forward;
                break;
            case 3: // Bas
                resultText.text = "Pose détectée: Bas";
                movement = Vector3.back;
                break;
            default:
                resultText.text = "Pose inconnue";
                break;
        }

        if (movement != Vector3.zero)
        {
            PlayerController playerController = Object.FindAnyObjectByType<PlayerController>();
            if (playerController != null)
            {
                playerController.OnMoveFromPose(movement);
            }
        }
    }

    private void OnDestroy()
    {
        // Libérer les ressources
        worker.Dispose();
        inputTensor.Dispose();
    }
}
