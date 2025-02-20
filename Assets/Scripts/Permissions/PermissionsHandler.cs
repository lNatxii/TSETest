using UnityEngine;
using UnityEngine.Android;
public static class PermissionsHandler
{
    public static void RequestPermission(string permissionType)
    {
        Debug.Log("Requesting permission: " + permissionType);
        if (!CheckPermission(permissionType))
            Permission.RequestUserPermission(Permission.FineLocation);
    }
    
    public static bool CheckPermission(string permissionType)
    {
        return Permission.HasUserAuthorizedPermission(permissionType);
    }
}
