using UnityEngine;
using UnityEngine.UI;

public class SecondaryObjectiveSocket : MonoBehaviour
{
	public delegate void GemSocketDelegate(Gem gem, SecondaryObjectiveSocket socket);
	public event GemSocketDelegate OnGemInstalled;

	public SecondaryObjectivePanel Owner { get; private set; }
	public bool IsActive { get; private set; }

	[field: SerializeField] public ObjectColor SocketColor { get; private set; }
	[SerializeField] int gemCapacity = 1;
	[SerializeField] Image fullImage;

	int socketedGems = 0;
	Image socketFrameRenderer, socketGemRenderer;
	AudioSource audioSource;
	public bool IsFull { get => socketedGems == gemCapacity; }

	private void Awake()
	{
		Image[] spriteRenderers = GetComponentsInChildren<Image>();

		Owner = GetComponentInParent<SecondaryObjectivePanel>();

		socketFrameRenderer = spriteRenderers[0];
		socketGemRenderer = spriteRenderers[2];
		audioSource = GetComponent<AudioSource>();
		if (audioSource)
		{
			audioSource.playOnAwake = false;
			audioSource.loop = false;
		}
	}

	public void InstallGem(Gem gem)
	{
		socketedGems += 1;
		if (socketGemRenderer)
		{
			socketGemRenderer.color = GetSocketColor(gem.GemColor);
			socketGemRenderer.fillAmount = (float)socketedGems / gemCapacity;
		}
		if (audioSource) audioSource.Play();
		if (IsFull && fullImage) fullImage.gameObject.SetActive(true);
		OnGemInstalled?.Invoke(gem, this);


        //GemPool.Instance.ReturnToPool(gem);
	}

	public void InitializeGemSocket(int capacity, ObjectColor color)
	{
		gemCapacity = capacity;
		SocketColor = color;
		socketedGems = 0;

		if (socketFrameRenderer)
			socketFrameRenderer.color = GetSocketColor(SocketColor);

		if (socketGemRenderer)
			socketGemRenderer.color = Color.black;

		if (fullImage)
		{
			fullImage.sprite = SecondaryObjectiveManager.Instance.GetAssignedSprite(color);
		}
		IsActive = true;
	}

	Color GetSocketColor(ObjectColor color)
	{
		switch (color)
		{
			case ObjectColor.RED:
				return Color.red;

			case ObjectColor.BLUE:
				return Color.blue;

			case ObjectColor.GREEN:
				return Color.green;

			case ObjectColor.YELLOW:
				return Color.yellow;

			case ObjectColor.NONE:
				return Color.white;
			default:
				return Color.black;
		}
	}

	public void ResetSocket()
	{
		SocketColor = ObjectColor.NONE;
		if (socketFrameRenderer)
			socketFrameRenderer.color = GetSocketColor(SocketColor);
		if (socketGemRenderer)
			socketFrameRenderer.color = Color.black;
		if (fullImage)
		{
			fullImage.sprite = null;
			fullImage.gameObject.SetActive(false);
		}
		gameObject.SetActive(false);
		IsActive = false;
	}
	private void OnDestroy()
	{
		//OnColorMatch -= InstallGem;
	}
}