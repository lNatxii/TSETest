using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class CubePool: MonoBehaviour
{
    [SerializeField]private GameObject cubePrefab; // Prefab del cubo
    [SerializeField]private Transform parentTransform; // Transform del padre de los cubos
    [SerializeField] private List<GameObject> pooledCubes; // Lista de cubos en el pool
    public CubeSpawner spawner; // Evento que se dispara al destruir la pool
    private int initialPoolSize = 10; // Tamaño inicial del pool
    
    public void InitializePool(GameObject prefab, Transform parent, CubeSpawner cubeSpawner)
    {
        // Inicializar variables
        spawner = cubeSpawner;
        cubePrefab = prefab;
        parentTransform = parent;
        pooledCubes = new List<GameObject>();
        
        // Para evitar problemas con distintas instancias de CubePool
        DestroyPreviousPool();
        
        // Crear cubos iniciales
        for (int i = 0; i < initialPoolSize; i++)
            CreateNewCube();
    }
    
    private void DestroyPreviousPool()
    {
        if (parentTransform.childCount == pooledCubes.Count)
        {
            CustomLogger.Log("No hay objetos perdidos de una pool previa", LogSeverity.SUCCESS);
            return;
        }

        if (parentTransform.childCount >  pooledCubes.Count )
            CustomLogger.Log("Existen objetos perdidos de una pool previa", LogSeverity.CRITICAL);
        else
            CustomLogger.Log("Existen objetos en la pool que no están en el padre", LogSeverity.CRITICAL);

        //Si inicializamos una nueva pool (por el motivo que sea), deberemos limpiar los objetos de la anterior
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            if (Application.isEditor && !Application.isPlaying) //Si estamos en el editor, debemos llamar a DestroyImmediate
                GameObject.DestroyImmediate(parentTransform.GetChild(i).gameObject);
            else //Si estamos en cualquier otra situación, debemos llamar a Destroy únicamente
                GameObject.Destroy(parentTransform.GetChild(i).gameObject);
        }
    }

    private void OnDestroy()
    {
        spawner.UpdatePoolCounter(pooledCubes.Count);
    }

    private void CreateNewCube()
    {
        //Con esto se crean cubos, NO se spawnean
        GameObject newCube = Object.Instantiate(cubePrefab, parentTransform);
        newCube.SetActive(false);
        pooledCubes.Add(newCube);
    }
    
    public void DisableAllCubes()
    {
        //Desactivar todos los cubos
        CheckPoolIntegrity(); //Llamada para comprobar la integridad de la pool
        foreach (var cube in pooledCubes)
            cube.SetActive(false); // Desactivar el cubo
        spawner.UpdatePoolCounter(0);
    }

    public GameObject GetPooledCube()
    {
        CheckPoolIntegrity(); //Llamada para comprobar la integridad de la pool
        
        // Retornar un cubo desactivado, para poder usarlo sin instanciar uno nuebo
        foreach (var cube in pooledCubes)
            if (!cube.activeInHierarchy)
                return cube; 

        // Si no hay cubos disponibles, crear uno nuevo
        CreateNewCube();
        return pooledCubes[pooledCubes.Count - 1]; // Retornar el nuevo cubo
    }
    
    public GameObject SpawnCube(Vector3 position)
    {
        CheckPoolIntegrity();//Llamada para comprobar la integridad de la pool
        
        //Se activa un cubo de la pool o se instancia uno nuevo, y se establece su posición
        GameObject cube = GetPooledCube();
        cube.transform.position = position;
        cube.SetActive(true); // Activar el cubo
        spawner.UpdatePoolCounter(GetActiveCubesCounter());
        return cube;
    }

    public int GetActiveCubesCounter()
    {
        CheckPoolIntegrity();//Llamada para comprobar la integridad de la pool
        
        int activeCubes = 0;
        foreach (var cube in pooledCubes)
            if (cube && cube.activeInHierarchy)
                activeCubes++; // Retornar un cubo desactivado
        
        return activeCubes;
    }
    
    public void CheckPoolIntegrity()
    {
        var auxPooledCubes = new List<GameObject>();

        // Recorremos la lista de cubos
        for (var index = 0; index < pooledCubes.Count; index++)
        {
            var cube = pooledCubes[index];
            if (cube != null)
                auxPooledCubes.Add(cube); // Agregar solo los cubos no nulos a la nueva lista
            else
                CustomLogger.Log("Se han encontrado objetos nulos en la pool", LogSeverity.WARNING);
        }
        
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            var cube = parentTransform.GetChild(i);
            if (!pooledCubes.Contains(cube.gameObject))
            {
                CustomLogger.Log("Se han encontrado objetos en el padre, que no están en la pool", LogSeverity.CRITICAL);
            }
        }
        pooledCubes = auxPooledCubes;
        
        if(pooledCubes.Count != auxPooledCubes.Count)
            spawner.UpdatePoolCounter(GetActiveCubesCounter());
    }
}