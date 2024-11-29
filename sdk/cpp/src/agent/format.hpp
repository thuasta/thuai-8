#pragma once
#ifndef _THUAI8_AGENT_FORMAT_HPP_
#define _THUAI8_AGENT_FORMAT_HPP_

#include <fmt/format.h>
#include <fmt/ranges.h>

#include <magic_enum.hpp>
#include <string>
#include <type_traits>

#include "agent.hpp"
#include "environment_info.hpp"
#include "game_statistics.hpp"
#include "player_info.hpp"
#include "position.hpp"

namespace thuai8_agent {

class Agent;  // Forward declaration to avoid circular dependency
auto format_as(const Agent& object) -> std::string;

template <typename T>
  requires magic_enum::is_scoped_enum_v<T>
auto format_as(T object) -> std::string {
  return std::string(magic_enum::enum_name(object));
}

template <typename T>
  requires std::is_same_v<T, int> || std::is_same_v<T, double>
auto format_as(const Position<T>& object) -> std::string {
  return fmt::format("Position: [x: {}, y: {}, angle: {}]", object.x, object.y,
                     object.angle);
}

auto format_as(const Weapon& object) -> std::string;

auto format_as(const Armor& object) -> std::string;

auto format_as(const Skill& object) -> std::string;

auto format_as(const PlayerInfo& object) -> std::string;

auto format_as(const Wall& object) -> std::string;

auto format_as(const Fence& object) -> std::string;

auto format_as(const Bullet& object) -> std::string;

auto format_as(const EnvironmentInfo& object) -> std::string;

auto format_as(const OnesScore& object) -> std::string;

auto format_as(const GameStatistics& object) -> std::string;

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_FORMAT_HPP_