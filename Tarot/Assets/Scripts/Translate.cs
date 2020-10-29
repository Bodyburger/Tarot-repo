using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
    }
    void MoveCharacter(Vector2 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
