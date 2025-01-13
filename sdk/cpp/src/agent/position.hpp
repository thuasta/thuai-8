#pragma once
#ifndef _THUAI8_AGENT_POSITION_HPP_
#define _THUAI8_AGENT_POSITION_HPP_

#include <spdlog/fmt/bundled/format.h>

#include <cstdlib>
#include <type_traits>

constexpr double epsilon{1e-6};

namespace thuai8_agent {

template <class T>
  requires std::is_same_v<T, int> || std::is_same_v<T, double>
struct Position {
  T x{};
  T y{};
  double angle{};
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
struct fmt::formatter<thuai8_agent::Position<T>> : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::Position<T>& obj,
                     format_context& ctx) {
    return fmt::format_to(ctx.out(), "Position: {{x: {}, y: {}, angle: {}}}",
                          obj.x, obj.y, obj.angle);
  }
};

#endif  // _THUAI8_AGENT_POSITION_HPP_