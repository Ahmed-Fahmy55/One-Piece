using UnityEngine;

public class Test : MonoBehaviour
{


    [SerializeField] float speed;


    private void Start()
    {
        PrintData();
    }


    void PrintData()
    {
        do
        {
            Debug.Log("speed is :" + speed);
            break;
        }
        while (speed > 0);

    }

}


