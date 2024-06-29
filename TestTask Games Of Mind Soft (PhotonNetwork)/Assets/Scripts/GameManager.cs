using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string winCode;
    public static string code;

    public static void ResetCode()
    {
        winCode = "";
        code = "222222222";
    }

    private void Awake() => ResetCode();
}