using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class inimigo : MonoBehaviour
{



    [SerializeField] private Slider slide;
    [SerializeField] private float maxHP;
    private NavMeshAgent agent;
    public Transform jogador;
    public float distanciaParaAndarRapido = 10.0f;
    public float velocidadeDevagar = 2.0f;
    public float velocidadeRapida = 6.0f;
    public float distanciaParaAtaque = 1.0f;
    public float distanciaParado = 11.0f;
    public float tempoDeAtaque = 2.0f;
    public float forcaDoEmpurrao = 5.0f;
    public float distanciaDaDeslizada = 0.5f;
    public float duracaoDaDeslizada = 0.5f;
    public GameObject[] tipoInimigo;
    public float VidaInimigo;
    private float tempoUltimoAtaque;
    private bool atacando;
    private bool deslizando;
    private Vector3 deslizadaTarget;
   public BarraHpFlutuante VidaBarra;
    public Transform pontoA;
    public Transform pontoB;
    public float distanciaDePatrulha = 1.0f;

    private bool emPerseguicao = false;

    [SerializeField] private GameObject Hamburger;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(pontoA.position);
        VidaInimigo = maxHP;
        tipoInimigo[0].SetActive(true);
        VidaBarra = GetComponentInChildren<BarraHpFlutuante>();
        VidaBarra.UpDateHealhBar(VidaInimigo, maxHP);
        tipoInimigo[0].SetActive(true);// Começa a patrulha no ponto A
    }

    void Update()
    {

       
        Morte();
        if (jogador != null && !atacando)
        {
            float distanciaJogador = Vector3.Distance(transform.position, jogador.position);

            RaycastHit hit;
            Vector3 directionToPlayer = jogador.position - transform.position;

            Debug.DrawRay(transform.position, directionToPlayer * distanciaParado, Color.red);
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Jogador"))
                {
                    if (distanciaJogador <= distanciaParaAtaque && Time.time - tempoUltimoAtaque > tempoDeAtaque)
                    {
                        AtacarJogador();
                        tempoUltimoAtaque = Time.time;
                        emPerseguicao = true;
                        agent.isStopped = false;
                        Debug.Log("Inimigo em perseguição!");
                    }

                    // Atualiza o ponto de patrulha atual para o jogador
                    agent.SetDestination(jogador.position);
                }
            }

            if (emPerseguicao)
            {
                // Persegue o jogador
                agent.SetDestination(jogador.position);

                // Encerra a perseguição se a distância for grande
                if (distanciaJogador > distanciaParaAndarRapido)
                {
                    emPerseguicao = false;
                    agent.isStopped = true;
                    Debug.Log("Perseguição encerrada. Retornando à patrulha.");
                }
            }
            else
            {
                // Continua patrulhando entre os pontos A e B
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    if (Vector3.Distance(transform.position, pontoA.position) < Vector3.Distance(transform.position, pontoB.position))
                    {
                        agent.SetDestination(pontoB.position);
                        Debug.Log("Chegou ao ponto A. Indo para o ponto B.");
                    }
                    else
                    {
                        agent.SetDestination(pontoA.position);
                        Debug.Log("Chegou ao ponto B. Indo para o ponto A.");
                    }
                }
            }
        }
    }

    void Morte()
    {
        if (VidaInimigo <= 0)
        {
            Destroy(gameObject);
        }
    }

    void AtacarJogador()
    {
        atacando = true;
        if (atacando == true)
            Hamburger.SetActive(true);
        agent.isStopped = true;
        // Lógica de ataque aqui
        // Quando o ataque estiver concluído, reative o NavMesh Agent e termine o ataque
    }
    //IEnumerator AguardarPatrulha()
    //{
    //    aguardandoPatrulha = true;
    //    yield return new WaitForSeconds(2.0f); // Tempo de espera antes de retomar a patrulha
    //    agent.isStopped = false;
    //    aguardandoPatrulha = false;

    //    // Continua patrulhando entre os pontos A e B
    //    if (Vector3.Distance(transform.position, pontoA.position) < Vector3.Distance(transform.position, pontoB.position))
    //    {
    //        agent.SetDestination(pontoB.position);
    //        Debug.Log("Chegou ao ponto A. Indo para o ponto B.");
    //    }
    //    else
    //    {
    //        agent.SetDestination(pontoA.position);
    //        Debug.Log("Chegou ao ponto B. Indo para o ponto A.");
    //    }
    //}


    void Deslizar()
    {
        if (agent.isStopped)
        {
            deslizando = true;
            deslizadaTarget = transform.position + transform.forward * distanciaDaDeslizada;
            StartCoroutine(RealizarDeslizada());
        }
    }

    IEnumerator RealizarDeslizada()
    {
        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < duracaoDaDeslizada)
        {
            elapsedTime = Time.time - startTime;
            float t = elapsedTime / duracaoDaDeslizada;
            transform.position = Vector3.Lerp(transform.position, deslizadaTarget, t);
            yield return null;
        }

        deslizando = false;
        agent.isStopped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tipoInimigo[0].name == "Z.C")
        {
            if (other.tag == "C")
            {
                if (VidaInimigo >= 0)
                {
                    VidaInimigo = VidaInimigo - 10;
                    VidaBarra.UpDateHealhBar(VidaInimigo, maxHP);
                }

                Destroy(other.gameObject);
            }
        }



        if (tipoInimigo[0].name == "Z.A")
        {
            if (other.tag == "A")
            {
                if (VidaInimigo >= 0)
                {
                    VidaInimigo = VidaInimigo - 10;
                    VidaBarra.UpDateHealhBar(VidaInimigo, maxHP);
                }


                Destroy(other.gameObject);
            }
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (tipoInimigo[0].name == "Z.B")
        {
            if (other.gameObject.layer == 7)
            {
                if (VidaInimigo >= 0)
                {
                    VidaInimigo = VidaInimigo - 10;
                    VidaBarra.UpDateHealhBar(VidaInimigo, maxHP);
                }


            }
        }
    }
}