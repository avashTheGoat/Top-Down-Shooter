using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Utils Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
    }

    public void PrintWarning(string _printStatement)
    {
        Debug.LogWarning(_printStatement);
    }

    public void PrintError(string _printStatement)
    {
        Debug.LogError(_printStatement);
    }
}