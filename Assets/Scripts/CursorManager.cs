using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    [SerializeField] private Texture2D[] cursorTextures;

    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

    private int activeTexture;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        activeTexture = -1;
    }

    void Start()
    {
        SetCursor(0);
    }

    public void SetCursor(int cursorIndex)
    {
        if (cursorTextures.Length <= cursorIndex || cursorIndex < 0 || activeTexture == cursorIndex)
            return;
        if (cursorTextures[cursorIndex] == null)
            return;
        Cursor.SetCursor(cursorTextures[cursorIndex], cursorHotspot, cursorMode);
        activeTexture = cursorIndex;
    }
}
