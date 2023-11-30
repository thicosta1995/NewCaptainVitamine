using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class WeaponController : MonoBehaviour
{
    public List<Transform> firePoints;
    
    public Camera mainCamera;
    [SerializeField] private Transform Player;
    [SerializeField] private ParticleSystem leite;
    [SerializeField] private float municaoDeLeite;
    [SerializeField] private float municaoDeLeiteMax= 10000;
    public bool semLeite;
    public bool recarregarLeite;
    [SerializeField] PlayerMovement player;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public int numeroTiros = 3;
    public float intervaloTiros = 0.1f;
    public GameObject explosiveProjectilePrefab;
    [SerializeField] private int municaoDeLaraja;
    [SerializeField] private int municaoDeLarajaMax= 300;
    public bool recarregandoAlaranja;
    public bool semLaranja;
    private bool isFiring; // Indica se a arma está atirando
    public GameObject[] armas; // Um array de GameObjects representando suas diferentes armas.
    private int armaAtual = 0; // O índice da arma atual.


    private void Start()
    {
        municaoDeLaraja = municaoDeLarajaMax;
        semLaranja = false;
        recarregandoAlaranja = false;
        municaoDeLeite = municaoDeLeiteMax;
        semLeite = false;
        recarregarLeite = false;
    }
    private void Update()
    {
      
        Vector3 mousePositionScreen = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(mousePositionScreen);
        if(municaoDeLaraja <=0)
        {
            semLaranja = true;
        }
        if(municaoDeLeite <= 0) 
        {
            semLeite = true;
        }
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = hit.point - transform.position;
            direction.Normalize();

            SetWeaponDirection(direction);
        }

        if (Input.GetButtonDown("Fire1") && player.armaAtual == 1 && municaoDeLaraja > 0)
        {
            DispararTiros();
        }
        else if(Input.GetButtonUp("Fire1") && player.armaAtual == 1)
        {

        }
        if (Input.GetButtonDown("Fire1") && player.armaAtual == 0 && municaoDeLeite >0)
        {
            leite.Play();
           
           
        }
        else if(Input.GetButtonUp("Fire1") && player.armaAtual == 0)
              leite.Stop();
        if(recarregandoAlaranja == true)
        {
            municaoDeLaraja = municaoDeLarajaMax;
            semLaranja = false;
            recarregandoAlaranja = false;
        }
        if(recarregarLeite == true)
        {
            municaoDeLeite = municaoDeLeiteMax;
            semLeite = false;
            recarregarLeite = false;
        }

        animaçãoLeite();
    }

 
    void SetWeaponDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
  
    }

    //void SetWeaponDirectionUp(Vector3 direction)
    //{
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    angle -= 90f;
    //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
    //}

    //void SetWeaponDirectionRight(Vector3 direction)
    //{
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    angle -= 180f;
    //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
    //}

    //void SetWeaponDirectionDown(Vector3 direction)
    //{
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    angle -= 270f;
    //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
    //}

    //void SetWeaponDirectionLeft(Vector3 direction)
    //{
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
    //}

    void DispararTiros()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(DispararTirosCoroutine());
        }
    }

    void DispararTirosExplosivos()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(DispararTirosExplosivosCoroutine());
        }
    }
    void animaçãoLeite()

    {
        if(leite.isPlaying && municaoDeLeite>=0)
        {
            municaoDeLeite = municaoDeLeite - 1;
        }
        else if(municaoDeLeite<=0)
        {
            leite.Stop();
        }
    }
    IEnumerator DispararTirosCoroutine()
    {
        for (int i = 0; i < numeroTiros; i++)
        {
            foreach (Transform firePoint in firePoints)
            {
                GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Rigidbody projectileRigidbody = projectileGO.GetComponent<Rigidbody>();
                projectileRigidbody.AddForce(-firePoint.forward * projectileSpeed, ForceMode.Impulse);
                municaoDeLaraja = municaoDeLaraja - 15;


            }

            yield return new WaitForSeconds(intervaloTiros);
        }

        isFiring = false;
     
    }

    IEnumerator DispararTirosExplosivosCoroutine()
    {
        for (int i = 0; i < numeroTiros; i++)
        {
            //foreach (Transform firePoint in firePoints)
            //{
            //    GameObject explosiveProjectileGO = Instantiate(explosiveProjectilePrefab, firePoint.position, firePoint.rotation);
            //    Rigidbody explosiveProjectileRigidbody = explosiveProjectileGO.GetComponent<Rigidbody>();
            //    explosiveProjectileRigidbody.AddForce(firePoint.right * projectileSpeed, ForceMode.Impulse);
            //}
            yield return new WaitForSeconds(intervaloTiros);
        }
        
        isFiring = false;
    }
}