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
std::vector<thuai8_agent::Position<int>> path;
// Your global variables here
// But notice that you should avoid using global variables as much as possible
//
// NOLINTEND(cppcoreguidelines-avoid-non-const-global-variables)

//  Your functions here
//
}  // namespace

// NOLINTBEGIN(misc-use-internal-linkage)
void SelectBuff(const thuai8_agent::Agent& agent) {
  const auto& self_info{agent.self_info()};
  const auto& available_buffs{agent.available_buffs()};
  // Your code here
  //
  for (auto buff : available_buffs) {
    if (std::ranges::find_if(self_info.skills, [buff](auto skill) {
          return buff == skill.name;
        }) == self_info.skills.end()) {
      agent.SelectBuff(buff);
      return;
    }
  }
}

void Loop(const thuai8_agent::Agent& agent) {
  const auto& self_info{agent.self_info()};
  const auto& opponent_info{agent.opponent_info()};
  const auto& walls{agent.environment_info().walls};
  const auto& fences{agent.environment_info().fences};
  const auto& bullets{agent.environment_info().bullets};
  const auto& game_statistics{agent.game_statistics()};
  // Your code here
  //
  agent.MoveForward();
  agent.MoveBackward();
  agent.TurnClockwise();
  agent.TurnCounterClockwise();
  agent.Attack();
  agent.UseSkill(thuai8_agent::SkillKind::Flash);

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
    spdlog::debug("Found path from {} to {}:", self_info.position,
                  opponent_info.position);
  }
  while (path.back() != opponent_position_int) {
    path.pop_back();
  }
  if (auto size = path.size(); size > 1) {
    agent.MoveForward();
  }
}
// NOLINTEND(misc-use-internal-linkage)
