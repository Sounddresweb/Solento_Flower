using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Asegura que el objeto tenga un AudioSource
public class DisparoBasico : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public Transform puntoDisparo;
    public float velocidadBala = 15f;

    [Header("Configuración de Audio")]
    public AudioClip sonidoDisparo;
    [Range(0f, 1f)] public float volumenDisparo = 0.7f;
    
    private AudioSource audioSource;

    void Start()
    {
        // Obtenemos la referencia al componente de audio
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Disparar();
        }
    }

    void Disparar()
    {
        // 1. Pedimos una bala al Pool
        GameObject bala = ObjectPooler.Instance.GetFromPool();

        if (bala != null)
        {
            // 2. Posicionamiento
            bala.transform.position = puntoDisparo.position;
            bala.transform.rotation = puntoDisparo.rotation;

            // 3. Física
            Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
            rb.linearVelocity = puntoDisparo.right * velocidadBala;

            // 4. Bloque de Audio
            ReproducirSonido();
        }
    }

    private void ReproducirSonido()
    {
        if (audioSource != null && sonidoDisparo != null)
        {
            // PlayOneShot permite superponer sonidos si disparas rápido
            audioSource.PlayOneShot(sonidoDisparo, volumenDisparo);
        }
    }
}