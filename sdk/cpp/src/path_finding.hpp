#pragma once
#ifndef _PATH_FINDING_HPP_
#define _PATH_FINDING_HPP_

#include <span>
#include <vector>

#include "agent/environment_info.hpp"
#include "agent/position.hpp"

auto FindPathBFS(const thuai8_agent::Position<int>& start,
                 const thuai8_agent::Position<int>& end,
                 std::span<const thuai8_agent::Wall> walls,
                 std::span<const thuai8_agent::Fence> fences)
    -> std::vector<thuai8_agent::Position<int>>;

#endif  // _PATH_FINDING_HPP_