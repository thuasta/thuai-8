#pragma once
#ifndef _THUAI8_AGENT_PLAYER_INFO_HPP_
#define _THUAI8_AGENT_PLAYER_INFO_HPP_

#include <cstdint>
#include <format>
#include <magic_enum/magic_enum.hpp>
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
  BlackOut,
  SpeedUp,
  Flash,
  Destroy,
  Construct,
  Trap,
  Missile,
  Kamui
};

struct Weapon {
  bool isLaser{};
  bool antiArmor{};
  unsigned int damage{};
  unsigned int maxBullets{};
  unsigned int currentBullets{};
  double attackSpeed{};
  double bulletSpeed{};
};

struct Armor {
  bool canReflect{};
  bool gravityField{};
  unsigned int armorValue{};
  unsigned int health{};
  double dodgeRate{};
  ArmorKnifeState knife{};
};

struct Skill {
  SkillKind name{};
  unsigned int maxCoolDown{};
  unsigned int currentCoolDown{};
  bool isActive{};
};

struct PlayerInfo {
  std::string token;
  Position<double> position{};
  Weapon weapon{};
  Armor armor{};
  std::vector<Skill> skills;
};

}  // namespace thuai8_agent

template <>
struct std::formatter<thuai8_agent::ArmorKnifeState>
    : std::formatter<std::string> {
  template <class FormatContext>
  auto format(thuai8_agent::ArmorKnifeState object, FormatContext& ctx) const {
    return format_to(ctx.out(), "{}", magic_enum::enum_name(object));
  }
};

template <>
struct std::formatter<thuai8_agent::SkillKind> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(thuai8_agent::SkillKind object, FormatContext& ctx) const {
    return format_to(ctx.out(), "{}", magic_enum::enum_name(object));
  }
};

template <>
struct std::formatter<thuai8_agent::Weapon> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::Weapon& object, FormatContext& ctx) const {
    return format_to(
        ctx.out(),
        "Weapon: [IsLaser: {},  AntiArmor: {}, Damage: {}, MaxBullets: {}, "
        "CurrentBullets: {}, AttackSpeed: {}, BulletSpeed: {}]",
        object.isLaser, object.antiArmor, object.damage, object.maxBullets,
        object.currentBullets, object.attackSpeed, object.bulletSpeed);
  }
};

template <>
struct std::formatter<thuai8_agent::Armor> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::Armor& object, FormatContext& ctx) const {
    return format_to(
        ctx.out(),
        "Armor: [CanReflect: {}, GravityField: {}, ArmorValue: {}, Health: {}, "
        "DodgeRate: {}, Knife: {}]",
        object.canReflect, object.gravityField, object.armorValue,
        object.health, object.dodgeRate, object.knife);
  }
};

template <>
struct std::formatter<thuai8_agent::Skill> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::Skill& object, FormatContext& ctx) const {
    return format_to(
        ctx.out(),
        "Skill[Name: {}, MaxCoolDown: {}, CurrentCoolDown: {}, IsActive: {}]",
        object.name, object.maxCoolDown, object.currentCoolDown,
        object.isActive);
  }
};

template <>
struct std::formatter<thuai8_agent::PlayerInfo> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::PlayerInfo& object,
              FormatContext& ctx) const {
    return format_to(ctx.out(),
                     "PlayerInfo[Token: {}, {}, {}, {}, Skills: {{{}}}]",
                     object.token, object.position, object.weapon, object.armor,
                     object.skills);
  }
};

#endif  // _THUAI8_AGENT_PLAYER_INFO_HPP_