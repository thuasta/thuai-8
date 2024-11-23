#pragma once
#ifndef _THUAI8_AGENT_POSITION_HPP_
#define _THUAI8_AGENT_POSITION_HPP_

#include <cstdlib>
#include <type_traits>

constexpr double epsilon{1e-6};

namespace thuai8_agent {

template <class T>
  requires std::is_same_v<T, int> || std::is_same_v<T, double>
struct Position {
  T x{};
  T y{};
  T angle{};
};

template <typename T, typename U>
  requires(std::is_same_v<T, int> || std::is_same_v<T, double>) &&
          (std::is_same_v<U, int> || std::is_same_v<U, double>)
constexpr auto operator==(const Position<T>& lhs, const Position<U>& rhs)
    -> bool {
  if constexpr (std::is_same_v<T, int> && std::is_same_v<U, int>) {
    return lhs.x == rhs.x && lhs.y == rhs.y;
  } else if constexpr (std::is_same_v<T, int> && std::is_same_v<U, double>) {
    return lhs.x == static_cast<int>(rhs.x) && lhs.y == static_cast<int>(rhs.y);
  } else if constexpr (std::is_same_v<T, double> && std::is_same_v<U, int>) {
    return static_cast<int>(lhs.x) == rhs.x && static_cast<int>(lhs.y) == rhs.y;
  } else {
    return std::abs(lhs.x - rhs.x) < epsilon &&
           std::abs(lhs.y - rhs.y) < epsilon;
  }
}

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_POSITION_HPP_