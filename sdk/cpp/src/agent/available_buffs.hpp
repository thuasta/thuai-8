#pragma once
#ifndef _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_
#define _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_

#include <cstdint>
#include <format>
#include <magic_enum/magic_enum.hpp>
#include <type_traits>
#include <vector>

namespace thuai8_agent {

enum class SkillKind : std::uint8_t;  // Forward declaration

enum class BuffKind : std::uint8_t {
  BlackOut,
  SpeedUp,
  Flash,
  Destroy,
  Construct,
  Trap,
  Missile,
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

template <typename T, typename U>
  requires(std::is_same_v<T, BuffKind> && std::is_same_v<U, SkillKind>) ||
          (std::is_same_v<T, SkillKind> && std::is_same_v<U, BuffKind>)
constexpr auto operator==(T lhs, U rhs) -> bool {
  return static_cast<std::uint8_t>(lhs) == static_cast<std::uint8_t>(rhs);
}

using AvailableBuffs = std::vector<BuffKind>;

}  // namespace thuai8_agent

template <>
struct std::formatter<thuai8_agent::BuffKind> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(thuai8_agent::BuffKind object, FormatContext& ctx) const {
    return format_to(ctx.out(), "{}", magic_enum::enum_name(object));
  }
};

template <>
struct std::formatter<thuai8_agent::AvailableBuffs>
    : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::AvailableBuffs& object,
              FormatContext& ctx) const {
    return format_to(ctx.out(), "AvailableBuffs{}", object);
  }
};

#endif  // _THUAI8_AGENT_AVAILABLE_BUFFS_HPP_