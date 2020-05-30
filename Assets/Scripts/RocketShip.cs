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
    [SerializeField] AudioClip _mainEngineAudioClip;
    [SerializeField] AudioClip _successAudioClip;
    [SerializeField] AudioClip _deathAudioClip;
    [SerializeField] ParticleSystem _mainEngineParticleSystem;
    [SerializeField] ParticleSystem _successParticleSystem;
    [SerializeField] ParticleSystem _deathParticleSystem;

    enum State { Alive, Dying, Transcending };
    [SerializeField] State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            ProcessInput();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (state != State.Alive) { return; }   

        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Obstacle":
                StartDeathSequence();
                break;
            default:
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_deathAudioClip);
        _deathParticleSystem.Play();
        Invoke("LoadFirstLevel", 3.0f); // serialize
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_successAudioClip);
        _successParticleSystem.Play();
        Invoke("LoadNextLevel", 2.3f); // serialize
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void ProcessInput()
    {
        RespondToRotateInput();
        RespondToThrustInput();
    }

    private void RespondToRotateInput()
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

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rigidbody.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);

            if (!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(_mainEngineAudioClip);
                _mainEngineParticleSystem.Play();
            }
        }
        else
        {
            _audioSource.Stop();
            _mainEngineParticleSystem.Stop();
        }
    }
}
