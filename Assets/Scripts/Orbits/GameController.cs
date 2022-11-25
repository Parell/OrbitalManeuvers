using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public double UniversalTime;
    public int TimeScale;
    public int FrameRateLimit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            Debug.Log($"Cant have more then one Instance on {gameObject.name}.");
        }
    }

    private void Start()
    {
        Application.targetFrameRate = FrameRateLimit;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < TimeScale; i++)
        {
            UniversalTime += Time.fixedDeltaTime;
        }
    }
}
