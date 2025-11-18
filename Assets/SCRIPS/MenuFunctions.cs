using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("JuegoGranjaVR");
    }
}
