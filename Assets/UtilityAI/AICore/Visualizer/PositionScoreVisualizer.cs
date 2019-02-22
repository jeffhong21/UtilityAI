namespace AtlasAI
{
    using UnityEngine;
    using System.Collections.Generic;

    using Visualization;

    using Bang;

    public class PositionScoreVisualizer : ActionWithOptionsVisualizerComponent<MoveToBestPosition, Vector3>
    {
        [Range(0.1f, 2f)]
        public float sphereSize = 0.25f;
        [Range(0.01f, 0.99f)]
        public float sphereAlpha = 0.5f;


		//  Cached variables.
		//private Camera cam;
        //private GUIContent content; // = new GUIContent();
        //private GUIStyle label; // = new GUIStyle(GUI.skin.label);




		protected override List<Vector3> GetOptions(IAIContext context)
        {
            AgentContext c = context as AgentContext;
            return c.sampledPositions;
        }


        protected override void DrawGUI(List<ScoredOption<Vector3>> data)
        {
            var cam = Camera.main;
            if (cam == null)
                return;


            for (int i = 0; i < data.Count; i++)
            {
                var score = data[i].score;

                var pos = cam.WorldToScreenPoint(data[i].option);
                pos.y = Screen.height - pos.y;


                if (score < 0f)
                {
                    GUI.color = Color.red;
                }
                else if (score == 0f)
                {
                    GUI.color = Color.black;
                }
                else
                {
                    GUI.color = Color.green;
                }


                var content = new GUIContent(score.ToString("F0"));
                var size = new GUIStyle(GUI.skin.label).CalcSize(content);
                var rect = new Rect(pos.x, pos.y, size.x, size.y);
                //GUI.Box(rect, content);
                GUI.Label(rect, content);
            }

        }

        protected override void DrawGizmos(List<ScoredOption<Vector3>> data)
        {
            
            float maxScore = 0f;
            float minScore = Mathf.Infinity;

            for (int i = 0; i < data.Count; i++)
            {
                var value = data[i].score;
                if (value > maxScore)
                {
                    maxScore = value;
                }

                if (value < minScore)
                {
                    minScore = value;
                }
            }

            var diffScore = maxScore - minScore;

            for (int i = 0; i < data.Count; i++)
            {
                var pos = data[i].option;
                var score = data[i].score;

                var normScore = score - minScore;

                Gizmos.color = GetColor(normScore, diffScore, sphereAlpha);
                Gizmos.DrawSphere(pos, sphereSize);
            }


        }



        private static Color GetColor(float score, float maxScore, float alpha = 1f)
        {
            if (maxScore <= 0)
            {
                return Color.green;
            }

            if (score == maxScore)
            {
                return Color.cyan;
            }

            var quotient = score / maxScore;

            return new Color((1 - quotient), quotient, 0, alpha);
        }


    }
}
