using UnityEngine;

public class ThrowableDamage : MonoBehaviour
{
    public int damage = 1;

    private bool hasHit = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 1f)
        {
            if (!hasHit && collision.gameObject.CompareTag("Enemy"))
            {
                enemy enemyScript = collision.gameObject.GetComponent<enemy>();
                if (enemyScript != null)
                {
                    enemyScript.takeDamage(damage);
                }
                hasHit = true;
            }
        }        
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public void ResetHit()
    {
        hasHit = false;
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