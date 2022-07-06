using System.Collections.Generic;
using UnityEngine;

namespace GinputSystems.Examples
{
	[AddComponentMenu("Ginput/Examples/MultiplayerSpawner", 0)]
	public class MultiplayerSpawner : MonoBehaviour
	{
		public GameObject playerPrefab;

		private List<ShootyPlayer> players = new();

		private void Update()
		{
			InputDeviceSlot slot = Ginput.GetSlotPress("Join");

			if (slot != InputDeviceSlot.any)
			{
				//a player has pressed join!

				//first we check if this player has already joined
				bool alreadyJoined = false;
				for (int i = 0; i < players.Count; i++)
				{
					if (players[i].playerSlot == slot)
					{
						alreadyJoined = true;
						//lets assume this player is trying to unjoin, and remove them :)
						Destroy(players[i].gameObject);
						players.RemoveAt(i);
						i--;
					}
				}

				if (!alreadyJoined)
				{
					//this is a new player looking to join, so lets let them!
					GameObject newPlayer = Instantiate(playerPrefab);
					newPlayer.transform.position = new Vector3(Random.Range(-4f, 4f), 3f, Random.Range(-4f, 4f));
					newPlayer.GetComponent<ShootyPlayer>().playerSlot = slot;
					players.Add(newPlayer.GetComponent<ShootyPlayer>());

					//lets prevent any new inputs from this slot for a few frames
					//This isn't necessary, but will prevent people accidentally pressing join twice quickly :)
					Ginput.ResetInputs(slot);
				}
			}
		}
	}
}
