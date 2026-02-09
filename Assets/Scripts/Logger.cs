using System.Diagnostics;
using UnityEngine;


// Simple logging system for Unity with preprocessor support

public static class Logger
{
    
    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    
    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(object message, Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

   
    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

   
    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogWarning(object message, Object context)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }
    
    public static void LogError(object message)
    {
         UnityEngine.Debug.LogError(message);
    }

    
    public static void LogError(object message, Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }
    
    public static void LogException(System.Exception exception)
    {
        UnityEngine.Debug.LogException(exception);
    }
    
    public static void LogException(System.Exception exception, Object context)
    {
        UnityEngine.Debug.LogException(exception, context);
    }
}