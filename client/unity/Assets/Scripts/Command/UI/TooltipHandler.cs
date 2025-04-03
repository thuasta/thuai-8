using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltipPanel; // 提示框面板
    [SerializeField] private TextMeshProUGUI tooltipText; // 提示文本组件
    [SerializeField] private string tooltipMessage = "buff"; // 默认提示信息
    private Vector2 offset = new Vector2(0, -35); // 提示框偏移量
    private readonly Dictionary<string, string> _constantDict = new Dictionary<string, string>
    {
        {"BULLET_COUNT", "子弹数量增加"},
        {"BULLET_SPEED", "子弹移速增加"},
        {"ATTACK_SPEED", "攻速增加"},
        {"LASER", "激光"},
        {"DAMAGE", "伤害增加"},
        {"ANTI_ARMOR", "破甲"},
        {"ARMOR", "护盾"},
        {"REFLECT", "反弹盾"},
        {"DODGE", "闪避率增加"},
        {"KNIFE", "名刀"},
        {"GRAVITY", "重力场"},
        {"BLACK_OUT", "视野限制"},
        {"SPEED_UP", "加速"},
        {"FLASH", "闪现"},
        {"DESTROY", "破坏墙体"},
        {"CONSTRUCT", "建造墙体"},
        {"TRAP", "陷阱"},
        {"MISSILE", "导弹"},
        {"KAMUI", "虚化"}
    };

    // 鼠标进入时触发
    public void OnPointerEnter(PointerEventData eventData)
    {
        Image image = GetComponent<Image>();
        if(image != null && image.color.a != 0)
        {
            tooltipPanel.SetActive(true);
            tooltipText.text = _constantDict[image.sprite.name];
            Vector2 spawnPosition = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);
            tooltipPanel.GetComponent<RectTransform>().position = spawnPosition;
        }
    }

    // 鼠标退出时触发
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPanel.SetActive(false);
    }

}