using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LightingEffect : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f; // Velocidad del efecto, configurable en el Inspector
    private TextMeshProUGUI textMesh;
    private Material textMaterial;
    private float time;

    private const string BevelWidthProperty = "_BevelWidth"; // Nombre exacto de la propiedad en el shader

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textMaterial = textMesh.fontMaterial; // Obtener el material del TextMeshPro
        }
    }

    void Update()
    {
        // Aplicar efecto oscilante al Bevel Width
        if (textMaterial != null)
        {
            time += Time.deltaTime * speed;
            float width = Mathf.Lerp(-0.5f, 0.5f, (Mathf.Sin(time) + 1) / 2); // Movimiento oscilante
            textMaterial.SetFloat(BevelWidthProperty, width);
        }

        // Detectar ENTER para cambiar de escena
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No hay más escenas en la lista.");
        }
    }
}
