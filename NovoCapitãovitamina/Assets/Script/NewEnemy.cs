using System.Collections;
using System.Collections.Generic;

using UnityEngine;



public class NewEnemy : MonoBehaviour
{
    public float velocidade = 5f; // Defina a velocidade desejada aqui
    public Transform pontoA;
    public Transform pontoB;
    public float playerdistance;
    public Transform player;
    public float ataqueDistace;
    public Rigidbody rb;
    public int direction;
    private enum EstadoInimigo
    {
        Patrulha,
        PerseguirJogador,
        Atacar
    }


    private EstadoInimigo estadoAtual;
    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        rb = GetComponent<Rigidbody>();
        direction = (int)transform.localScale.x;
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        getDistancePlayer();
    }
  void getDistancePlayer()
    {
        playerdistance = player.position.x - transform.position.x;
        Debug.Log(playerdistance);

    }
}


