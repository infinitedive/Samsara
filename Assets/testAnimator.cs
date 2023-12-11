using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAnimator : MonoBehaviour
{

    Animator animator;
    public float a;
    public float b;
    public float _time;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (a > 0f) {
            animator.Play("0001_WalkForward", 0, 0f);
        }

        _time = (Time.deltaTime + _time) % 1f;
        
    }
}
