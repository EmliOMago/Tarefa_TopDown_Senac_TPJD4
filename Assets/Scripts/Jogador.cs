using UnityEngine;
using UnityEngine.Diagnostics;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Jogador : MonoBehaviour
{
    public float velocidade = 5;
    bool cutscene;
    float horizontal;
    float vertical;

    Rigidbody2D corpo;
    [HideInInspector] public GameObject Alvo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        corpo = transform.GetComponent<Rigidbody2D>();
        cutscene = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cutscene)
        {
            ProcessarEntradas();
            return;
        }
    }

    // FixedUpdate is called once per frame in specific frame
    void FixedUpdate()
    {
        if (!cutscene)
        {
            Movimentar();
            //Rotacionar();
            RotacionarPeloMouse();
            return;
        }
        OlharParaNPC();
    }

    void Movimentar()
    {
        Vector2 movimemento = new Vector2(horizontal, vertical);

        if (movimemento.magnitude > 1)
        {
            movimemento = movimemento.normalized;
        }

        corpo.linearVelocity = movimemento * velocidade;
    }

    void Rotacionar()
    {
        if (vertical == 0 && horizontal == 0)
        {
            return;
        }

        float angulo = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
        corpo.rotation = angulo;
    }

    void RotacionarPeloMouse()
    {
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Alvo = GameObject.FindWithTag("NPC");

        if (cutscene)
        {
            Utils.OlharParaObjeto(transform, posicaoMouse);

            return;
        }
        //Vector3 direcao = (posicaoMouse - transform.position).normalized;

        //float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        //corpo.rotation = angulo;
    }

    void ProcessarEntradas()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        cutscene = Input.GetButton("Jump");
    }

    void OlharParaNPC()
    {
        Utils.OlharParaObjeto(transform, Alvo.transform.position);
    }
}
