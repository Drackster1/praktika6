using UnityEngine;

public class sphere2 : MonoBehaviour
{
    public Vector3 Speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().linearVelocity = Speed * Time.deltaTime;
    }
}
