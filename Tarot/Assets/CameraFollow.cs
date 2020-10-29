using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 camOffset = new Vector3(0, 0, -0.3f);
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerObject");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position + camOffset;
    }
}
