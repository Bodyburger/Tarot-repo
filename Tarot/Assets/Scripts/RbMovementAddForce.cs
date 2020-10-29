﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbMovementAddForce : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    public Rigidbody2D rb;
    public Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate() {
        MoveCharacter(movement);
    }
    void MoveCharacter(Vector2 direction) {
        rb.AddForce(direction * speed);
    }
}
