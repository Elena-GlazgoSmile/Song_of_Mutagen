using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    public float speed = 10.0f;
    public float maxSpeed = 15.0f;
    public float rotationSpeed = 360.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Получаем ввод от клавиатуры
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Получаем ввод от мыши
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * rotationSpeed;
        transform.localEulerAngles = new Vector3(0, rotationX, 0);

        // Применяем силу для перемещения
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddRelativeForce(movement * speed);

        // Ограничение скорости
        Vector3 clampedVelocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        rb.velocity = clampedVelocity;

        // передаём скорость в аниматор
        float animSpeed = rb.velocity.magnitude;
        anim.SetFloat("Speed", animSpeed);
    }
}