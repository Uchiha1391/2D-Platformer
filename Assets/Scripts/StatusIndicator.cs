using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private Text healthText;

    void Start()
    {
        if(healthBarRect==null)
        {
            Debug.LogError("No health bar object referenced!");
        }
        if (healthText == null)
        {
            Debug.LogError("No health bar object referenced!");
        }
    }

    public void SetHealth(int _cur,int _max)
    {
        float _value = (float)_cur / _max;

        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = _cur + "/" + _max + " HP";
    }
    public void SetColour(byte r,byte g,byte b,byte a=255)
    {
        healthBarRect.GetComponent<Image>().color = new Color32(r, g, b, a);
    }
    public Color32 GetColour()
    {
        return healthBarRect.GetComponent<Image>().color;
    }
}
