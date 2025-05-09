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
        {"BULLET_COUNT", "整备"},
        {"BULLET_SPEED", "鹰眼"},
        {"ATTACK_SPEED", "连弩"},
        {"LASER", "激光"},
        {"DAMAGE", "重击"},
        {"ANTI_ARMOR", "破甲"},
        {"ARMOR", "铁壁"},
        {"REFLECT", "借箭"},
        {"DODGE", "八卦"},
        {"KNIFE", "名刀"},
        {"GRAVITY", "力场"},
        {"BLACK_OUT", "磁暴"},
        {"SPEED_UP", "疾跑"},
        {"FLASH", "闪现"},
        {"DESTROY", "破竹"},
        {"CONSTRUCT", "围界"},
        {"TRAP", "网罗"},
        {"RECOVER", "复苏"},
        {"KAMUI", "神威"}
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