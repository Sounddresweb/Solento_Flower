using UnityEngine;

public class DisparoBasico : MonoBehaviour
{
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    public float velocidadBala = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            Disparar();
        }
    }

    void Disparar()
    {
        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);

        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        rb.linearVelocity = puntoDisparo.right * velocidadBala;
    }
}