#pragma once
#ifndef _THUAI8_AGENT_PLAYER_INFO_HPP_
#define _THUAI8_AGENT_PLAYER_INFO_HPP_

#include <spdlog/fmt/bundled/format.h>
#include <spdlog/fmt/bundled/ranges.h>

#include <cstdint>
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
  Recover,
  Kamui
};

struct Weapon {
  double attackSpeed{};
  double bulletSpeed{};
  bool isLaser{};
  bool antiArmor{};
  unsigned int damage{};
  unsigned int maxBullets{};
  unsigned int currentBullets{};
};

struct Armor {
  bool canReflect{};
  bool gravityField{};
  unsigned int armorValue{};
  int health{};
  double dodgeRate{};
  ArmorKnifeState knife{};
};

struct Skill {
  SkillKind name{};
  unsigned int maxCoolDown{};
  unsigned int currentCoolDown{};
  bool isActive{};
};

struct Player {
  std::string token;
  Position<double> position{};
  Weapon weapon{};
  Armor armor{};
  std::vector<Skill> skills;
};

using Players = std::vector<Player>;

}  // namespace thuai8_agent

template <>
struct fmt::formatter<thuai8_agent::ArmorKnifeState>
    : fmt::formatter<std::string> {
  static auto format(thuai8_agent::ArmorKnifeState obj, format_context& ctx) {
    return fmt::format_to(ctx.out(), "{}", magic_enum::enum_name(obj));
  }
};

template <>
struct fmt::formatter<thuai8_agent::SkillKind> : fmt::formatter<std::string> {
  static auto format(thuai8_agent::SkillKind obj, format_context& ctx) {
    return fmt::format_to(ctx.out(), "{}", magic_enum::enum_name(obj));
  }
};

template <>
struct fmt::formatter<thuai8_agent::Weapon> : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::Weapon& obj, format_context& ctx) {
    return fmt::format_to(
        ctx.out(),
        "Weapon: {{AttackSpeed: {}, BulletSpeed: {}, IsLaser: {}, AntiArmor: "
        "{}, Damage: {}, MaxBullets: {}, CurrentBullets: {}}}",
        obj.attackSpeed, obj.bulletSpeed, obj.isLaser, obj.antiArmor,
        obj.damage, obj.maxBullets, obj.currentBullets);
  }
};

template <>
struct fmt::formatter<thuai8_agent::Armor> : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::Armor& obj, format_context& ctx) {
    return fmt::format_to(
        ctx.out(),
        "Armor: {{CanReflect: {}, GravityField: {}, ArmorValue: {}, Health: "
        "{}, DodgeRate: {}, Knife: {}}}",
        obj.canReflect, obj.gravityField, obj.armorValue, obj.health,
        obj.dodgeRate, obj.knife);
  }
};

template <>
struct fmt::formatter<thuai8_agent::Skill> : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::Skill& obj, format_context& ctx) {
    return fmt::format_to(ctx.out(),
                          "Skill: {{Name: {}, MaxCoolDown: {}, "
                          "CurrentCoolDown: {}, IsActive: {}}}",
                          obj.name, obj.maxCoolDown, obj.currentCoolDown,
                          obj.isActive);
  }
};

template <>
struct fmt::formatter<thuai8_agent::Player> : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::Player& obj, format_context& ctx) {
    return fmt::format_to(
        ctx.out(), "Player: {{Token: {}, {}, {}, {}, Skills: {}}}", obj.token,
        obj.position, obj.weapon, obj.armor, obj.skills);
  }
};

#endif  // _THUAI8_AGENT_PLAYER_INFO_HPP_