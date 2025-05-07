using UnityEngine;

public class ThrowableDamage : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 10f;
    public int damage = 10;
    public LayerMask groundLayer;
    //private bool hasHit = false;
    [SerializeField] private AudioClip explodeSoundClip;

    void OnCollisionEnter(Collision collision)
    {
        //check if collision with ground
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            SoundFXManager.instance.PlaySoundFXClip(explodeSoundClip, transform, 0.6f);
            Explode();
        }
        //if (collision.relativeVelocity.magnitude > 1f)
        //{
        //    if (!hasHit && collision.gameObject.CompareTag("Enemy"))
        //    {
        //        enemy enemyScript = collision.gameObject.GetComponent<enemy>();
        //        if (enemyScript != null)
        //        {
        //            enemyScript.takeDamage(damage);
        //        }
        //        hasHit = true;
        //    }
        //}        
    }
    private void Explode()
    {
        //spawn visual explosion

        //Add explosion logic
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // Apply damage to the enemy
                collider.GetComponent<enemy>().takeDamage(damage);
            }

            // Apply force to rigidbody if needed (for physics-based reactions)
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        //destroy
        Destroy(gameObject);
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public void SetTransparency(bool isTransparent)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color color = renderer.material.color;
            if (isTransparent)
            {
                color.a = 0.3f;  // semi-transparent
                renderer.material.SetFloat("_Mode", 3); // set rendering mode to transparent if using Standard Shader
                renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                renderer.material.SetInt("_ZWrite", 0);
                renderer.material.DisableKeyword("_ALPHATEST_ON");
                renderer.material.EnableKeyword("_ALPHABLEND_ON");
                renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                renderer.material.renderQueue = 3000;
            }
            else
            {
                color.a = 1f;  // fully opaque
                renderer.material.SetFloat("_Mode", 0); // set back to opaque
                renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                renderer.material.SetInt("_ZWrite", 1);
                renderer.material.DisableKeyword("_ALPHABLEND_ON");
                renderer.material.renderQueue = -1;
            }
            renderer.material.color = color;
        }
    }

}