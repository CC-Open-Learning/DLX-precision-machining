/*
 * Controls the presentation of Windows and the Buttons which call them into view. 
 * 
 * Responsibilities of each individual submenu are left to them; this class manages 
 * the aggregation of these Windows and their Buttons
 *		
 */

using System.Collections.Generic;
using UnityEngine;

namespace RemoteEducation.UI
{

    /// <summary>
    ///     Manages multiple <see cref="Window"/> objects to present UI windows to a user
    /// </summary>
    public class WindowManager : MonoBehaviour
    {

		[Header("UI Settings")]
		public RectTransform ButtonGroup;
		public RectTransform WindowGroup;


		/// <summary>This stores all active and enabled WindowCloser instances.</summary>
		public static List<WindowManager> Instances = new();

		/// <summary>If every window is closed and you press the close key, this window will be opened. This can be used to open an options menu.</summary>
		public Window EmptyWindow { set { emptyWindow = value; } get { return emptyWindow; } }
		[SerializeField] private Window emptyWindow;

		/// <summary>This allows you to set the key that must be pressed to close the window on top.</summary>
		public KeyCode CloseKey { set { closeKey = value; } get { return closeKey; } }
		[SerializeField] private KeyCode closeKey = KeyCode.Escape;


		/// <summary>This stores a list of all opened windows, in order of opening, so they can be closed in reverse order.</summary>
		public List<Window> WindowOrder { get { if (windowOrder == null) windowOrder = new List<Window>(); return windowOrder; } }
		[SerializeField] private List<Window> windowOrder;


		protected virtual void OnEnable()
		{
			// Button bar is visible if it contains buttons
			UpdateButtonGroupVisibility();

			Instances.Add(this);
		}

		protected virtual void OnDisable()
		{
			Instances.Remove(this);
		}

		public static void Register(Window window)
		{
			if (Instances.Count > 0 && window != null)
			{
				Instances[0].RegisterNow(window);
			}
		}

		/// <summary>This allows you to close all open Windows.</summary>
		[ContextMenu("Close All")]
		public void CloseAll()
		{
			for (var i = WindowOrder.Count - 1; i >= 0; i--) // NOTE: Property
			{
				var window = windowOrder[i];

				if (window != null && window.On == true)
				{
					window.TurnOff();
				}
			}

			windowOrder.Clear();
		}

		/// <summary>This allows you to close the top most Window.</summary>
		[ContextMenu("Close Top Most")]
		public void CloseTopMost()
		{
			for (var i = WindowOrder.Count - 1; i >= 0; i--) // NOTE: Property
			{
				var window = windowOrder[i];

				// Only 'Closeable' windows should be closed
				if (window != null && !window.Closeable)
				{
					return;
				}

				windowOrder.RemoveAt(i);

				if (window != null && window.On == true)
				{
					window.TurnOff();

					return;
				}
			}

			if (emptyWindow != null)
			{
				emptyWindow.TurnOn();
			}
		}



		protected virtual void Update()
		{
			if (this == Instances[0])
			{
				if (Input.GetKeyDown(CloseKey) == true)
				{
					CloseTopMost();
				}
			}
		}

		private void RegisterNow(Window window)
		{
			WindowOrder.Remove(window); // NOTE: Property

			windowOrder.Add(window);
		}

		public void UpdateButtonGroupVisibility()
        {
			SetButtonGroupVisible(ButtonGroup.childCount > 0);
		}


		/// <summary>
		///     Set the visibility of the WindowManager button group
		/// </summary>
		/// <param name="visible"></param>
		public void SetButtonGroupVisible(bool visible)
        {
            ButtonGroup.gameObject.SetActive(visible);
        }
    }
}
