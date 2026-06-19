using System;

using UnityEngine;

using UnityEngine.InputSystem;



public class PlayerWASDMovement : MonoBehaviour

{

    [SerializeField] private float movementPerSecond;

    [SerializeField] private float rotationPerSecond;

    [SerializeField] private GameObject camera;


    private void Start()
    {

    }


    void Update()
    {

        if (Input.GetKey(KeyCode.W))

            transform.position += transform.forward * movementPerSecond * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))

            transform.position += -transform.right * movementPerSecond * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))

            transform.position += -transform.forward * movementPerSecond * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))

            transform.position += transform.right * movementPerSecond * Time.deltaTime;


        if (Input.GetMouseButton(1))

        {

            camera.transform.RotateAround(transform.position, new Vector3(0, 1, 0), rotationPerSecond * Time.deltaTime);

        }

        if (Input.GetMouseButton(0))

        {

            camera.transform.RotateAround(transform.position, new Vector3(0, 1, 0), -rotationPerSecond * Time.deltaTime);

        }

    }


}