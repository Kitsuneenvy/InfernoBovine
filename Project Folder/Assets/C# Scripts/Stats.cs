using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;



public enum auraInfluence {none = 0, lawful, holy, sinister, magical}
public enum damageType {none = 0, Physical, Magical, Holy, Unholy, Builder}

[System.Serializable]
public class Stats : MonoBehaviour {

	public int cost = 10;
	public List<GameObject> createdObjects = new List<GameObject>();
	public bool structure = false;
	public bool finishedBuilding = false;
	public bool worker = false;

	[DontSaveMember]
	public bool moving = false;
	bool startmove = false;
	public float repathDelay = 0.25f;
	float timeSinceLastRepath = 9999f;

	public bool attackSet = false;
	public float speed = 1;
	bool refreshPath = false;
	Vector2 targetLocation = Vector2.zero;
	Vector3 originalPosition = Vector3.zero;
	Vector3 nextWaypointPosition = Vector3.zero;
	int currentWaypoint = 0;

	//Taken from Design Doc section 4.1
	public string objectName;
	public string tooltip;
	public int team = 1;

	public float maxHealth;
	public float health;
	public int armour;

	public float attackSpeed;
	float timeSinceLastAttack;
	public float attackDamage;
	public damageType dmgType;
	//Enum for damage type
	public float attackRange;

	float productionSpeed;

	public List<AudioClip> audioClips = new List<AudioClip>();

	public GameObject attackTarget = null;
	Transform attackTransform = null;

	[DontSaveMember]
	Path currentPath;
	[DontSaveMember]
	Seeker seekerComponent;

	public int auraInf = 0;

	// Use this for initialization
	void Start () {
		if(this.tag=="Unit"){
			seekerComponent = this.GetComponent<Seeker>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(health<=0){
			//replace with play death anim later
			Destroy(this.gameObject,0.01f);
		}
		if(finishedBuilding == false){
			if(health == maxHealth){
				finishedBuilding = true;
			}
		}
		if(attackTarget!=null){
			if(timeSinceLastRepath>repathDelay){
				startmove = true;
				timeSinceLastRepath = 0;
			}
			timeSinceLastRepath+= Time.deltaTime;
			if(Vector3.Distance(this.transform.position,attackTarget.transform.position)<attackRange&&timeSinceLastAttack>attackSpeed){
				SendHit();
				timeSinceLastAttack = 0;
			}
			timeSinceLastAttack+= Time.deltaTime;
		}

		if(startmove == true){
			if(attackTarget!=null){
				attackTransform = attackTarget.transform;
				seekerComponent.StartPath(this.transform.position,attackTransform.position,OnPathComplete);
			} else {
				seekerComponent.StartPath(this.transform.position,targetLocation,OnPathComplete);
			}
			startmove = false;
		}

		if(moving == true){
			if(currentPath!=null){
				if(Vector3.Distance(this.transform.position,currentPath.vectorPath[currentWaypoint])>0.15f){
					this.transform.position = Vector3.MoveTowards(this.transform.position,currentPath.vectorPath[currentWaypoint],speed*Time.deltaTime);
				} else {
					if(currentWaypoint<currentPath.vectorPath.Count-1){
						currentWaypoint++;
					} else {
						//finished the path
						moving = false;
					}
				}
			}
		}




//		if(this.tag=="Unit"){
//			if(attackTarget!=null&&startmove==true){
//				this.GetComponent<AIPath>().SearchPath();
//				//seekerComponent.StartPath(transform.position,new Vector2(GetComponent<AIPath>().target.position.x,GetComponent<AIPath>().target.position.y), OnPathComplete);
//				startmove = false;
//			} else if(startmove==true){
//				seekerComponent.StartPath(transform.position,targetLocation, OnPathComplete);
//				startmove = false;
//			}
//			if(moving==true){
//				if(currentPath!=null){
//					transform.position = Vector3.MoveTowards(transform.position,nextWaypointPosition,speed*Time.deltaTime);
//					if(Vector2.Distance(new Vector2(transform.position.x,transform.position.y),nextWaypointPosition)<0.05f){
//						if(currentWaypoint == currentPath.vectorPath.Count-1){
//							moving =false;
//							return;
//						} else {
//							currentWaypoint++;
//							nextWaypointPosition = currentPath.vectorPath[currentWaypoint];
//						}
//					} 
//				}
//			}
//		}
	}

	public void startMove(Vector2 newPosition){
		targetLocation = newPosition;
		startmove = true;
	}

	public void startMove(){
		startmove = true;
	}

	void OnPathComplete(Path p){
		if(!p.error){
			currentWaypoint = 0;
			currentPath = p;
			moving=true;
			nextWaypointPosition = p.vectorPath[currentWaypoint];
		}
	}

	void SendHit(){
		//Delay moving until animation is complete
		Stats targetStats = attackTarget.GetComponent<Stats>();
		if(dmgType == damageType.Builder){
			if(targetStats.structure == true){
				if(targetStats.health<targetStats.maxHealth){
					targetStats.health += attackDamage;
				} else {
					attackTarget = null;
				}
				return;
			}
		}
		if(attackDamage > targetStats.armour){
			targetStats.health -= attackDamage - targetStats.armour;
		} else {
			targetStats.health -= 1;
		}

	}

		
}
