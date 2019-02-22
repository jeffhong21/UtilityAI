using UnityEngine;
using System.Collections;

namespace Hivemind {

	public class NavMeshActions : ActionLibrary {
		
		[Hivemind.Action]
		[Hivemind.Expects("gameObject", typeof(GameObject))]
		public Hivemind.Status MoveToGameObject(string animationFloat, float animationFactor) {
			GameObject go = context.Get<GameObject> ("gameObject");
			UnityEngine.AI.NavMeshAgent navMeshAgent = agent.GetComponent<UnityEngine.AI.NavMeshAgent>();
			Animator anim = agent.GetComponent<Animator>();
			UnityEngine.AI.NavMeshHit sampledDestination;
			UnityEngine.AI.NavMesh.SamplePosition(go.transform.position, out sampledDestination, 3f, 1);
			float distance = Vector3.Distance (agent.transform.position, sampledDestination.position);
			Debug.DrawRay (sampledDestination.position, Vector3.up, Color.green);

			// Planning path
			if (navMeshAgent.pathPending) {
				return Status.Running;
			}

			// Moving
			else if (distance > navMeshAgent.stoppingDistance) {
				navMeshAgent.SetDestination(sampledDestination.position);
				if (animationFloat != null) anim.SetFloat (animationFloat, animationFactor);
				return Status.Running;
			}
			
			// Reached destination
			else if (distance <= navMeshAgent.stoppingDistance) {
				if (animationFloat != null) anim.SetFloat (animationFloat, 0f);
				return Status.Success;
			}

			else {
				return Status.Error;
			}
		}

		[Hivemind.Action]
		[Hivemind.Expects("position", typeof(Vector3))]
		public Hivemind.Status MoveToPosition(string animationFloat, float animationFactor) {
			
			UnityEngine.AI.NavMeshAgent navMeshAgent = agent.GetComponent<UnityEngine.AI.NavMeshAgent>();
			Animator anim = agent.GetComponent<Animator>();
			UnityEngine.AI.NavMeshHit sampledDestination;
			Vector3 position = context.Get<Vector3>("position");
			UnityEngine.AI.NavMesh.SamplePosition(position, out sampledDestination, 10f, 1);
			float distance = Vector3.Distance (agent.transform.position, sampledDestination.position);

			Debug.DrawLine(agent.transform.position, sampledDestination.position, Color.green);
			
			// Planning path
			if (navMeshAgent.pathPending) {
				return Status.Running;
			}

			// Moving towards destination
			else if (distance > navMeshAgent.stoppingDistance) {
				navMeshAgent.SetDestination(sampledDestination.position);
				if (animationFloat != null) anim.SetFloat (animationFloat, animationFactor);
				return Status.Running;
			}
			
			// Reached destination
			else if (distance <= navMeshAgent.stoppingDistance) {
				if (animationFloat != null) anim.SetFloat (animationFloat, 0f);
				return Status.Success;
			}
			
			// Can't reach destination
			else if (navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) {
				return Status.Failure;
			}
			
			// Set destination
			else {
				return Status.Error;
			}
		}

		[Hivemind.Action]
		[Hivemind.Outputs("position", typeof(Vector3))]
		public Hivemind.Status GetRandomPosition(float radius) {
			Vector2 r = Random.insideUnitCircle;
			Vector3 position = agent.transform.position + new Vector3(r.x, 0f, r.y) * radius;
			UnityEngine.AI.NavMeshHit navMeshHit;
			bool sampleSuccessful = UnityEngine.AI.NavMesh.SamplePosition(position, out navMeshHit, 5f, 1);
			if (sampleSuccessful) {
				context.Set<Vector3>("position", navMeshHit.position);
				return Status.Success;
			} else {
				return Status.Failure;
			}

		}

	}
	

}