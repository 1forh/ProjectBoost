using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketShip : MonoBehaviour
{
    Rigidbody _rigidbody;
    AudioSource _audioSource;
    [SerializeField] float _mainThrust = 700.0f;
    [SerializeField] float _rcsThrust = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                SceneManager.LoadScene(1);
                break;
            case "Obstacle":
                SceneManager.LoadScene(0);
                break;
            default:
                break;
        }
    }

    private void ProcessInput()
    {
        Rotate();
        Thrust();
    }

    private void Rotate()
    {
        _rigidbody.freezeRotation = true;

        float _rotationThisFrame = Time.deltaTime * _rcsThrust;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * _rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * _rotationThisFrame);
        }

        _rigidbody.freezeRotation = false;
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rigidbody.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);

            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.Stop();
        }
    }
}
