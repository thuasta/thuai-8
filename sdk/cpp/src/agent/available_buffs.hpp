#pragma once
#ifndef _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_
#define _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_

#include <spdlog/fmt/bundled/format.h>

#include <cstdint>
#include <magic_enum/magic_enum.hpp>
#include <vector>

namespace thuai8_agent {

enum class SkillKind : std::uint8_t;

enum class BuffKind : std::uint8_t {
  BlackOut,
  SpeedUp,
  Flash,
  Destroy,
  Construct,
  Trap,
  Recover,
  Kamui,
  BulletCount,
  BulletSpeed,
  AttackSpeed,
  Laser,
  Damage,
  AntiArmor,
  Armor,
  Reflect,
  Dodge,
  Knife,
  Gravity
};

constexpr auto operator==(BuffKind lhs, SkillKind rhs) -> bool {
  return static_cast<std::uint8_t>(lhs) == static_cast<std::uint8_t>(rhs);
}

using AvailableBuffs = std::vector<BuffKind>;

}  // namespace thuai8_agent

template <>
struct fmt::formatter<thuai8_agent::BuffKind> : fmt::formatter<std::string> {
  static auto format(thuai8_agent::BuffKind obj, format_context& ctx) {
    return fmt::format_to(ctx.out(), "{}", magic_enum::enum_name(obj));
  }
};

#endif  // _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_