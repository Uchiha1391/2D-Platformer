using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MoneyCounterUI : MonoBehaviour
{
    [SerializeField]
    private Text moneyText;

    // Start is called before the first frame update
    void Awake()
    {
        moneyText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = (GameMaster.Money).ToString();
    }
}
