using UnityEngine;
using UnityEngine.Networking;

public static class MapInfoLoader
{
    public static int[] Load(int level)
    {
        string fileName = Map.FilePath + level.ToString() + "nfo";
        UnityWebRequest www = UnityWebRequest.Get(fileName);
        www.SendWebRequest();

        while (www.isDone == false) { }

        string stream = www.downloadHandler.text;

        char[] bytes = stream.ToCharArray();

        int[] tanksCount = new int[4];

        for (int i = 0; i < 4; i++)
        {
            tanksCount[i] = (byte)bytes[i];
        }

        return tanksCount;
    }
}
