using UnityEngine;
using System.Collections;

namespace social
{
    public class ScoreResultCollection 
    {
        public ScoreResult[] results;
    }
    
    public class ScoreResult 
    {
        public string objectId;
        public string name;
        public int score;
        
        public string createdAt;
        public string updatedAt;

        public override string ToString ()
        {
            return string.Format ("[ScoreResult] objId: {0}; name: {1}; score: {2}", objectId, name, score);
        }
    }
}