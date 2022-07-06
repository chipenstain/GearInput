namespace UnityEngine.EventSystems
{
	[AddComponentMenu("Ginput/Events/StandaloneGinputModule", 0)]
	public class StandaloneGinputModule : StandaloneInputModule
	{
		public string m_GinputUpButton = "Up";
		public string m_GinputDownButton = "Down";
		public string m_GinputLeftButton = "Left";
		public string m_GinputRightButton = "Right";
		public string m_GinputSubmitButton = "Submit";
		public string m_GinputCancelButton = "Cancel";

		public override void Process()
		{
			bool usedEvent = SendUpdateEventToSelectedObject();

			if (eventSystem.sendNavigationEvents)
			{
				if (!usedEvent)
					usedEvent |= SendMoveEventToSelectedObject();

				if (!usedEvent)
				{
					//SendSubmitEventToSelectedObject();
					if (SendSubmitEventToSelectedObject()) Ginput.ResetInputs();
				}
			}
			ProcessMouseEvent();
		}

		public override bool ShouldActivateModule()
		{
			//Debug.Log("happens");
			if (!base.ShouldActivateModule())
				return false;

			var shouldActivate = Ginput.GetButtonDown(m_GinputSubmitButton);
			shouldActivate |= Ginput.GetButtonDown(m_GinputCancelButton);
			shouldActivate |= Ginput.GetButtonDownRepeating(m_GinputUpButton);
			shouldActivate |= Ginput.GetButtonDownRepeating(m_GinputDownButton);
			shouldActivate |= Ginput.GetButtonDownRepeating(m_GinputLeftButton);
			shouldActivate |= Ginput.GetButtonDownRepeating(m_GinputRightButton);

			shouldActivate |= (m_MousePos - m_LastMousePos).sqrMagnitude > 0.0f;
			shouldActivate |= Input.GetMouseButtonDown(0);
			return shouldActivate;
		}

		private new bool SendSubmitEventToSelectedObject()
		{
			if (eventSystem.currentSelectedGameObject == null)
				return false;

			var data = GetBaseEventData();
			if (Ginput.GetButtonDown(m_GinputSubmitButton))
				ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

			if (Ginput.GetButtonDown(m_GinputCancelButton))
				ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
			return data.used;
		}

		private new bool SendMoveEventToSelectedObject()
		{
			Vector2 movement = GetRawMoveVector();

			var axisEventData = GetAxisEventData(movement.x, movement.y, 0.4f);
			if (!Mathf.Approximately(axisEventData.moveVector.x, 0f) || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
			{
				ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			}

			return axisEventData.used;
		}

		private Vector2 GetRawMoveVector()
		{
			Vector2 move = Vector2.zero;
			if (Ginput.GetButtonDownRepeating(m_GinputUpButton)) move.y += 1f;
			if (Ginput.GetButtonDownRepeating(m_GinputDownButton)) move.y -= 1f;
			if (Ginput.GetButtonDownRepeating(m_GinputLeftButton)) move.x -= 1f;
			if (Ginput.GetButtonDownRepeating(m_GinputRightButton)) move.x += 1f;

			return move;
		}

		private Vector2 m_LastMousePos;
		private Vector2 m_MousePos;
		public override void UpdateModule()
		{
			m_LastMousePos = m_MousePos;
			m_MousePos = Input.mousePosition;
		}
	}
}