#pragma once
#ifndef _THUAI8_ENVIRONMENT_INFO_HPP_
#define _THUAI8_ENVIRONMENT_INFO_HPP_

#include <spdlog/fmt/bundled/format.h>
#include <spdlog/fmt/bundled/ranges.h>

#include <vector>

#include "agent/position.hpp"

namespace thuai8_agent {

struct Wall {
  Position<int> position{};
};

struct Fence {
  Position<int> position{};
  unsigned int health{};
};

struct Bullet {
  unsigned int no{};
  bool isMissile{};
  bool isAntiArmor{};
  Position<double> position{};
  double speed{};
  double damage{};
  double traveledDistance{};
};

struct EnvironmentInfo {
  unsigned int mapSize{};
  std::vector<Wall> walls;
  std::vector<Fence> fences;
  std::vector<Bullet> bullets;
};

}  // namespace thuai8_agent

template <>
struct fmt::formatter<thuai8_agent::Wall> : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::Wall& obj, format_context& ctx) {
    return fmt::format_to(ctx.out(), "Wall: {{{}}}", obj.position);
  }
};

template <>
struct fmt::formatter<thuai8_agent::Fence> : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::Fence& obj, format_context& ctx) {
    return fmt::format_to(ctx.out(), "Fence: {{{}, Health: {}}}", obj.position,
                          obj.health);
  }
};

template <>
struct fmt::formatter<thuai8_agent::Bullet> : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::Bullet& obj, format_context& ctx) {
    return fmt::format_to(ctx.out(),
                          "Bullet: {{No: {}, IsMissile{}, IsAntiArmor{}, {}, "
                          "Speed: {}, Damage: {}, TraveledDistance: {}}}",
                          obj.no, obj.isMissile, obj.isAntiArmor, obj.position,
                          obj.speed, obj.damage, obj.traveledDistance);
  }
};

template <>
struct fmt::formatter<thuai8_agent::EnvironmentInfo>
    : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::EnvironmentInfo& obj,
                     format_context& ctx) {
    return fmt::format_to(
        ctx.out(),
        "EnvironmentInfo: {{MapSize: {}, Walls: {}, Fences: {}, Bullets: {}}}",
        obj.mapSize, obj.walls, obj.fences, obj.bullets);
  };
};

#endif  // _THUAI8_ENVIRONMENT_INFO_HPP_