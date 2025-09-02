using UnityEngine;

public class Seguidor : MonoBehaviour
{
    [HideInInspector]public Transform alvo;
    public float suavidade = 1.5f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Limites da Câmera")]
    public bool usarLimites = false;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alvo = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (alvo != null)
        {
            Vector3 posicaoAlvo = alvo.position + offset;

            // Aplica limites se estiver habilitado
            if (usarLimites)
            {
                posicaoAlvo.x = Mathf.Clamp(posicaoAlvo.x, minX, maxX);
                posicaoAlvo.y = Mathf.Clamp(posicaoAlvo.y, minY, maxY);
            }

            transform.position = Vector3.Lerp(transform.position, posicaoAlvo, suavidade * Time.deltaTime);
        }
    }
}