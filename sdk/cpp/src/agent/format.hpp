#pragma once
#ifndef _THUAI8_AGENT_FORMAT_HPP_
#define _THUAI8_AGENT_FORMAT_HPP_

#include <fmt/format.h>
#include <fmt/ranges.h>

#include <magic_enum.hpp>
#include <string>

#include "agent.hpp"
#include "available_buffs.hpp"
#include "environment_info.hpp"
#include "game_statistics.hpp"
#include "player_info.hpp"
#include "position.hpp"

namespace thuai8_agent {

class Agent;  // Forward declaration to avoid circular dependency
auto format_as(const Agent& object) -> std::string;

auto format_as(const Position& object) -> std::string;

auto format_as(Stage object) -> std::string;

auto format_as(BuffKind object) -> std::string;

auto format_as(ArmorKnifeState object) -> std::string;

auto format_as(SkillKind object) -> std::string;

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