using UnityEngine;
using UnityEngine.Events;

public class GazeButton : MonoBehaviour
{
    public UnityEvent onGazeComplete;
    public float gazeTime = 3f;

    float timer = 0f;
    bool isGazing = false;

    public void StartGaze()
    {
        isGazing = true;
        timer = 0f;
    }

    public void EndGaze()
    {
        isGazing = false;
        timer = 0f;
    }

    void Update()
    {
        if (isGazing)
        {
            timer += Time.deltaTime;
            if (timer >= gazeTime)
            {
                onGazeComplete.Invoke();
                isGazing = false;
            }
        }
    }
}
