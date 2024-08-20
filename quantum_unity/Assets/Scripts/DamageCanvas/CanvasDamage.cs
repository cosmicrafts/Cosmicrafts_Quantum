using UnityEngine;
using TMPro;
using Quantum;

public class CanvasDamage : MonoBehaviour
{
    [SerializeField]
    TMP_Text damageText;

    [Header("Color Settings")]
    public Color noneColor = Color.gray;
    public Color criticColor = Color.yellow;
    public Color evasionColor = Color.magenta;

    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        //damageText.text = damageValue.ToString();
        Destroy(gameObject, .5f);
    }

    public void SetDamage(float newDamage, EAttackMode attackMode)
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (newDamage == 0 && attackMode == EAttackMode.Evasion)
        {
            damageText.text = "Miss";
            damageText.color = evasionColor;
        }
        else
        {
            switch (attackMode)
            {
                case EAttackMode.None:
                    damageText.color = noneColor;
                    break;

                case EAttackMode.Critic:
                    damageText.color = criticColor;
                    break;

                case EAttackMode.Evasion:
                    damageText.color = evasionColor;
                    break;

                default:
                    damageText.color = noneColor;
                    break;
            }

            damageText.text = "" + (int)newDamage;
        }

        if (mainCamera)
        {
            transform.rotation = mainCamera.transform.rotation;
        }

        Invoke("ReturnToPool", 1.5f);
    }

    private void ReturnToPool()
    {
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }
        else
        {
            Debug.LogError("ObjectPoolManager instance is not initialized.");
            Destroy(gameObject);
        }
    }
}
