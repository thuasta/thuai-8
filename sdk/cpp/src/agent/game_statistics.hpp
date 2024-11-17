#pragma once
#ifndef _THUAI8_AGENT_GAME_STATISTICS_HPP_
#define _THUAI8_AGENT_GAME_STATISTICS_HPP_

#include <cstdint>
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

#endif  // _THUAI8_AGENT_GAME_STATISTICS_HPP_