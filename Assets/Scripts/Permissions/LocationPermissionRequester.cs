using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

public class LocationPermissionRequester : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI permissionStatusText; // Texto de la UI para mostrar el estado del permiso.
    [SerializeField] private UnityEvent onPermissionGranted; // Evento cuando el permiso es concedido.
    [SerializeField] private UnityEvent onPermissionDenied; // Evento cuando el permiso es denegado la primera vez.
    [SerializeField] private UnityEvent onPermissionDoubleDenied; // Evento cuando el permiso es denegado dos veces..
    private const string PermissionRequestCountKey = "PermissionRequestCount"; // String que sirve como key de PlayerPrefs para almacenar la cantidad de rechazos.
    
    //En futuras versiones de Unity, AndroiJNI tiene la posibilidad de usar un callback cuando se solicita un permiso, para no tener que comprobar cuándo se responde al mismo.
    private Coroutine permissionChecker; // Rutina para comprobar la respuesta del usuario en tiempo real.
    private void Start()
    {
        //Al empezar la aplicación, gestionamos los permisos
        RequestLocationPermission();
    }

    public void RequestLocationPermission()
    {
        // Comprobar si el permiso ha sido concedido
        bool permissionGranted = PermissionsHandler.CheckPermission(Permission.FineLocation);
        
        // Usar playerprefs para solicitar el permiso o indicar que la única forma de aceptarlo (después de dos rechazos) es a través de la configuración de la app.
        int permissionRequestCount = PlayerPrefs.GetInt(PermissionRequestCountKey, 0);

        if (!permissionGranted)
        {
            if (permissionRequestCount == 0)
            {
                // Incrementamos el contador de solicitudes
                permissionRequestCount++;
                PlayerPrefs.SetInt(PermissionRequestCountKey, permissionRequestCount);
                PlayerPrefs.Save();

                onPermissionDenied.Invoke();
                permissionChecker = StartCoroutine(WaitForPermissionResponse());
                PermissionsHandler.RequestPermission(Permission.FineLocation);
            }
            else
            {
                onPermissionDoubleDenied.Invoke();
                if(permissionChecker == null)
                    permissionChecker = StartCoroutine(WaitForPermissionResponse());
                PermissionsHandler.RequestPermission(Permission.FineLocation);
            }
            
        }
        else
        {
            onPermissionGranted.Invoke();
        }
        // Actualizamos el texto de la UI que indica el estado de los permisos
        UpdatePermissionStatus(permissionGranted);
    }

    private void UpdatePermissionStatus(bool permissionGranted)
    {
        permissionStatusText.text = permissionGranted ? "Granted" : "Denied";
        permissionStatusText.color = permissionGranted ? new Color(0.05f, 0.7f, 0.05f) : Color.red;
    }
    
    //Corutina que comprueba periódicamente cuándo el usuario ha tomado una decisión sobre el permiso
    private IEnumerator WaitForPermissionResponse()
    {
        while (!PermissionsHandler.CheckPermission(Permission.FineLocation))
            yield return new WaitForSeconds(1f); //La frecuencia de comprobación puede ser ajustada según las necesidades.

        UpdatePermissionStatus(true);
        onPermissionGranted.Invoke();
    }
    
    public void OpenAppSettings()
    {
        //Código extraído de https://discussions.unity.com/t/redirect-to-app-settings/658339 para abrir la configuración de la app en Android.
        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            string packageName = currentActivityObject.Call<string>("getPackageName");

            using (var uriClass = new AndroidJavaClass("android.net.Uri"))
            using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null))
            using (var intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject))
            {
                intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
                intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
                currentActivityObject.Call("startActivity", intentObject);
            }
        }
    }
}
