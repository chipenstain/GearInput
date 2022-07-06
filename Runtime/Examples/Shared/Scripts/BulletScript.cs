using UnityEngine;

namespace GinputSystems.Examples
{
	[AddComponentMenu("Ginput/Examples/BulletScript", 0)]
	public class BulletScript : MonoBehaviour
	{
		public Vector3 moveDir;
		public float moveSpeed;
		public float life = 1f;

		private void Update()
		{
			transform.position += moveDir * moveSpeed * Time.deltaTime;
			transform.LookAt(transform.position + moveDir);

			life -= Time.deltaTime;
			if (life < 0f) Destroy(gameObject);
		}
	}
}