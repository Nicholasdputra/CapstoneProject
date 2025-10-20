using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossScript : ClickableEntity
{
    public int timer;
    public int timeLimit;
    public Coroutine timerCoroutine;

    public override void HandleDestroy()
    {
        // 
    }

    public override void Initialize()
    {
        
    }

    public override void OnClick()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator TimerCoroutine()
    {
        while (timer < timeLimit)
        {
            yield return new WaitForSeconds(1f);
            timer++;
        }

        //Time's up, handle miniboss failure
        WaveManager.instance.HandleMinibossFailure();
    }
}
