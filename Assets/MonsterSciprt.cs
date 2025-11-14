using UnityEngine;

public class MonsterSciprt : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void on2DCollisionEnter(Collision2D collision)
    {
        Debug.Log("Player Hurt!");
    }   
}
