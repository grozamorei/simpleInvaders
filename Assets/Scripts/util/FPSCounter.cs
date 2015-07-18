using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

    public Text fpsText;
    public float frameCount = 0f;
    public float dt = 0f;
    public float fps = 0f;
    public float updateRate = 4f;  // 4 updates per sec.
    
    void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0/updateRate)
        {
            fps = frameCount / dt ;
            frameCount = 0;
            dt -= 1.0f/updateRate;
            
            fpsText.text = (Mathf.Floor(fps * 100f)/100f).ToString();
        }
    }
}
