using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float lifeTime = 2f;

    void OnEnable()
    {
        Invoke(nameof(Deactivate), lifeTime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            Deactivate();
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}