using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Jogador : MonoBehaviour
{
    public float velocidade;

    float horizontal;
    float vertical;

    Rigidbody2D corpo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        corpo = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessarEntradas();
    }

    // FixedUpdate is called once per frame in specific frame
    void FixedUpdate()
    {
        Movimentar();
    }

    void Movimentar()
    {
        corpo.linearVelocity += new Vector2(horizontal, vertical) * velocidade;
    }
    
    void ProcessarEntradas()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
}
