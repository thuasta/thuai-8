#pragma once
#ifndef _THUAI8_ENVIRONMENT_INFO_HPP_
#define _THUAI8_ENVIRONMENT_INFO_HPP_

#include <format>
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
  Position<double> position{};
  double speed{};
  double damage{};
  double traveledDistance{};
};

struct Laser {
  Position<double> start{};
  Position<double> end{};
};

struct EnvironmentInfo {
  unsigned int mapSize{};
  std::vector<Wall> walls;
  std::vector<Fence> fences;
  std::vector<Bullet> bullets;
};

}  // namespace thuai8_agent

template <>
struct std::formatter<thuai8_agent::Wall> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::Wall& object, FormatContext& ctx) const {
    return format_to(ctx.out(), "Wall[{}]", object.position);
  }
};

template <>
struct std::formatter<thuai8_agent::Fence> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::Fence& object, FormatContext& ctx) const {
    return format_to(ctx.out(), "Fence[{}, health: {}]", object.position,
                     object.health);
  }
};

template <>
struct std::formatter<thuai8_agent::Bullet> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::Bullet& object, FormatContext& ctx) const {
    return format_to(
        ctx.out(), "Bullet[{}, speed: {}, damage: {}, traveledDistance: {}]",
        object.position, object.speed, object.damage, object.traveledDistance);
  }
};

template <>
struct std::formatter<thuai8_agent::EnvironmentInfo>
    : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::EnvironmentInfo& object,
              FormatContext& ctx) const {
    return format_to(
        ctx.out(),
        "EnvironmentInfo[Walls: {{{}}}, Fences: {{{}}}, Bullets: {{{}}}]",
        object.walls, object.fences, object.bullets);
  };
};

#endif  // _THUAI8_ENVIRONMENT_INFO_HPP_