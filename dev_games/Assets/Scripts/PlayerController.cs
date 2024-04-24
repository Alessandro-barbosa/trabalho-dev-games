using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 15.0f;
    public float turnSpeed = 45.0f;
    private float horizontalInput;
    private float forwardInput;
    private bool isSKeyPressed = false;

    private Rigidbody playerRb;
    public float jumpForce;
    public float gravityModifier;

    public bool isOnground = true;

    private Animator playerAnim;

    // Start is called before the first frame updates
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSKeyPressed == false)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            forwardInput = Input.GetAxis("Vertical");
            // Move o player pra "cima" com o input vertical
            transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
            // Rotacionar o player com o input horizontal
            transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
        }
        // Pular se o espaço está pressionado
        if (Input.GetKeyDown(KeyCode.Space) && isOnground)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnground = false;
            playerAnim.SetTrigger("Jump_Trigger");
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            playerAnim.SetBool("W_B", true);
            isSKeyPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.SetBool("W_B", false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            playerAnim.SetBool("S_B", true);
            isSKeyPressed = true;
        }

        if (isSKeyPressed == true)
        {
            forwardInput = Input.GetAxis("Vertical");
            // Move o player pra "cima" com o input vertical
            transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
            horizontalInput = Input.GetAxis("Horizontal");
            // Move o player pra "cima" com o input vertical
            // Rotacionar o player com o input horizontal
            transform.Rotate(Vector3.up, turnSpeed * -horizontalInput * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.SetBool("S_B", false);
            isSKeyPressed = false;
        }
        if (isOnground)
        {
            // Resetar a rotação em torno do eixo Z (roll)
            playerRb.rotation = Quaternion.Euler(playerRb.rotation.eulerAngles.x, playerRb.rotation.eulerAngles.y, 0f);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            playerAnim.SetTrigger("Right_Mouse_Trigger");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isOnground = true;
    }
}
