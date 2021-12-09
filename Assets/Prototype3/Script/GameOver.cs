using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text scoreText;
    public Button restartBtn;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        scoreText.text = RoleBulletController.score.ToString();
        restartBtn.onClick.AddListener(() =>
        {
            RoleBulletController.score = 0;
            UnityEngine.SceneManagement.SceneManager.LoadScene(7);
        });
    }
}
