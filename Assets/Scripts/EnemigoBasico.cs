using UnityEngine;

public class EnemigoBasico : MonoBehaviour
{
    public Transform jugador;
    public float velocidad = 3f;

    void Update()
    {
        if (jugador != null)
        {
            // Dirección hacia el jugador
            Vector2 direccion = (jugador.position - transform.position).normalized;

            // Movimiento
            transform.position += (Vector3)direccion * velocidad * Time.deltaTime;
        }
    }
}