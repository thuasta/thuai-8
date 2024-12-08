#pragma once
#ifndef _THUAI8_AGENT_POSITION_HPP_
#define _THUAI8_AGENT_POSITION_HPP_

#include <cstdlib>
#include <format>
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
  if constexpr (std::is_same_v<T, double> && std::is_same_v<U, double>) {
    return std::abs(lhs.x - rhs.x) < epsilon &&
           std::abs(lhs.y - rhs.y) < epsilon;
  } else {
    return lhs.x == rhs.x && lhs.y == rhs.y;
  }
}

}  // namespace thuai8_agent

template <class T>
  requires std::is_same_v<T, int> || std::is_same_v<T, double>
struct std::formatter<thuai8_agent::Position<T>> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::Position<T>& object,
              FormatContext& ctx) const {
    return format_to(ctx.out(), "Position: [x: {}, y: {}, angle: {}]", object.x,
                     object.y, object.angle);
  }
};

#endif  // _THUAI8_AGENT_POSITION_HPP_