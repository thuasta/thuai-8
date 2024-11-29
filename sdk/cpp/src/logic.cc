#include <spdlog/spdlog.h>

#include <algorithm>
#include <vector>

#include "agent/agent.hpp"
#include "agent/environment_info.hpp"
#include "agent/format.hpp"
#include "agent/player_info.hpp"
#include "agent/position.hpp"
#include "path_finding.hpp"

namespace {
// NOLINTBEGIN(cppcoreguidelines-avoid-non-const-global-variables)
// Your global variables here
// But notice that you should avoid using global variables as much as possible
std::vector<thuai8_agent::Position<int>> path;
//
// NOLINTEND(cppcoreguidelines-avoid-non-const-global-variables)

//  Your functions here
//
}  // namespace

// NOLINTBEGIN(misc-use-internal-linkage)
void SelectBuff(const thuai8_agent::Agent& agent) {
  // Your code here
  // Here is an example of how to select a buff
  const auto& self_info{agent.self_info()};
  const auto& available_buffs{agent.available_buffs()};
  for (auto buff : available_buffs) {
    if (std::ranges::none_of(self_info.skills, [buff](auto skill) {
          return buff == skill.name;
        })) {
      agent.SelectBuff(buff);
      return;
    }
  }
  agent.SelectBuff(available_buffs.front());
}

void Loop(const thuai8_agent::Agent& agent) {
  // Your code here
  // Here is an example of how to move your agent
  const auto& self_info{agent.self_info()};
  const auto& opponent_info{agent.opponent_info()};
  const auto& walls{agent.environment_info().walls};
  const auto& fences{agent.environment_info().fences};
  const auto& bullets{agent.environment_info().bullets};
  const auto& game_statistics{agent.game_statistics()};

  agent.MoveForward();
  agent.MoveBackward();
  agent.TurnClockwise();
  agent.TurnCounterClockwise();
  agent.Attack();

  if (!self_info.skills.empty()) {
    agent.UseSkill(self_info.skills.front().name);
  }

  thuai8_agent::Position self_position_int{
      .x = static_cast<int>(self_info.position.x),
      .y = static_cast<int>(self_info.position.y)};
  thuai8_agent::Position opponent_position_int{
      .x = static_cast<int>(opponent_info.position.x),
      .y = static_cast<int>(opponent_info.position.y)};
  if (std::ranges::find(path, self_position_int) == path.end() ||
      std::ranges::find(path, opponent_position_int) == path.end()) {
    path = FindPathBFS(self_position_int, opponent_position_int, walls, fences);
    if (path.empty()) {
      spdlog::debug("No path from {} to {}", self_info.position,
                    opponent_info.position);
      return;
    }
    spdlog::debug("Found path from {} to {}", self_info.position,
                  opponent_info.position);
  }
  while (path.back() != self_position_int) {
    path.pop_back();
  }
  if (auto size = path.size(); size > 1) {
    agent.MoveForward();
  }
}
// NOLINTEND(misc-use-internal-linkage)
