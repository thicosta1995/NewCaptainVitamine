using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string nomeDoLevelDoJogo;
    [SerializeField] private GameObject PainelMenuPrincipal;
    [SerializeField] private GameObject painelOpcoes;
  

    public void Jogar()
    {
        SceneManager.LoadScene(nomeDoLevelDoJogo);
    }

    public void OpcoesAbrir()
    {
        PainelMenuPrincipal.SetActive(false);
    }
    public void OpcoesFechar()
    {

    }

    public void Creditos()
    {

    }
    public void Sair() 
    {
        Application.Quit();
    }
}
