using UnityEngine;

public static class ExtensionMethods
{
    public static void Log(this Object context, string message)
    {
#if UNITY_EDITOR
        Debug.Log(GetFormattedText(context.name, message, "white"), context);
#endif
    }

    public static void Log(this Object context, string message, float value, string color = "white")
    {
#if UNITY_EDITOR
        Debug.Log($"<b>{context.name}</b> <color={color}>{message}: <b>{value.ToString("0.000").Replace(',', '.')}</b></color>");
#endif
    }

    public static void LogWarning(this System.Type context, string message)
    {
        Debug.LogWarning(GetFormattedText(context.Name, message, "yellow"), null);
    }

    public static void LogWarning(this Object context, string message)
    {
        Debug.LogWarning(GetFormattedText(context.name, message, "yellow"), context);
    }

    public static void LogError(this Object context, string message)
    {
        Debug.LogError(GetFormattedText(context.name, message, "orange"), context);
    }

    static string GetFormattedText(string name, object message, string color)
    {
        return $"<b><color=white>{name}</color></b> <color={color}>{message}</color>";
    }
}