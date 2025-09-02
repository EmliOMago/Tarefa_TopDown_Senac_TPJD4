using UnityEngine;

public class Inimigo : MonoBehaviour
{
    Transform alvo;
    public int velocidade;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alvo = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (alvo == null)
        {
            return;
        }
        Vector3 direcao = alvo.position - transform.position;
        direcao = direcao.normalized;

        if (Vector2.Distance(transform.position, alvo.position) < 1)
        {
            transform.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            return;
        }

        transform.position += direcao * velocidade * Time.deltaTime;
        
    }
}
