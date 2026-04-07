using UnityEngine;

public class MovimientoPlayer : MonoBehaviour

{

    public float velocidad = 5f;

    void Update()

    {

        float movX = Input.GetAxis("Horizontal"); // A / D

        float movY = Input.GetAxis("Vertical");   // W / S

        Vector3 movimiento = new Vector3(movX, movY, 0);

        transform.position += movimiento * velocidad * Time.deltaTime;

    }

}
