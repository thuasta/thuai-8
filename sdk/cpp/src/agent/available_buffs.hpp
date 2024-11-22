#pragma once
#ifndef _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_
#define _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_

#include <cstdint>

namespace thuai8_agent {

enum class BuffKind : std::uint8_t {
  BulletCount,  // 子弹数量
  BulletSpeed,  // 子弹速度
  AttackSpeed,  // 攻击速度
  Laser,        // 激光
  Damage,       // 伤害
  AntiArmor,    // 破甲
  Armor,        // 护盾
  Reflect,      // 反弹
  Dodge,        // 闪避
  Knife,        // 名刀
  Gravity,      // 重力
  BlackOut,     // 视野限制
  SpeedUp,      // 加速
  Flash,        // 闪现
  Destroy,      // 破坏墙体
  Construct,    // 建造墙体
  Trap,         // 陷阱
  Missile,      // 导弹
  Kamui         // 虚化
};

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_