using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils : MonoBehaviour
{
    #region FUNÇÕES DE MOVIMENTO

    /// <summary>
    /// Move o objeto horizontalmente com entrada do teclado
    /// </summary>
    /// <param name="velocidade">Velocidade de movimento</param>
    public void MoverHorizontal(float velocidade)
    {
        float entradaMovimento = Input.GetAxisRaw("Horizontal");
        Vector2 movimento = new Vector2(entradaMovimento * velocidade * Time.deltaTime, 0);
        transform.Translate(movimento);
    }

    /// <summary>
    /// Move o objeto verticalmente com entrada do teclado
    /// </summary>
    /// <param name="velocidade">Velocidade de movimento</param>
    public void MoverVertical(float velocidade)
    {
        float entradaMovimento = Input.GetAxisRaw("Vertical");
        Vector2 movimento = new Vector2(0, entradaMovimento * velocidade * Time.deltaTime);
        transform.Translate(movimento);
    }

    /// <summary>
    /// Move o objeto em 8 direções (WASD/Teclas de Seta)
    /// </summary>
    /// <param name="velocidade">Velocidade de movimento</param>
    public void MoverEm8Direcoes(float velocidade)
    {
        float moverX = Input.GetAxisRaw("Horizontal");
        float moverY = Input.GetAxisRaw("Vertical");
        Vector2 movimento = new Vector2(moverX, moverY).normalized * velocidade * Time.deltaTime;
        transform.Translate(movimento);
    }

    /// <summary>
    /// Move o objeto em direção a uma posição alvo
    /// </summary>
    /// <param name="posicaoAlvo">Posição alvo</param>
    /// <param name="velocidade">Velocidade de movimento</param>
    public void MoverParaPosicao(Vector2 posicaoAlvo, float velocidade)
    {
        transform.position = Vector2.MoveTowards(transform.position, posicaoAlvo, velocidade * Time.deltaTime);
    }

    /// <summary>
    /// Aplica força para pular
    /// </summary>
    /// <param name="forcaPulo">Força do pulo</param>
    /// <param name="corpoRigido">Rigidbody2D do objeto</param>
    /// <param name="verificarChao">Verifica se está no chão</param>
    public void Pular(float forcaPulo, Rigidbody2D corpoRigido, bool estaNoChao)
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            corpoRigido.linearVelocity = new Vector2(corpoRigido.linearVelocity.x, forcaPulo);
        }
    }

    public static void OlharParaObjeto(Transform origem, Vector3 objeto)
    {
        Vector3 direcao = (objeto - origem.position).normalized;

        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        if (origem.GetComponent<Rigidbody2D>())
        {
            origem.GetComponent<Rigidbody2D>().rotation = angulo;
        }
        else
        {
            origem.eulerAngles = new Vector3(0, 0, angulo);
        }
    }


    #endregion

    #region FUNÇÕES DE COLISÃO E GATILHO

    /// <summary>
    /// Verifica se o objeto está no chão
    /// </summary>
    /// <param name="posicaoVerificacaoChao">Posição da verificação de chão</param>
    /// <param name="raioVerificacao">Raio da verificação</param>
    /// <param name="camadaChao">Layer do chão</param>
    public bool EstaNoChao(Vector2 posicaoVerificacaoChao, float raioVerificacao, LayerMask camadaChao)
    {
        return Physics2D.OverlapCircle(posicaoVerificacaoChao, raioVerificacao, camadaChao);
    }

    /// <summary>
    /// Verifica colisão com tag específica
    /// </summary>
    private void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Inimigo"))
        {
            ManipularColisaoInimigo();
        }
        else if (colisao.gameObject.CompareTag("Coletavel"))
        {
            ManipularColisaoColetavel(colisao.gameObject);
        }
    }

    /// <summary>
    /// Verifica gatilho com tag específica
    /// </summary>
    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("ZonaMorte"))
        {
            ManipularMorte();
        }
        else if (outro.CompareTag("PontoVerificacao"))
        {
            ManipularPontoVerificacao(outro.transform.position);
        }
    }

    #endregion

    #region FUNÇÕES DE ANIMAÇÃO

    /// <summary>
    /// Controla animações baseadas no movimento
    /// </summary>
    /// <param name="animador">Componente Animator</param>
    /// <param name="estaMovendo">Se está se movendo</param>
    /// <param name="estaNoChao">Se está no chão</param>
    public void ControlarAnimacoesMovimento(Animator animador, bool estaMovendo, bool estaNoChao)
    {
        animador.SetBool("EstaMovendo", estaMovendo);
        animador.SetBool("EstaNoChao", estaNoChao);

        // Define a direção do sprite baseado no movimento horizontal
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    /// <summary>
    /// Toca uma animação específica
    /// </summary>
    /// <param name="animador">Componente Animator</param>
    /// <param name="nomeAnimacao">Nome da animação</param>
    public void TocarAnimacao(Animator animador, string nomeAnimacao)
    {
        animador.Play(nomeAnimacao);
    }

    #endregion

    #region FUNÇÕES DE ENTRADA

    /// <summary>
    /// Verifica se o botão de ação foi pressionado
    /// </summary>
    public bool BotaoAcaoPressionado()
    {
        return Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return);
    }

    /// <summary>
    /// Verifica se o botão de ataque foi pressionado
    /// </summary>
    public bool BotaoAtaquePressionado()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J);
    }

    /// <summary>
    /// Obtém a posição do mouse no mundo 2D
    /// </summary>
    public Vector2 ObterPosicaoMouseMundo()
    {
        Vector3 posicaoMouse = Input.mousePosition;
        posicaoMouse.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(posicaoMouse);
    }

    #endregion

    #region FUNÇÕES DE COMBATE

    /// <summary>
    /// Aplica dano a um alvo
    /// </summary>
    /// <param name="alvo">GameObject alvo</param>
    /// <param name="dano">Quantidade de dano</param>
    public void AplicarDano(GameObject alvo, int dano)
    {
        Vida vidaAlvo = alvo.GetComponent<Vida>();
        if (vidaAlvo != null)
        {
            vidaAlvo.ReceberDano(dano);
        }
    }

    /// <summary>
    /// Instancia um projétil
    /// </summary>
    /// <param name="prefabProjetil">Prefab do projétil</param>
    /// <param name="pontoDisparo">Ponto de origem</param>
    /// <param name="direcao">Direção do disparo</param>
    /// <param name="velocidade">Velocidade do projétil</param>
    public void AtirarProjetil(GameObject prefabProjetil, Transform pontoDisparo, Vector2 direcao, float velocidade)
    {
        GameObject projetil = Instantiate(prefabProjetil, pontoDisparo.position, Quaternion.identity);
        Rigidbody2D corpoRigido = projetil.GetComponent<Rigidbody2D>();

        if (corpoRigido != null)
        {
            corpoRigido.linearVelocity = direcao.normalized * velocidade;
        }
    }

    #endregion

    #region FUNÇÕES UTILITÁRIAS

    /// <summary>
    /// Destroi o objeto após um atraso
    /// </summary>
    /// <param name="atraso">Tempo em segundos</param>
    public void DestruirAposAtraso(float atraso)
    {
        Destroy(gameObject, atraso);
    }

    /// <summary>
    /// Instancia um efeito de partícula
    /// </summary>
    /// <param name="efeitoParticula">Prefab do efeito</param>
    /// <param name="posicao">Posição de spawn</param>
    /// <param name="atrasoDestruir">Tempo para destruir o efeito</param>
    public void InstanciarEfeitoParticula(GameObject efeitoParticula, Vector2 posicao, float atrasoDestruir = 2f)
    {
        GameObject efeito = Instantiate(efeitoParticula, posicao, Quaternion.identity);
        Destroy(efeito, atrasoDestruir);
    }

    /// <summary>
    /// Toca um som específico
    /// </summary>
    /// <param name="fonteAudio">AudioSource</param>
    /// <param name="clip">Clip de áudio</param>
    public void TocarSom(AudioSource fonteAudio, AudioClip clip)
    {
        if (fonteAudio != null && clip != null)
        {
            fonteAudio.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Faz o objeto piscar (útil para indicar dano)
    /// </summary>
    /// <param name="renderizadorSprite">SpriteRenderer do objeto</param>
    /// <param name="quantidadePiscadas">Número de piscadas</param>
    /// <param name="duracaoPiscada">Duração de cada piscada</param>
    public IEnumerator PiscarSprite(SpriteRenderer renderizadorSprite, int quantidadePiscadas, float duracaoPiscada)
    {
        Color corOriginal = renderizadorSprite.color;

        for (int i = 0; i < quantidadePiscadas; i++)
        {
            renderizadorSprite.color = Color.red;
            yield return new WaitForSeconds(duracaoPiscada);
            renderizadorSprite.color = corOriginal;
            yield return new WaitForSeconds(duracaoPiscada);
        }
    }

    /// <summary>
    /// Limita o objeto dentro dos limites da câmera
    /// </summary>
    /// <param name="camera">Câmera de referência</param>
    /// <param name="tamanhoObjeto">Tamanho do objeto (opcional)</param>
    public void ManterDentroLimitesCamera(Camera camera, float tamanhoObjeto = 0f)
    {
        Vector3 posicaoViewport = camera.WorldToViewportPoint(transform.position);

        posicaoViewport.x = Mathf.Clamp(posicaoViewport.x, tamanhoObjeto, 1 - tamanhoObjeto);
        posicaoViewport.y = Mathf.Clamp(posicaoViewport.y, tamanhoObjeto, 1 - tamanhoObjeto);

        transform.position = camera.ViewportToWorldPoint(posicaoViewport);
    }

    #endregion

    #region FUNÇÕES DE GERENCIAMENTO DE JOGO

    /// <summary>
    /// Carrega uma cena
    /// </summary>
    /// <param name="nomeCena">Nome da cena</param>
    public void CarregarCena(string nomeCena)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nomeCena);
    }

    /// <summary>
    /// Reinicia a cena atual
    /// </summary>
    public void ReiniciarCena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Sai do jogo
    /// </summary>
    public void SairDoJogo()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    #endregion

    #region MANIPULADORES DE EVENTOS (para serem sobrescritos)

    protected virtual void ManipularColisaoInimigo()
    {
        // Implementação base - sobrescreva em classes derivadas
        Debug.Log("Colidiu com inimigo");
    }

    protected virtual void ManipularColisaoColetavel(GameObject coletavel)
    {
        // Implementação base
        Destroy(coletavel);
        Debug.Log("Coletou item");
    }

    protected virtual void ManipularMorte()
    {
        // Implementação base
        Debug.Log("Player morreu");
        ReiniciarCena();
    }

    protected virtual void ManipularPontoVerificacao(Vector2 posicaoPontoVerificacao)
    {
        // Implementação base
        Debug.Log("Ponto de verificação alcançado: " + posicaoPontoVerificacao);
    }

    #endregion
}

// Classe auxiliar exemplo para sistema de vida
[System.Serializable]
public class Vida
{
    public int vidaMaxima = 100;
    public int vidaAtual;

    public Vida(int vidaMaxima)
    {
        this.vidaMaxima = vidaMaxima;
        vidaAtual = vidaMaxima;
    }

    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    public void Curar(int quantidade)
    {
        vidaAtual += quantidade;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);
    }

    private void Morrer()
    {
        Debug.Log("Entidade morreu");
        // Lógica de morte aqui
    }
}