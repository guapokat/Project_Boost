﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
    }

    private void ProcessInput() {

        // Can thrust while rotating
        if(Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(Vector3.up);
            // So the audio doesn't layer
            if(!audioSource.isPlaying) {
                audioSource.Play();
            }
        } else {
            audioSource.Stop();
        }

        if(Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward);
        } else if(Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward);
        }

    }
}