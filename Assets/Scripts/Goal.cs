using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static bool goalMet = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Projectile"))
        {
            goalMet = true;
            var mat = GetComponent<Renderer>().material;
            var c = mat.color;
            c.a = 1; // set alpha channel of goal equal to 1
            mat.color = c;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
