using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public BoolData simulate;

    static World instance;

    // Update is called once per frame
    void Update()
    {
        if (simulate.value == false) return;

        float dt = Time.deltaTime;
    }
}
