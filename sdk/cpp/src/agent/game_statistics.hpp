#pragma once
#ifndef _THUAI8_AGENT_GAME_STATISTICS_HPP_
#define _THUAI8_AGENT_GAME_STATISTICS_HPP_

#include <spdlog/fmt/bundled/format.h>

#include <cstdint>
#include <magic_enum/magic_enum.hpp>
#include <string>
#include <vector>

namespace thuai8_agent {

enum class Stage : std::uint8_t { Rest, Battle, End };

struct OnesScore {
  std::string token;
  unsigned int score{};
};

struct GameStatistics {
  Stage currentStage{};
  unsigned int countDown{};
  unsigned int ticks{};
  std::vector<OnesScore> scores;
};

}  // namespace thuai8_agent

template <>
struct fmt::formatter<thuai8_agent::Stage> : fmt::formatter<std::string> {
  static auto format(thuai8_agent::Stage obj, format_context& ctx) {
    return fmt::format_to(ctx.out(), "{}", magic_enum::enum_name(obj));
  }
};

template <>
struct fmt::formatter<thuai8_agent::OnesScore> : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::OnesScore& obj, format_context& ctx) {
    return fmt::format_to(ctx.out(), "Token({}).Score({})", obj.token,
                          obj.score);
  }
};

template <>
struct fmt::formatter<thuai8_agent::GameStatistics>
    : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::GameStatistics& obj,
                     format_context& ctx) {
    return fmt::format_to(
        ctx.out(),
        "GameStatistics: {{Stage: {}, CountDown: {}, Ticks: {}, "
        "Scores: ({} : {} = {} : {})}}",
        obj.currentStage, obj.countDown, obj.ticks, obj.scores[0].token,
        obj.scores[1].token, obj.scores[0].score, obj.scores[1].score);
  }
};

#endif  // _THUAI8_AGENT_GAME_STATISTICS_HPP_