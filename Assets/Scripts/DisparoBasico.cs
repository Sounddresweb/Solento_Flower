using UnityEngine;

public class DisparoBasico : MonoBehaviour
{
    // Ya no necesitamos 'balaPrefab' aquí porque el Pool lo gestiona
    public Transform puntoDisparo;
    public float velocidadBala = 15f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Disparar();
        }
    }

    void Disparar()
    {
        // 1. Pedimos una bala al Pool en lugar de crear una nueva
        GameObject bala = ObjectPooler.Instance.GetFromPool();

        // 2. Si recibimos una bala válida, la posicionamos
        if (bala != null)
        {
            bala.transform.position = puntoDisparo.position;
            bala.transform.rotation = puntoDisparo.rotation;

            // 3. Aplicamos la velocidad
            Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
            rb.linearVelocity = puntoDisparo.right * velocidadBala;
        }
    }
}