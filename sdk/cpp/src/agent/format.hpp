#pragma once
#ifndef _THUAI8_AGENT_FORMAT_HPP_
#define _THUAI8_AGENT_FORMAT_HPP_

#include <fmt/format.h>

#include <string>

#include "agent.hpp"
#include "player_info.hpp"

namespace thuai8_agent {

class Agent; // Forward declaration to avoid circular dependency
auto format_as(const Agent& object) -> std::string;

auto format_as(SkillKind object) -> std::string;

auto format_as(BuffKind object) -> std::string;

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_FORMAT_HPP_