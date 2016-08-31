using UnityEngine;
using System.Collections;

public class ChangeColors : MonoBehaviour {

	public Material mat;
	enum ColorPalette { LIGHT, DARK }
	[SerializeField] ColorPalette colorScheme;

	void Start () {
		Color white = Color.white;
		Color black = Color.black;
		Color red = Color.red;
		Color blue = Color.blue;

		switch (colorScheme) {
		case ColorPalette.DARK:
			white = new Color(0.9f,0.9f,0.9f);
			black = new Color(0.2f,0.3f,0.4f);
			red = new Color(1f,0.4f,0.4f);
			blue = new Color(0f,0.7f,0.9f);
			break;
		case ColorPalette.LIGHT:
			white = new Color(0.3f,0.2f,0.2f);
			black = new Color(0.9f,0.9f,0.9f);
			red = new Color(1f,0.4f,0.4f);
			blue = new Color(0f,0.7f,0.9f);
			break;
		/*case ColorPalette.LIGHT:
			white = new Color(0.3f,0.2f,0.2f);
			black = new Color(0.9f,0.9f,0.9f);
			red = new Color(1f,0.4f,0.4f);
			blue = new Color(0f,0.7f,0.9f);
			break;*/
		default:
			break;
		}
		mat.SetVector("_White", white);
		mat.SetVector("_Black", black);
		mat.SetVector("_Red", red);
		mat.SetVector("_Blue", blue);
	}
	
	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Graphics.Blit(src, dest, mat);
	}
}
