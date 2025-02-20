using UnityEngine;

public enum LogSeverity
{
    DEBUG = 1,
    INFO = 2,
    WARNING = 3,
    ERROR = 4,
    CRITICAL = 5,
    SUCCESS = 6 // Añadido el nivel de severidad SUCCESS
}

public static class CustomLogger
{
    public static void Log(string message, LogSeverity severity, 
        [System.Runtime.CompilerServices.CallerFilePath] string filePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
    {
        string color = GetColorForSeverity(severity);
        string formattedMessage = FormatMessage(message, severity, filePath, lineNumber, color);
        
        // Utiliza el método de logging adecuado según la severidad
        switch (severity)
        {
            case LogSeverity.ERROR:
                Debug.LogError(formattedMessage);
                break;
            case LogSeverity.WARNING:
                Debug.LogWarning(formattedMessage);
                break;
            case LogSeverity.CRITICAL:
                Debug.LogError(formattedMessage);
                break;
            case LogSeverity.SUCCESS:
                Debug.Log(formattedMessage);
                break;
            default:
                Debug.Log(formattedMessage);
                break;
        }
    }

    private static string GetColorForSeverity(LogSeverity severity)
    {
        //Se devuelve un color según la severidad del error
        switch (severity)
        {
            case LogSeverity.DEBUG: return "#A9A9A9"; // Gris oscuro
            case LogSeverity.INFO: return "#FFFFFF"; // Blanco
            case LogSeverity.WARNING: return "#FFD700"; // Oro
            case LogSeverity.ERROR: return "#FF4500"; // Naranja rojo
            case LogSeverity.CRITICAL: return "#B22222"; // Rojo fuego
            case LogSeverity.SUCCESS: return "#32CD32"; // Verde lima para éxito
            default: return "#000000"; // Negro por defecto
        }
    }

    private static string FormatMessage(string message, LogSeverity severity, string filePath, int lineNumber, string color)
    {
        //Se formatea el mensaje de error para mostrar en el log según su severidad
        return $"<color={color}><b> [{severity}]</b></color> " +
               $"<color=black>[{System.IO.Path.GetFileName(filePath)}:{lineNumber}]</color> " +
               $"<color=black>{message}</color>\n";
    }

}
