using UnityEngine;

public static class FilesExtensions
{
    public static string SaveRTToFile(this RenderTexture rt, string name)
    {
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes;
        bytes = tex.EncodeToPNG();

        string path = Application.persistentDataPath + "/" + name + ".png";
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log("Saved to " + path);

        return path;
    }

    public static void ShowInExplorer(string itemPath)
    {
        itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
        System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
    }

}
