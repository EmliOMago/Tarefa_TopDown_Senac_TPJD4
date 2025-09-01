using UnityEngine;
using UnityEngine.SceneManagement;

public class Cardapio : MonoBehaviour
{
   public void Iniciar()
    {
        SceneManager.LoadScene("FaseInicial");
    }

    public void Creditos()
    {

    }

    public void Sair()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
