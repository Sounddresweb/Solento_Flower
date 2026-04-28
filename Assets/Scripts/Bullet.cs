using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 2f;

    void OnEnable()
    {
        Invoke(nameof(Deactivate), lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void Deactivate()
    {
        // Cancelamos el Invoke por seguridad
        CancelInvoke();
        
        // CORRECCIÓN: Devolvemos la bala al pool en lugar de solo desactivarla
        ObjectPooler.Instance.ReturnToPool(this.gameObject);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}