using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controle;
    public float moveSpeed = 1f;
    public float jumpForce = 5f;
    public Vector3 xVelocity;
    public Vector3 yVelocity;
    public Vector3 finalVelocity;
    private Rigidbody rb;
    public float gravity;
    private bool isJumping = false;
    public float maxHeight= 2f;
    public float jumpSpeed;
    public float timeToPeak =1f;
    public float Hp;
    public Text Hptx;
    public GameObject[] armas; // Um array de GameObjects representando suas diferentes armas.
    public int armaAtual = 0; // O índice da arma atual.
    ParticleSystem leite;
    private void Awake()
    {

        controle = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Hp = 100;
        gravity = 2 * maxHeight / Mathf.Pow(timeToPeak, 2);
        jumpSpeed = gravity * timeToPeak;
    }
    private void Start()
    {
        AtualizarArma();
    }

    private void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        xVelocity = moveSpeed * xInput * Vector3.right;
      
        yVelocity += gravity * Time.deltaTime * Vector3.down;
        if(controle.isGrounded)
        {
            yVelocity = Vector3.down;
        }
    
        if(Input.GetKeyDown(KeyCode.W) && controle.isGrounded)
        {
            yVelocity = jumpSpeed * Vector3.up;
        }
        finalVelocity = -xVelocity + yVelocity;
        controle.Move(finalVelocity * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TrocarArma();
        }
      if(armaAtual ==1)
        {
            leite.Play();
        }
    }

    void TrocarArma()
    {
        // Desativa a arma atual.
        armas[armaAtual].SetActive(false);

        // Muda para a próxima arma, ou volta para a primeira se atingir o final do array.
        armaAtual = (armaAtual + 1) % armas.Length;

        // Ativa a nova arma.
        AtualizarArma();
    }
    void AtualizarArma()
    {
        armas[armaAtual].SetActive(true);
    }
    private void FixedUpdate()
    {

        //float moveInput = Input.GetAxis("Horizontal");
        //Move(-moveInput);

        //if (Input.GetButtonDown("Jump") && IsGrounded())
        //{
        //    Jump();
        //}
        float xInput = Input.GetAxis("Horizontal");
        xVelocity = moveSpeed * xInput * Vector3.right;

        yVelocity += gravity * Time.deltaTime * Vector3.down;
        if (controle.isGrounded)
        {
            yVelocity = Vector3.down;
        }

        if (Input.GetKeyDown(KeyCode.W) && controle.isGrounded)
        {
            yVelocity = jumpSpeed * Vector3.up;
        }
        finalVelocity = -xVelocity + yVelocity;
        controle.Move(finalVelocity * Time.deltaTime);
        Hptx.text = Hp.ToString();
    }

    private void Move(float inputValue)
    {
        Vector3 movement = new Vector3(inputValue * moveSpeed , 0f, 0f);
        rb.MovePosition(transform.position + movement);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        isJumping = true;
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        float raycastDistance = 1f; // Aumentamos a distância do raio para garantir que atinja o chão adequadamente
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f; // Adicionamos uma pequena elevação ao ponto de origem do raio para evitar colisões imediatas
        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance))
        {
            isJumping = false;
            return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
        if(collision.gameObject.CompareTag("C"))
        {
            Hp = Hp - 20;
        }
        if (collision.gameObject.CompareTag("B"))
        {
            Hp = Hp - 20;
        }
        if (collision.gameObject.CompareTag("D"))
        {
            Hp = Hp - 20;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Hamburger"))
        {
            Hp = Hp - 20;
        }
       
    }
}
