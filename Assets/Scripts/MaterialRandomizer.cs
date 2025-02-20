using UnityEngine;

public class MaterialRandomizer : MonoBehaviour
{
    public Material targetMaterial; // Material a randomizar
    public bool randomizeColor; //Booleano para elegir si se randomiza el albedo
    public bool randomizeMetallic; //Booleano para elegir si se randomiza el factor de metalizado
    public bool randomizeSmoothness; //Booleano para elegir si se randomiza el factor de rugosidad

    public void RandomizeMaterial()
    {
        // Verificar si estamos en el editor y no en modo de juego
        if (Application.isEditor && !Application.isPlaying)
        {
            // Crear un nuevo material en modo de edición
            targetMaterial = new Material(Shader.Find("Standard"));
            GetComponent<Renderer>().material = targetMaterial;
            CustomLogger.Log("Creando un nuevo material porque no estoy en playmode", LogSeverity.WARNING);
        }
        else
        {
            // Obtener el material del Renderer en modo de juego
            targetMaterial = GetComponent<Renderer>().material;
        }
        // Randomizar color
        if (randomizeColor)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value, 1f); // Albedo (color)
            //CustomLogger.Log("Random color generated: " + randomColor.ToString(), LogSeverity.INFO);
            targetMaterial.color = randomColor;
        }

        // Randomizar metallic
        if (randomizeMetallic)
        {
            float randomMetallic = Random.Range(0f, 1f); // Valor entre 0 y 1
            //CustomLogger.Log("Random metallic factor generated: " + randomMetallic, LogSeverity.INFO);
            targetMaterial.SetFloat("_Metallic", randomMetallic);
        }

        // Randomizar smoothness
        if (randomizeSmoothness)
        {
            float randomSmoothness = Random.Range(0f, 1f); // Valor entre 0 y 1
            //CustomLogger.Log("Random smoothness factor generated: " + randomSmoothness, LogSeverity.INFO);
            targetMaterial.SetFloat("_Smoothness", randomSmoothness);
        }
    }
}
