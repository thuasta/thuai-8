#pragma once
#ifndef _THUAI8_AGENT_GAME_STATISTICS_HPP_
#define _THUAI8_AGENT_GAME_STATISTICS_HPP_

#include <cstdint>
#include <format>
#include <magic_enum/magic_enum.hpp>
#include <string>
#include <vector>

namespace thuai8_agent {

enum class Stage : uint8_t { Rest, Battle, End };

struct OnesScore {
  std::string token;
  unsigned int score;
};

struct GameStatistics {
  Stage currentStage{};
  unsigned int countDown{};
  unsigned int ticks{};
  std::vector<OnesScore> scores;
};

}  // namespace thuai8_agent

template <>
struct std::formatter<thuai8_agent::Stage> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(thuai8_agent::Stage object, FormatContext& ctx) const {
    return format_to(ctx.out(), "{}", magic_enum::enum_name(object));
  }
};

template <>
struct std::formatter<thuai8_agent::OnesScore> : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::OnesScore& object, FormatContext& ctx) const {
    return format_to(ctx.out(), "Token {} : Score {}", object.token,
                     object.score);
  }
};

template <>
struct std::formatter<thuai8_agent::GameStatistics>
    : std::formatter<std::string> {
  template <class FormatContext>
  auto format(const thuai8_agent::GameStatistics& object,
              FormatContext& ctx) const {
    return format_to(ctx.out(),
                     "GameStatistics[Stage: {}, CountDown: {}, Ticks: {}, "
                     "Scores: ({} : {} = {} : {})]",
                     object.currentStage, object.countDown, object.ticks,
                     object.scores[0].token, object.scores[1].token,
                     object.scores[0].score, object.scores[1].score);
  }
};

#endif  // _THUAI8_AGENT_GAME_STATISTICS_HPP_