using UnityEngine;

namespace GinputSystems.Examples
{
	[AddComponentMenu("Ginput/Examples/ShootyPlayer", 0)]
	public class ShootyPlayer : MonoBehaviour
	{
		//which input device controls this player
		public InputDeviceSlot playerSlot = InputDeviceSlot.any;

		//lets display which input slot we are using, just for kicks
		public TextMesh playerSlotDisplay;

		//stuff we need for our platforming code
		private CharacterController characterController;
		private float yMotion = 0f;

		public Renderer[] playerRenderers;

		private Vector3 lookDirection = Vector3.forward;

		public GameObject bulletPrefab;
		private float bulletCooldown = 0f;
		public Transform gunTransform;

		private void Start()
		{
			characterController = transform.GetComponent<CharacterController>();
			//set the player a random colour
			Color playerColor = new(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 1f);
			for (int i = 0; i < playerRenderers.Length; i++)
			{
				playerRenderers[i].material.color = playerColor;
			}
			playerSlotDisplay.text = "Input:\n" + playerSlot.ToString();
		}

		private void Update()
		{
			//get player input for motion
			Vector3 motionInput = Ginput.GetVector("Horizontal", "", "Vertical", playerSlot);

			//we want to move like, three times as much as this
			motionInput *= 3f;

			//gravity
			yMotion -= Time.deltaTime * 10f;
			motionInput.y = yMotion;

			//move our character controller now
			characterController.Move(motionInput * Time.deltaTime);

			//landing/jumping
			if (characterController.isGrounded)
			{
				yMotion = -0.05f;

				if (Ginput.GetButtonDown("Jump", playerSlot))
				{
					//we pressed jump while on the ground, so we jump!
					yMotion = 5f;
				}
			}

			//aiming
			Vector3 aimDir = Vector3.zero;
			aimDir.x = Ginput.GetAxisRaw("Horizontal", playerSlot);
			aimDir.z = Ginput.GetAxisRaw("Vertical", playerSlot);
			if (aimDir.magnitude > 0.4f)
			{
				//inputs are strong enough, lets look in the aim direction
				lookDirection = aimDir.normalized;
				Quaternion fromRotation = transform.rotation;
				transform.LookAt(transform.position + lookDirection);
				transform.rotation = Quaternion.Slerp(fromRotation, transform.rotation, Time.deltaTime * 10f);
			}
			//make sure our display text always faces the same way
			playerSlotDisplay.transform.eulerAngles = Vector3.zero;

			//shooting
			bulletCooldown -= Time.deltaTime;
			if (Ginput.GetButton("Fire1", playerSlot) && bulletCooldown <= 0f)
			{
				bulletCooldown = 0.2f;
				GameObject newBullet = Instantiate(bulletPrefab);
				newBullet.transform.SetPositionAndRotation(gunTransform.position, gunTransform.rotation);
				newBullet.GetComponent<BulletScript>().moveDir = gunTransform.forward;
			}
			if (Ginput.GetButton("Fire2", playerSlot) && bulletCooldown <= 0f)
			{
				bulletCooldown = 0.05f;
				GameObject newBullet = Instantiate(bulletPrefab);
				newBullet.transform.SetPositionAndRotation(gunTransform.position, gunTransform.rotation);
				newBullet.transform.localScale = Vector3.one * 0.3f;
				newBullet.GetComponent<BulletScript>().moveDir = gunTransform.forward;
				newBullet.GetComponent<BulletScript>().moveSpeed = 30f;
				newBullet.GetComponent<BulletScript>().life = 0.33f;
			}
		}
	}
}