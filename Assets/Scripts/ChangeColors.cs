using UnityEngine;
using System.Collections;

public class ChangeColors : MonoBehaviour {

	public Material mat;
	public enum ColorPalette { LIGHT, DARK, GAMEBOY }

	void Start () {
		SetColorScheme(ColorPalette.DARK);
	}

	public void SetColorScheme (ColorPalette colorScheme) {
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
			white = new Color(0.2f,0.2f,0.2f);
			black = new Color(0.9f,0.9f,0.9f);
			red = new Color(1f,0.4f,0.4f);
			blue = new Color(0f,0.7f,0.9f);
			break;
		case ColorPalette.GAMEBOY:
			black = new Color(156/255f,189/255f,15/255f);
			white = new Color(140/255f,173/255f,15/255f);
			blue = new Color(48/255f,98/255f,48/255f);
			red = new Color(15/255f,56/255f,15/255f);
			break;
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
