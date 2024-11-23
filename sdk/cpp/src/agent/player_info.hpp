#pragma once
#ifndef _THUAI8_AGENT_PLAYER_INFO_HPP_
#define _THUAI8_AGENT_PLAYER_INFO_HPP_

#include <cstdint>
#include <string>
#include <vector>

#include "agent/position.hpp"

namespace thuai8_agent {

enum class ArmorKnifeState : std::uint8_t {
  NotOwned,
  Available,
  Active,
  Broken
};

enum class SkillKind : std::uint8_t {
  BlackOut,   // 视野限制
  SpeedUp,    // 加速
  Flash,      // 闪现
  Destroy,    // 破坏墙体
  Construct,  // 建造墙体
  Trap,       // 陷阱
  Missile,    // 导弹
  Kamui       // 虚化
};

struct Weapon {
  bool isLaser{};    // 是否为激光
  bool antiArmor{};  // 是否破甲
  unsigned int damage{};
  unsigned int maxBullets{};      // 子弹最大存在数量
  unsigned int currentBullets{};  // 当前存在的子弹数量
  double attackSpeed{};           // 攻击速度
  double bulletSpeed{};           // 子弹速度
};

struct Armor {
  bool canReflect{};          // 是否可以反弹
  bool gravityField{};        // 是否重力场
  unsigned int armorValue{};  // 护盾值
  unsigned int health{};      // 血量
  double dodgeRate{};         // 闪避率
  ArmorKnifeState knife{};    // 名刀
};

struct Skill {
  SkillKind name{};
  unsigned int maxCoolDown{};      // 最大冷却时间
  unsigned int currentCoolDown{};  // 当前冷却时间
  bool isActive{};                 // 是否正在生效
};

struct PlayerInfo {
  std::string token;
  Position<double> position{};
  Weapon weapon{};
  Armor armor{};
  std::vector<Skill> skills;
};

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_PLAYER_INFO_HPP_