#pragma once
#ifndef _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_
#define _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_

#include <cstdint>
#include <type_traits>

#include "player_info.hpp"

namespace thuai8_agent {

enum class BuffKind : std::uint8_t {
  BlackOut,     // 视野限制
  SpeedUp,      // 加速
  Flash,        // 闪现
  Destroy,      // 破坏墙体
  Construct,    // 建造墙体
  Trap,         // 陷阱
  Missile,      // 导弹
  Kamui,        // 虚化
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
  Gravity       // 重力
};

template <typename T, typename U>
  requires(std::is_same_v<T, BuffKind> && std::is_same_v<U, SkillKind>) ||
          (std::is_same_v<T, SkillKind> && std::is_same_v<U, BuffKind>)
constexpr auto operator==(T lhs, U rhs) -> bool {
  return static_cast<std::uint8_t>(lhs) == static_cast<std::uint8_t>(rhs);
}

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_