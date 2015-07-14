using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreenMediator : MonoBehaviour {

    [SerializeField] Text _statusMessage;
    [SerializeField] Text _scoreMessage;
    [SerializeField] Transform _sendPanel;
    [SerializeField] Text _dontSendMessage;
    [SerializeField] Text _loadingMessage;

    void Awake()
    {
    }

    public void init(bool win, string score)
    {
        gameObject.SetActive(true);
        
        if (win) {
            _statusMessage.text = "YOU WIN!!";
        } else {
            _statusMessage.text = "You loose.";
        }
        _scoreMessage.text = score;
        
        _loadingMessage.gameObject.SetActive(true);
        StartCoroutine(loadCheck());
    }
    
    IEnumerator loadCheck()
    {
        while(true) {
            yield return new WaitForSeconds(0.5f);
            _loadingMessage.enabled = !_loadingMessage.enabled;
        }
    }
    
    public void onPlayAgainClick()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }
}
