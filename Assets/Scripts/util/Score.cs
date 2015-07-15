using UnityEngine;
using System.Collections;

namespace util
{
    public class Score : MonoBehaviour
    {
        public static string formatToStr(int score)
        {
            if (score == 0) {
                return "0000";
            } else if (score < 100) {
                return "00" + score.ToString();
            } else if (score < 1000) {
                return "0" + score.ToString();
            }
            return score.ToString();
        }
    }
}