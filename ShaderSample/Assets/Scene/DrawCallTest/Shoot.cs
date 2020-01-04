using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject go;

    // Update is called once per frame
    void Update()
    {
        var obj = Instantiate(go);
        var rigidbody = obj.GetComponent<Rigidbody>();
        var script = obj.GetComponent<materialize>();
        var col = new Color( Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        script.color = col;

        rigidbody.AddForce( Random.Range(-20.0f, 20.0f), Random.Range(3.0f, 50.0f), Random.Range(-20.0f, 20.0f), ForceMode.Impulse);
    }
}
