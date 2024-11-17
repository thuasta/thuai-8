#pragma once
#ifndef _THUAI8_AGENT_POSITION_HPP_
#define _THUAI8_AGENT_POSITION_HPP_

namespace thuai8_agent {

struct Position {
  double x{};
  double y{};
  double angle{};
};

constexpr auto operator==(const Position& lhs, const Position& rhs) -> bool {
  return lhs.x == rhs.x && lhs.y == rhs.y;
}

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_POSITION_HPP_