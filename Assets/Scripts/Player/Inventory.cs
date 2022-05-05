using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Rect rect;
    Texture texture;
    public int key;
    // Start is called before the first frame update
    void Start()
    {
        float size = Screen.width * 0.1f;

        rect = new Rect(Screen.width / 2, Screen.height * 0.7f, size, size);
        texture = Resources.Load("Textires/key") as Texture; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
