using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class inimigo : MonoBehaviour
{
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
    public bool inimigoParado;
    private float tempoUltimoAtaque;
    private bool atacando;
    private bool deslizando;
    private Vector3 deslizadaTarget;

    public Transform pontoA;
    public Transform pontoB;
    public float distanciaDePatrulha = 1.0f;

    private Transform pontoPatrulhaAtual;
    private bool indoParaPontoA = true;

    private bool emPerseguicao = false;

    [SerializeField] private GameObject Hamburger;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pontoPatrulhaAtual = pontoA;
        inimigoParado = true;
        tipoInimigo[0].SetActive(true);
        agent.SetDestination(pontoA.position);
    }

    void Update()
    {
        morte();
        if (jogador != null && !atacando)
        {
            float distanciaJogador = Vector3.Distance(transform.position, jogador.position);

            RaycastHit hit;
            Vector3 directionToPlayer = jogador.position - transform.position;

            // Sempre manter o Raycast ativo
            Debug.DrawRay(transform.position, directionToPlayer * distanciaParado, Color.red);
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Jogador"))
                {
                    // Se o jogador estiver à vista, atacar
                    if (distanciaJogador <= distanciaParaAtaque && Time.time - tempoUltimoAtaque > tempoDeAtaque)
                    {
                        AtacarJogador();
                        tempoUltimoAtaque = Time.time;
                        emPerseguicao = true; // Inicia a perseguição quando ataca
                        agent.isStopped = false; // Garante que o agente não está parado durante a perseguição
                        Debug.Log("Inimigo em perseguição!");
                    }

                    // Atualiza o ponto de patrulha atual para o jogador
                    pontoPatrulhaAtual = jogador;
                }
            }

            if (emPerseguicao == true)
            {
                // Se em perseguição, persegue o jogador
                agent.SetDestination(jogador.position);

                // Se a distância entre o inimigo e o jogador for grande, encerra a perseguição
                if (distanciaJogador > distanciaParaAndarRapido)
                {
                    emPerseguicao = false;
                    agent.isStopped = false;
                    Debug.Log("Perseguição encerrada. Retornando à patrulha.");
                }
            }
            else
            {
                // Se não estiver em perseguição, continua usando pontos de patrulha
                if (distanciaJogador > distanciaParaAndarRapido && distanciaJogador <= distanciaParado)
                {
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, distanciaParado))
                    {
                        if (hit.collider.CompareTag("Parede"))
                        {
                            Vector3 newDirection =
                                Vector3.RotateTowards(transform.forward, hit.normal, Time.deltaTime * 2.0f, 0.0f);
                            transform.rotation = Quaternion.LookRotation(newDirection);
                            agent.isStopped = true;
                        }
                        else
                        {
                            agent.isStopped = false;
                        }
                    }
                }

                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    // Continua patrulhando entre os pontos A e B
                    if (pontoPatrulhaAtual == pontoA)
                    {
                        agent.SetDestination(pontoB.position);
                        pontoPatrulhaAtual = pontoB;
                        Debug.Log("Chegou ao ponto A. Indo para o ponto B.");
                    }
                    else
                    {
                        agent.SetDestination(pontoA.position);
                        pontoPatrulhaAtual = pontoA;
                        Debug.Log("Chegou ao ponto B. Indo para o ponto A.");
                    }
                }
            }
        }

    }

    // Restante do código...

    void morte()
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
                    VidaInimigo = VidaInimigo - 1;
                }

               
            }
        }
    }
}