using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
   [SerializeField] public Image fillbar;
    [SerializeField] private GameObject MenuPause;
    [SerializeField]private string scena;

    private bool isPause;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))     
        {
            if(isPause) 
            {
                isPause = false;
                Time.timeScale = 1f;
                MenuPause.SetActive(false);
            }
            else{
                
                isPause = true;
                Time.timeScale = 0f;
                MenuPause.SetActive(true);
            }
        
        }
        fillbar.fillAmount = player.Hp / 100;
    }
    public void BottaoMenu()
    {
        SceneManager.LoadScene(scena);
    }
    public void Sair()
    {
        Application.Quit();
    }
}
