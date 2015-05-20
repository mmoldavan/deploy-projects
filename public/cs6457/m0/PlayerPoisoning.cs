using UnityEngine;
using UnityEngine.UI;

public class PlayerPoisoning : MonoBehaviour
{
	public int damagePerShot = 100;
	public float timeBetweenPoisons = 5f;
	public Slider poisonSlider;

	float exploisonTimer;

	ParticleSystem poisonParticles;
	Light playerLight;
	float effectsDisplayTime = 0.2f;

	Ray poisonRay;
	RaycastHit[] collisions;
	
	
	void Awake ()
	{
		poisonParticles = GetComponent<ParticleSystem> ();
		playerLight = GetComponent<Light> ();
	}
	
	
	void Update ()
	{
		exploisonTimer += Time.deltaTime;
		
		if(Input.GetButton ("Fire2") && exploisonTimer >= timeBetweenPoisons && Time.timeScale != 0)
		{
			Poison ();
		}

		if(exploisonTimer >= timeBetweenPoisons * effectsDisplayTime)
		{
			DisableEffects ();
		}

		poisonSlider.value = Mathf.Min (exploisonTimer, timeBetweenPoisons) / timeBetweenPoisons;
	}
	
	
	public void DisableEffects ()
	{
		playerLight.enabled = false;
	}
	
	
	void Poison ()
	{
		exploisonTimer = 0f;
		
		playerLight.enabled = true;
		
		poisonParticles.Stop ();
		poisonParticles.Play ();

		poisonRay.origin = transform.position;
		poisonRay.direction = transform.forward;
		
		collisions = Physics.SphereCastAll (poisonRay, 5);
		foreach (RaycastHit hit in collisions)
		{
			EnemyHealth enemyHealth = hit.collider.GetComponent <EnemyHealth> ();
			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage (damagePerShot, hit.point);
			}
		}
	}
}
