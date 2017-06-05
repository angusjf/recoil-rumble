using System.Collections;
using System.Collections.Generic;

namespace Ui {
	public class MenuScene : IEnumerable<Element> {

		List<Element> elements;
		Button currentButton;

		public MenuScene(params Element[] elements) {
			this.elements = new List<Element>(elements);

			Button lastButton = null;
			foreach (var element in elements) {
				if (element.GetType() == typeof(Button)) {
					if (currentButton == null) {
						currentButton = (Button) element;
					} else {
						lastButton = currentButton;
						while (lastButton.next != null) {
							lastButton = lastButton.next;
						}
						lastButton.next = (Button)element;
						lastButton = lastButton.next;
					}
				}
			}

			if (lastButton == null) lastButton = currentButton;

			if (currentButton != null) { // there is a button
				//loop the list
				lastButton.next = currentButton;
				// linked list reverse
				Button j = currentButton;
				for (int i = 0; i < elements.Length; i++) {
					j.next.previous = j;
					j = j.next;
				}
			}
		}

		public void Show() {
			foreach (var element in elements) {
				element.Visible = true;
			}
			if (currentButton != null) {
				currentButton.SetState(Button.ButtonState.Highlighted);
			}
		}

		public void Hide() {
			foreach (var element in elements) {
				element.Visible = false;
			}
		}

		public void NextElement() {
			currentButton.SetState(Button.ButtonState.Normal);
			currentButton = currentButton.next;
			currentButton.SetState(Button.ButtonState.Highlighted);
		}

		public void PreviousElement() {
			currentButton.SetState(Button.ButtonState.Normal);
			currentButton = currentButton.previous;
			currentButton.SetState(Button.ButtonState.Highlighted);
		}

		public void Press() {
			if (currentButton != null) {
				currentButton.Press();
			}
		}

		public IEnumerator<Element> GetEnumerator ()
		{
			return ((IEnumerable<Element>)elements).GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<Element>)elements).GetEnumerator ();
		}
	}
}