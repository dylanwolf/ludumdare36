using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;

public class LevelLoaderTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

        var levels = LevelReader.Load("RetrofutureLevels");
        Debug.Log(levels.Length);
        for(int i = 0; i < levels.Length; i++)
        {
            Debug.Log(string.Format("{0}: {1}", i, levels[i].Name));


            StringBuilder board = new StringBuilder();
            if (levels[i].Board != null)
            {
                for (int j = 0; j < levels[i].Board.GetLength(0); j++)
                {
                    if (board.Length > 0)
                        board.Append("\n");
                    for (int k = 0; k < levels[i].Board.GetLength(1); k++)
                    {
                        board.AppendFormat("{0}||", levels[i].Board[j,k]);
                    }
                }
            }
            else
            {
                board.Append("(null)");
            }
            Debug.Log(string.Format("{0}: {1}", i, board.ToString()));

            StringBuilder parts = new StringBuilder();
            foreach (var kvp in levels[i].Parts)
            {
                parts.AppendFormat("{0}={1}, ", kvp.Key, kvp.Value);
            }
            Debug.Log(string.Format("{0}: {1}", i, parts));

            Debug.Log(string.Format("{0}: {1}", i, levels[i].Switches));
        }
	}

}
