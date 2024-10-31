
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInteract : MonoBehaviour
{
    public void Mouseclick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
