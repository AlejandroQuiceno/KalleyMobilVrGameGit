using UnityEngine;


namespace ArmnomadsGames
{
	public class LaserCollider : MonoBehaviour
	{
		public delegate void CollisionDelegate(Collider coll);

		public event CollisionDelegate OnCollEnter;
		public event CollisionDelegate OnCollStay;
		public event CollisionDelegate OnCollExit;

		private new Collider collider;

		public bool Enable
		{
			get
			{
				if (collider == null)
					collider = GetComponent<Collider>();

				return collider.enabled;
			}
			set
			{
				if (collider == null)
					collider = GetComponent<Collider>();

				collider.enabled = value;
			}
		}


		void Awake()
		{
			gameObject.layer = LayerMask.NameToLayer("LaserSword");
		}

		void OnTriggerEnter(Collider coll)
		{
			OnCollEnter?.Invoke(coll);
		}

		void OnTriggerStay(Collider coll)
		{
			OnCollStay?.Invoke(coll);
		}

		void OnTriggerExit(Collider coll)
		{
			OnCollExit?.Invoke(coll);
		}
	}
}