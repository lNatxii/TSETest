using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab; //Prefab del cubo
    [SerializeField] private Transform spawnedObjectsParent; //Objeto padre de los cubos spawneados
    [SerializeField] public Vector3 spawnAreaSize = new Vector3(5f, 5f, 5f); //Tamaño del area de spawneo
    [SerializeField] public Color gizmoColor = new Color(0f, 1f, 0f, 0.3f); //Color del gizmo del area de spawneo
    
    [SerializeField] private int cubeSpawnMultiplyer = 0; //Cantidad de cubos spawneados en ese momento
    [SerializeField] private Slider inGameMultiplierSlider; //Slider para modificar la cantidad de cubos a spawnear ingame
    [SerializeField] private TextMeshProUGUI inGameCubeCounter; //Texto para mostrar la cantidad de cubos spawneados ingame
    private CubePool cubePool; //Pool de cubos para  optimización
    private int activeCubesCounter = 0; //Tamaño del pool de cubos
    
    private void OnEnable()
    {
        if(cubePool == null)
            FindPool();
    }
    private void Start()
    {
        cubePool.DisableAllCubes();
    }

    public void UpdatePoolCounter(int poolSize)
    {
        activeCubesCounter = poolSize;
        UpdateUI(poolSize);
    }

    public int GetActiveCubesCounter()
    {
        return activeCubesCounter;
    }
    private void FindPool()
    {
        cubePool = spawnedObjectsParent.gameObject.GetComponent<CubePool>();
        if (cubePool == null)
        {
            cubePool = spawnedObjectsParent.gameObject.AddComponent<CubePool>();
            cubePool.InitializePool(cubePrefab, spawnedObjectsParent, this);
        }
    }
    
    public void SpawnCube()
    {
        // Verificar si estamos en el editor y no en modo de juego
        if (Application.isPlaying && inGameMultiplierSlider != null)
        {
            CustomLogger.Log("Using in-game multiplier slider, ignoring editor value", LogSeverity.INFO);
            cubeSpawnMultiplyer = (int)inGameMultiplierSlider.value;
        }
        
        if(cubePool == null)
            FindPool();
        
        //Se spawnean tantos cubos como los elegidos por el usuario
        for (int i = 0; i < cubeSpawnMultiplyer; i++)
        {
            Vector3 randomPosition = transform.position + new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );
            GameObject cube = cubePool.SpawnCube(randomPosition);
            
            //Randomizamos el material del cubo
            RandomizeCubeMaterial(cube);
        }
        
    }
    
    public void UpdateUI(int poolSize)
    {
        if(inGameCubeCounter != null)
            inGameCubeCounter.text = "Spawned cubes: " + poolSize.ToString();
    }
    
    public void DestroySpawnedCubes()
    {
        if (cubePool == null)
        {
            CustomLogger.Log("Cube pool is null, no cubes to destroy.", LogSeverity.ERROR);
            return;
        }
        cubePool.DisableAllCubes();
    }


    private void OnDrawGizmos()
    {
        //Gizmos del area de spawneo
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, spawnAreaSize);
        Gizmos.color = Color.green; 
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }

    public void RandomizeCubeMaterial(GameObject cube)
    {
        //Búsqueda de MaterialRandomizer en el prefab para llamar a su función de randomizado
        MaterialRandomizer randomizer = cube.GetComponent<MaterialRandomizer>();
        if (randomizer != null)
        {
            randomizer.RandomizeMaterial();
            //CustomLogger.Log("Material del cubo randomizado", LogSeverity.SUCCESS);
        }
            
        else
        {
            CustomLogger.Log("No se ha podido encontrar el componente MaterialRandomizer en el prefab del cubo", LogSeverity.ERROR);
        }
    }
}


