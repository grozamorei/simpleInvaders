using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using util;
using LitJson;

namespace social
{
    public class EndScreenMediator : MonoBehaviour {
        
        [SerializeField] Text _statusMessage = null;
        [SerializeField] Text _scoreMessage = null;
        [SerializeField] Transform _sendPanel = null;
        [SerializeField] Text _dontSendMessage = null;
        [SerializeField] Text _loadingMessage = null;

        [SerializeField] Text _nameInput = null;
        [SerializeField] Text _inputStatus = null;

        [SerializeField] Transform _highScorePanel = null;
        [SerializeField] ScoreUIBindings[] _highScoreData = null;


        private Dictionary<string, string> _getHeader;
        private Dictionary<string, string> _postHeader;

        private bool _loading = false;
        private int _currentScore;
        private string _currentName = null;
        private ScoreResultCollection _currentResults;

        void Awake()
        {
            _getHeader = new Dictionary<string, string>();
            _postHeader = new Dictionary<string, string>();
            _getHeader["Methods"] = "GET"; _postHeader["Methods"] = "POST";
            _getHeader["X-Parse-Application-Id"] = _postHeader["X-Parse-Application-Id"] = "VXuJqglLjoe90rmppH3y5e2tYkO1wlekOGg0UWkG";
            _getHeader["X-Parse-REST-API-Key"] = _postHeader["X-Parse-REST-API-Key"] = "r137sN6SYtRxrsyDZXZkWEOKiQnIJFrU83gHLbSr";
            _getHeader["Content-Type"] = _postHeader["Content-Type"] = "application/json";
        }
        
        public void init(bool win, int score, string strScore)
        {
            Debug.Log(score);
            _currentScore = score;
            gameObject.SetActive(true);
            
            if (win) {
                _statusMessage.text = "YOU  WIN!!";
            } else {
                _statusMessage.text = "You  lose";
            }
            _scoreMessage.text = strScore;
            
            _loadingMessage.gameObject.SetActive(true);
            StartCoroutine(loadCheck());
            StartCoroutine(loadHighScoreTable(true));
        }
        

        
        public void onPlayAgainClick()
        {
            Application.LoadLevel(Application.loadedLevelName);
        }

        public void onSubmitNameClick()
        {
            _inputStatus.text = "";
            if (_nameInput.text.Length < 3) {
                _inputStatus.text = "at least 3 characters length";
                return;
            }

            _currentName = _nameInput.text;
            _sendPanel.gameObject.SetActive(false);
            _loading = true;

            StartCoroutine(pushResults());
        }

        IEnumerator loadCheck()
        {
            while(true) {
                yield return new WaitForSeconds(0.3f);
                if (_loading)
                    _loadingMessage.enabled = !_loadingMessage.enabled;
            }
        }
        
        IEnumerator loadHighScoreTable(bool showPanelsAfter)
        {
            _loading = true;
            var w = new WWW("https://api.parse.com/1/classes/Score?order=-score&limit=10", null, _getHeader);
            yield return w;

            var answer = JsonMapper.ToObject<ScoreResultCollection>(w.text);
            _currentResults = answer;
            
            for (int i = 0; i < answer.results.Count; i++) {
                var name = answer.results[i].name;
                _highScoreData[i].name.text = name;
                
                var score = answer.results[i].score;
                _highScoreData[i].score.text = Score.formatToStr(score);

                if (name == _currentName && score == _currentScore) {
                    _highScoreData[i].name.color = new Color(0.8f, 0.8f, 0, 1f);
                    _highScoreData[i].score.color = new Color(0.8f, 0.8f, 0, 1f);
                } else {
                    _highScoreData[i].name.color = new Color(0.19f, 0.19f, 0.19f, 1f);
                    _highScoreData[i].score.color = new Color(0.19f, 0.19f, 0.19f, 1f);
                }
            }
            _highScorePanel.gameObject.SetActive(true);
            _loading = false;
            _loadingMessage.enabled = false;
            
            if (showPanelsAfter) {
                if (answer.results.Count == 0) {
                    _sendPanel.gameObject.SetActive(true);
                } else {
                    var minScore = answer.results[answer.results.Count-1].score;
                    if ((_currentScore > 0) && ((_currentScore <= minScore && answer.results.Count < 10) || _currentScore > minScore)) {
                        _sendPanel.gameObject.SetActive(true);
                    } else {
                        _dontSendMessage.gameObject.SetActive(true);
                    }
                }
            }
        }

        IEnumerator pushResults()
        {
            var enc = new System.Text.UTF8Encoding();
            var data = enc.GetBytes(JsonMapper.ToJson(new {name=_currentName, score=_currentScore}));
            var w = new WWW("https://api.parse.com/1/classes/Score", data, _postHeader);
            yield return w;
            StartCoroutine(loadHighScoreTable(false));
        }
    }
}