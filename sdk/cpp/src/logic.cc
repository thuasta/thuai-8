#include <spdlog/spdlog.h>

#include <algorithm>
#include <vector>

#include "agent/agent.hpp"
#include "agent/environment_info.hpp"
#include "agent/format.hpp"
#include "agent/player_info.hpp"

namespace {
// NOLINTBEGIN(cppcoreguidelines-avoid-non-const-global-variables)
// Your global variables here
// But notice that you should avoid using global variables as much as possible
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
  // Here is an example of how to use the agent's functions
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

  if (!self_info.skills.empty() &&
      self_info.skills.front().currentCoolDown == 0) {
    agent.UseSkill(self_info.skills.front().name);
  }
}
// NOLINTEND(misc-use-internal-linkage)
