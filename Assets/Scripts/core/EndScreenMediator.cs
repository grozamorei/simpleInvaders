using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        StartCoroutine(loadHighScoreTable());
    }
    
    IEnumerator loadCheck()
    {
        while(true) {
            yield return new WaitForSeconds(0.5f);
            _loadingMessage.enabled = !_loadingMessage.enabled;
        }
    }
    
    IEnumerator loadHighScoreTable()
    {
        Debug.Log("STARTED");
        
        WWWForm form = new WWWForm();
        
        var headers = new Hashtable();
        headers.Add("Methods", "POST");
        headers.Add("X-Parse-Application-Id", "VXuJqglLjoe90rmppH3y5e2tYkO1wlekOGg0UWkG");
        headers.Add("X-Parse-REST-API-Key", "r137sN6SYtRxrsyDZXZkWEOKiQnIJFrU83gHLbSr");
        headers.Add("Content-Type", "application/json");
//        var headers = form.headers;
//        headers["Method"] = "POST";
//        headers["X-Parse-Application-Id"] = "VXuJqglLjoe90rmppH3y5e2tYkO1wlekOGg0UWkG";
//        headers["X-Parse-REST-API-Key"] = "r137sN6SYtRxrsyDZXZkWEOKiQnIJFrU83gHLbSr";
//        headers["X-Parse-Revocable-Session"] = "1";
        headers["Content-Type"] = "application/json";
        
//        WWW www = new WWW("https://api.parse.com/1/login?username="+name+"&password="+password, null, headers);
        
        var wForm = new Hashtable();
        wForm.Add("name", "grozamorei");
        wForm.Add("score", Random.Range(10, 1000));
        
//    https://myAppID:javascript-key=myJavaScriptKey@api.parse.com
//        Bq6qxLHgZG8ZVrplLUdB8eMqtMhCCZVZneAxxLNA
        var d = new Dictionary<string, string>();
        d["Methods"] = "GET";
        d["X-Parse-Application-Id"] = "VXuJqglLjoe90rmppH3y5e2tYkO1wlekOGg0UWkG";
        d["X-Parse-REST-API-Key"] = "r137sN6SYtRxrsyDZXZkWEOKiQnIJFrU83gHLbSr";
        d["Content-Type"] = "application/json";
        
        var encoding = new System.Text.UTF8Encoding();
//        var dd = encoding.GetBytes(JsonConvert.SerializeObject(new {order="score", score=1}));
//        "'name':'some', 'score':500"
        var w = new WWW("https://api.parse.com/1/classes/Score", null, d);
        yield return w;
        
        var zz = JsonConvert.DeserializeObject(w.text);
        Debug.Log(zz);
//        Debug.Log(w.error + ";;" + w.text + ";;" + w.ToString());
    }
    
    public void onPlayAgainClick()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }
    
    static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }
}
