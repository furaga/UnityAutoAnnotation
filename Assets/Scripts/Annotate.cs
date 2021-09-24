using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annotate : MonoBehaviour
{
    public bool SaveAnnotations = true;
    public bool SaveImages = true;
    public string OutputDirectory = "output/";
    Texture2D texture_;
    public string Prefix = "";
    public int counter_ = 0;
    [System.Serializable]
    public class Joint
    {
        public string Name;
        public GameObject Target;
        public Joint(string name)
        {
            this.Name = name;
        }
    }

    public Joint[] Joints = new[]
    {
        new Joint("left_shoulder"),
        new Joint("right_shoulder"),
        new Joint("left_elbow"),
        new Joint("right_elbow"),
        new Joint("left_wrist"),
        new Joint("right_wrist"),
        new Joint("left_hip"),
        new Joint("right_hip"),
        new Joint("left_knee"),
        new Joint("right_knee"),
        new Joint("left_ankle"),
        new Joint("right_ankle"),
        new Joint("head"),
        new Joint("neck"),
    };

    // Start is called before the first frame update
    void Start()
    {
        counter_ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 1frame目は撮影うまくいかないのでスキップする
        if (counter_ <= 0)
        {
            counter_++;
            return;
        }

        if (SaveAnnotations)
        {
            savePose();
        }
        if (SaveImages)
        {
            saveCameraImage();
        }
        counter_++;
    }

    void createDirectoryOf(string filepath)
    {
        string dir = System.IO.Path.GetDirectoryName(filepath);
        if (false == System.IO.Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
        }
    }

    void savePose()
    {

    }

    void saveCameraImage()
    {
        var camera = gameObject.GetComponent<Camera>();
        if (texture_ == null && camera.targetTexture != null)
        {
            texture_ = new Texture2D(
                camera.targetTexture.width,
                camera.targetTexture.height,
                TextureFormat.ARGB32, false);
        }

        if (texture_ == null)
        {
            return;
        }

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;
        camera.Render();
        texture_.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        texture_.Apply();

        RenderTexture.active = currentRT;
        byte[] png = texture_.EncodeToPNG();
        string filename = string.Format("{0}_{1:00000}.png", Prefix, counter_);
        string filepath = System.IO.Path.Combine(OutputDirectory, filename);
        createDirectoryOf(filepath);
        System.IO.File.WriteAllBytes(filepath, png);
    }
}
