using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class ButtonInteract : MonoBehaviour
    {
        public void Mouseclick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}