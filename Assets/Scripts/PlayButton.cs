using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button button;

    public delegate void onButtonClick();

    public static event onButtonClick onClickEvents;

    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayGame);
    }

    private void PlayGame()
    {
        onClickEvents?.Invoke();
    }
}