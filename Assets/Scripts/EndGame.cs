using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public void reStartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
