using UnityEngine;

public class Seguidor : MonoBehaviour
{
    public Transform jogador;
    public float suavidade = 5f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Limites da Câmera")]
    public bool usarLimites = true;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    void LateUpdate()
    {
        if (jogador != null)
        {
            Vector3 posicaoAlvo = jogador.position + offset;

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