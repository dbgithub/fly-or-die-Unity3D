using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// More info about TimeScale and ParticleSystems at: http://answers.unity3d.com/questions/774083/i-want-a-timescale-to-affect-on-everything-except.html
public class TimeScaleIndependentParticleSystem : MonoBehaviour {

	private ParticleSystem pSystem;
 
     public void Awake()
     {
         pSystem = gameObject.GetComponent<ParticleSystem>();
     }
 
     public void Play(){
         pSystem.Simulate(Time.unscaledDeltaTime,true,true);
     }
 
	 // Update is called once per frame
     public void Update()
     {
         pSystem.Simulate(Time.unscaledDeltaTime,true,false);
     }
	
}
